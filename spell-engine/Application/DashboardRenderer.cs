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
        /// <summary>
        /// Draws a celebratory "You Win!" panel when the dojo's durability reaches 0.
        /// Purely additive to the current screen - callers still show the final
        /// dashboard frame before invoking this.
        /// </summary>
        public static void RenderVictory(Character character)
        {
            var panel = new Panel(new Markup($"[bold]{Markup.Escape(character.Name)}[/] destroyed the Taiga Dojo!\nYou win!"))
            {
                Header = new PanelHeader("Victory!"),
                Border = BoxBorder.Double,
                Padding = new Padding(2, 1),
            };
            panel.BorderStyle = new Style(foreground: Style.Parse(character.AccentColor).Foreground);

            AnsiConsole.WriteLine();
            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();
        }

        public static void Render(DashboardView view)
        {
            ClearConsole();

            string accent = view.ActiveCharacter.AccentColor;
            AnsiConsole.Write(new Rule($"[{accent}]Taiga Dojo[/]").RuleStyle(accent));

            WriteWorldStatus(view.World, accent);
            WriteCharacterStatus(view.ActiveCharacter);
            WriteLastAction(view.LastAction);
            WriteEventLog(view.World);
            WriteSpellMenu(view.ActiveCharacter, view.World);
            WriteTaigaLine(view.World, accent);
        }

        /// <summary>
        /// Clears the screen ahead of the character selection prompt — used both
        /// for the initial character pick and for the in-session "Switch Character"
        /// option, so it fully replaces the dashboard rather than appending below it.
        /// </summary>
        public static void PrepareCharacterSelect()
        {
            ClearConsole();
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

        private static void WriteSpellMenu(Character character, WorldState world)
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
            AnsiConsole.WriteLine();
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
