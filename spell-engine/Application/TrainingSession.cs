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

        public Character SelectCharacter(IReadOnlyList<Character> roster)
        {
            DashboardRenderer.RenderCharacterSelect(roster);

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

                int index = characterChoice - 1;
                if (index >= 0 && index < roster.Count)
                {
                    return roster[index];
                }

                _logger.LogWarning("Choice out of range: {Choice}", characterChoice);
                Console.WriteLine($"Please choose a number between 1 and {roster.Count}.\n");
            }
        }

        public void Run(IReadOnlyList<Character> roster, WorldState world)
        {
            Character activeCharacter = SelectCharacter(roster);
            string? lastAction = null;
            bool isTraining = true;

            while (isTraining)
            {
                (int switchOption, int exitOption) = DashboardRenderer.Render(new DashboardView(activeCharacter, world, lastAction));

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

                if (choice == switchOption)
                {
                    _logger.LogInformation("User selected Switch Character");
                    activeCharacter = SelectCharacter(roster);
                    lastAction = $"Switched to {activeCharacter.Name}.";
                    continue;
                }

                if (world.Durability <= 0)
                {
                    _logger.LogInformation("Attempt to cast on destroyed dojo (Durability {Durability})", world.Durability);
                    lastAction = "Taiga: The dojo has already been destroyed! What are you doing??";
                    continue;
                }

                // Dispatch by index (scales with number of spells)
                int spellIndex = choice - 1;
                if (spellIndex < 0 || spellIndex >= activeCharacter.AvailableSpells.Count)
                {
                    _logger.LogWarning("Computed spell index out of range: {Index}", spellIndex);
                    lastAction = "Invalid spell selection. Try again.";
                    continue;
                }

                lastAction = HandleSpellCast(activeCharacter, world, spellIndex);
                world.AdvanceTurn();
                activeCharacter.TickTransform(world);

                if (world.Durability <= 0)
                {
                    lastAction += " The Taiga Dojo has been destroyed!";
                }
            }
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

        private static string HandleSpellCast(Character character, WorldState world, int spellIndex)
        {
            CastSpellResult result = character.CastSpell(character.AvailableSpells[spellIndex], world);
            return FormatCastResult(result);
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
