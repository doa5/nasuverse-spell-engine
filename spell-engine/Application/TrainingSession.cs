using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain;

namespace NasuverseSpellEngine.Application
{
    public class TrainingSession
    {
        private readonly ILogger _logger;

        public TrainingSession(ILogger logger)
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

        public void Run(Character selectedCharacter, TaigaDojo dojo)
        {
            Console.WriteLine($"Welcome to the Taiga Dojo with {selectedCharacter.Name}!");
            Console.WriteLine($"Initial Mana: {selectedCharacter.Resources.Mana}, Dojo HP: {dojo.TargetHP}\n");

            bool isTraining = true;
            while (isTraining)
            {
                Console.WriteLine($"--- {selectedCharacter.Name}'s Spells ---");
                for (int i = 0; i < selectedCharacter.AvailableSpells.Count; i++)
                {
                    Spell spell = selectedCharacter.AvailableSpells[i];
                    Console.WriteLine($"{i + 1}. {spell.Name} (Cost: {spell.ManaCost}, Damage: {spell.Damage})");
                }
                int exitOption = selectedCharacter.AvailableSpells.Count + 1;
                Console.WriteLine($"{exitOption}. Exit\n");

                Console.Write("Choose a spell: ");
                string input = Console.ReadLine()!; // ! means "trust me, it's not null"
                Console.Write("\n");

                if (!int.TryParse(input, out int choice))
                {
                    _logger.LogWarning("Invalid input (non-numeric): {Input}", input);
                    Console.WriteLine("That's not a number. Try again.\n");
                    continue;
                }

                _logger.LogDebug("User selected {Choice}", choice);

                if (choice < 1 || choice > exitOption)
                {
                    _logger.LogWarning("Choice out of range: {Choice}", choice);
                    Console.WriteLine("Invalid choice. Try again.\n");
                    continue;
                }

                // Allow exit even when dojo destroyed
                if (choice == exitOption)
                {
                    _logger.LogInformation("User selected Exit");
                    isTraining = false;
                    continue;
                }

                if (dojo.TargetHP <= 0)
                {
                    _logger.LogInformation("Attempt to cast on destroyed dojo (HP {HP})", dojo.TargetHP);
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

                HandleSpellCast(selectedCharacter, dojo, spellIndex);

                if (dojo.TargetHP <= 0)
                {
                    Console.WriteLine("The Taiga Dojo has been destroyed!\n");
                }
            }
        }

        private static void HandleSpellCast(Character character, TaigaDojo dojo, int spellIndex)
        {
            string result = character.CastSpell(character.AvailableSpells[spellIndex], dojo);
            Console.WriteLine(result);
            Console.WriteLine($"{character.Name}'s Mana: {character.Resources.Mana}, Dojo HP: {dojo.TargetHP}\n");
        }
    }
}
