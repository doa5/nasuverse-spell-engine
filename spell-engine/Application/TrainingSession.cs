using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;
using Spectre.Console;

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
            DashboardRenderer.PrepareCharacterSelect();

            var prompt = new SelectionPrompt<Character>()
                .Title("Select a character:")
                .HighlightStyle(new Style(decoration: Decoration.Underline))
                .UseConverter(character => character.Name)
                .AddChoices(roster);

            Character chosen = AnsiConsole.Prompt(prompt);
            _logger.LogDebug("User selected character {Character}", chosen.Name);
            return chosen;
        }

        private const string SwitchCharacterChoice = "Switch Character";

        public void Run(IReadOnlyList<Character> roster, WorldState world)
        {
            Character activeCharacter = SelectCharacter(roster);
            string? lastAction = null;

            while (true)
            {
                DashboardRenderer.Render(new DashboardView(activeCharacter, world, lastAction));

                string choice = ReadSpellChoice(activeCharacter);

                if (choice == SwitchCharacterChoice)
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

                int spellIndex = activeCharacter.AvailableSpells.FindIndex(spell => spell.Name == choice);
                if (spellIndex < 0)
                {
                    _logger.LogWarning("Selected spell not found in available spells: {Choice}", choice);
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

        private string ReadSpellChoice(Character character)
        {
            var prompt = new SelectionPrompt<string>()
                .Title("Choose a spell:")
                .HighlightStyle(new Style(decoration: Decoration.Underline))
                .AddChoices(character.AvailableSpells.Select(spell => spell.Name))
                .AddChoices(SwitchCharacterChoice);

            string choice = AnsiConsole.Prompt(prompt);
            _logger.LogDebug("User selected {Choice}", choice);
            return choice;
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
