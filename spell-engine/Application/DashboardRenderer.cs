using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;
using Spectre.Console;

namespace NasuverseSpellEngine.Application
{
    /// <summary>
    /// Pure console renderer for a <see cref="DashboardView"/>. Only ever reads the
    /// view and writes to the console — never reads input, never mutates
    /// <see cref="WorldState"/> or <see cref="Character"/>.
    /// </summary>
    public static class DashboardRenderer
    {
        public static int Render(DashboardView view)
        {
            ClearConsole();

            string accent = view.ActiveCharacter.AccentColor;
            AnsiConsole.Write(new Rule($"[{accent}]Taiga Dojo[/]").RuleStyle(accent));

            WriteWorldStatus(view.World, accent);
            WriteCharacterStatus(view.ActiveCharacter);
            WriteLastAction(view.LastAction);
            WriteEventLog(view.World);
            int switchOption = WriteSpellMenu(view.ActiveCharacter, view.World);
            WriteTaigaLine(view.World, accent);

            return switchOption;
        }

        /// <summary>
        /// Clears the screen and draws only the character selection list — used both
        /// for the initial character pick and for the in-session "Switch Character"
        /// option, so it fully replaces the dashboard rather than appending below it.
        /// </summary>
        public static void RenderCharacterSelect(IReadOnlyList<Character> roster)
        {
            ClearConsole();

            AnsiConsole.MarkupLine("Select a character:");
            for (int i = 0; i < roster.Count; i++)
            {
                Character character = roster[i];
                AnsiConsole.MarkupLine($"{i + 1}. [{character.AccentColor}]{character.Name}[/]");
            }

            AnsiConsole.WriteLine();
        }

        private static void ClearConsole()
        {
            // AnsiConsole.Clear() still relies on the console output; guard the same
            // way as before when output isn't attached to a real console (e.g.
            // piped/redirected input, some test runners).
            if (!Console.IsOutputRedirected)
            {
                AnsiConsole.Clear();
            }
        }

        private static void WriteWorldStatus(WorldState world, string accent)
        {
            var content = new Markup(
                $"Turn: {world.TurnCount}    Durability: {world.Durability}\n" +
                $"Entropy: {world.Entropy}%    Texture: {world.ActiveTexture}\n" +
                $"Atmospheric Mana: {world.AtmosphericMana}");

            var panel = new Panel(content)
            {
                Header = new PanelHeader("World Status"),
                Border = BoxBorder.Rounded,
            };
            panel.BorderStyle = new Style(foreground: Style.Parse(accent).Foreground);

            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();
        }

        private static void WriteCharacterStatus(Character character)
        {
            AnsiConsole.MarkupLine($"[{character.AccentColor}]-- {character.Name} --[/]");
            AnsiConsole.MarkupLine($"Mana: {character.Resources.Mana}");
            AnsiConsole.WriteLine();
        }

        private static void WriteLastAction(string? lastAction)
        {
            if (lastAction is not null)
            {
                AnsiConsole.MarkupLine(Markup.Escape(lastAction));
                AnsiConsole.WriteLine();
            }
        }

        private static int WriteSpellMenu(Character character, WorldState world)
        {
            AnsiConsole.MarkupLine($"[{character.AccentColor}]-- {character.Name}'s Spells --[/]");

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("#");
            table.AddColumn("Name");
            table.AddColumn("Cost");
            table.AddColumn("Damage");
            table.AddColumn("Description");

            for (int i = 0; i < character.AvailableSpells.Count; i++)
            {
                Spell spell = character.AvailableSpells[i];
                int effectiveCost = character.CastingRules.GetManaCost(spell, world);
                bool canAfford = character.Resources.Mana >= effectiveCost;
                string costDisplay = effectiveCost != spell.ManaCost
                    ? $"{effectiveCost} (base {spell.ManaCost})"
                    : $"{effectiveCost}";
                string costMarkup = canAfford ? costDisplay : $"[red]{costDisplay}[/]";

                table.AddRow(
                    (i + 1).ToString(),
                    Markup.Escape(spell.Name),
                    costMarkup,
                    spell.Damage.ToString(),
                    Markup.Escape(spell.Description ?? string.Empty));
            }

            AnsiConsole.Write(table);

            int switchOption = character.AvailableSpells.Count + 1;
            AnsiConsole.MarkupLine($"{switchOption}. Switch Character");
            AnsiConsole.WriteLine();
            return switchOption;
        }

        private static void WriteEventLog(WorldState world)
        {
            AnsiConsole.Write(new Rule("Recent Events").LeftJustified());
            foreach (string eventMessage in world.RecentEvents)
            {
                AnsiConsole.MarkupLine($"  {Markup.Escape(eventMessage)}");
            }

            AnsiConsole.WriteLine();
        }

        private static void WriteTaigaLine(WorldState world, string accent)
        {
            var panel = new Panel(Markup.Escape(TaigaCommentary.GetLine(world)))
            {
                Header = new PanelHeader("Taiga"),
                Border = BoxBorder.Rounded,
            };
            panel.BorderStyle = new Style(foreground: Style.Parse(accent).Foreground);

            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();
        }
    }
}
