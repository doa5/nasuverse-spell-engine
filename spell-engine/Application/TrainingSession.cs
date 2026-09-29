using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Application
{
    public class TrainingSession
    {
        private readonly ILogger _logger;

        public TrainingSession(ILogger<TrainingSession> logger)
        {
            _logger = logger;
        }

        public Character SelectCharacter(Character aoko, Character arcueid)
        {
            Console.WriteLine("Select a character:");
            Console.WriteLine($"1. {aoko.Name}");
            Console.WriteLine($"2. {arcueid.Name}\n");

            while (true)
            {
                Console.Write("Choose a character: ");
                string characterInput = Console.ReadLine()!;
                Console.Write("\n");

                if (!int.TryParse(characterInput, out int characterChoice))
                {
                    _logger.LogWarning("Invalid character input (non-numeric): {Input}", characterInput);
                    Console.WriteLine("That's not a number. Try again.\n");
                    continue;
                }

                _logger.LogDebug("User selected character {Choice}", characterChoice);

                if (characterChoice == 1)
                {
                    return aoko;
                }
                else if (characterChoice == 2)
                {
                    return arcueid;
                }

                _logger.LogWarning("Choice out of range: {Choice}", characterChoice);
                Console.WriteLine("Please choose 1 or 2.\n");
            }
        }

        public void Run(Character selectedCharacter, WorldState world)
        {
            Console.WriteLine($"Welcome to the Taiga Dojo with {selectedCharacter.Name}!");
            Console.WriteLine($"Initial Mana: {selectedCharacter.Resources.Mana}, Dojo Durability: {world.Durability}\n");

            bool isTraining = true;
            while (isTraining)
            {
                int exitOption = DisplaySpellMenu(selectedCharacter);

                if (!TryReadSpellChoice(exitOption, out int choice))
                {
                    continue;
                }

                // Allow exit even when dojo destroyed
                if (choice == exitOption)
                {
                    _logger.LogInformation("User selected Exit");
                    isTraining = false;
                    continue;
                }

                if (world.Durability <= 0)
                {
                    _logger.LogInformation("Attempt to cast on destroyed dojo (Durability {Durability})", world.Durability);
                    Console.WriteLine("Taiga: The dojo has already been destroyed! What are you doing??\n");
                    continue;
                }

                // Dispatch by index (scales with number of spells)
                int spellIndex = choice - 1;
                if (spellIndex < 0 || spellIndex >= selectedCharacter.AvailableSpells.Count)
                {
                    _logger.LogWarning("Computed spell index out of range: {Index}", spellIndex);
                    Console.WriteLine("Invalid spell selection. Try again.\n");
                    continue;
                }

                HandleSpellCast(selectedCharacter, world, spellIndex);
                world.AdvanceTurn();

                if (world.Durability <= 0)
                {
                    Console.WriteLine("The Taiga Dojo has been destroyed!\n");
                }
            }
        }

        private static int DisplaySpellMenu(Character character)
        {
            Console.WriteLine($"--- {character.Name}'s Spells ---");
            for (int i = 0; i < character.AvailableSpells.Count; i++)
            {
                Spell spell = character.AvailableSpells[i];
                Console.WriteLine($"{i + 1}. {spell.Name} (Cost: {spell.ManaCost}, Damage: {spell.Damage})");
            }

            int exitOption = character.AvailableSpells.Count + 1;
            Console.WriteLine($"{exitOption}. Exit\n");
            return exitOption;
        }

        private bool TryReadSpellChoice(int exitOption, out int choice)
        {
            Console.Write("Choose a spell: ");
            string input = Console.ReadLine()!; // ! means "trust me, it's not null"
            Console.Write("\n");

            if (!int.TryParse(input, out choice))
            {
                _logger.LogWarning("Invalid input (non-numeric): {Input}", input);
                Console.WriteLine("That's not a number. Try again.\n");
                return false;
            }

            _logger.LogDebug("User selected {Choice}", choice);

            if (choice < 1 || choice > exitOption)
            {
                _logger.LogWarning("Choice out of range: {Choice}", choice);
                Console.WriteLine("Invalid choice. Try again.\n");
                return false;
            }

            return true;
        }

        private static void HandleSpellCast(Character character, WorldState world, int spellIndex)
        {
            CastSpellResult result = character.CastSpell(character.AvailableSpells[spellIndex], world);
            Console.WriteLine(FormatCastResult(result));
            Console.WriteLine($"{character.Name}'s Mana: {character.Resources.Mana}, Dojo Durability: {world.Durability}");
            Console.WriteLine($"{TaigaCommentary.GetLine(world)}\n");
        }

        private static string FormatCastResult(CastSpellResult result)
        {
            if (!result.Success)
            {
                return $"{result.Caster.Name} tried to cast {result.Spell.Name} but couldn't afford the mana cost.";
            }

            string effectSummary = string.Join(" ",
                result.EffectResults
                .Select(effect => effect.Message)
                .Where(message => !string.IsNullOrWhiteSpace(message)));

            return string.IsNullOrWhiteSpace(effectSummary)
                ? $"{result.Caster.Name} cast {result.Spell.Name}!"
                : $"{result.Caster.Name} cast {result.Spell.Name}! {effectSummary}";
        }
    }
}
