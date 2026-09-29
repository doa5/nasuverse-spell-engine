using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

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
            // Console.Clear() throws IOException when output isn't attached to a real
            // console (e.g. piped/redirected input, some test runners). Skip it in that case.
            if (!Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            WriteWorldStatus(view.World);
            WriteCharacterStatus(view.ActiveCharacter);
            WriteLastAction(view.LastAction);
            WriteEventLog(view.World);
            int exitOption = WriteSpellMenu(view.ActiveCharacter);
            WriteTaigaLine(view.World);

            return exitOption;
        }

        private static void WriteWorldStatus(WorldState world)
        {
            Console.WriteLine("=== Taiga Dojo ===");
            Console.WriteLine($"Turn: {world.TurnCount}  Durability: {world.Durability}  Entropy: {world.Entropy}");
            Console.WriteLine($"Texture: {world.ActiveTexture}  Atmospheric Mana: {world.AtmosphericMana}");
            Console.WriteLine();
        }

        private static void WriteCharacterStatus(Character character)
        {
            Console.WriteLine($"-- {character.Name} --");
            Console.WriteLine($"Mana: {character.Resources.Mana}");
            Console.WriteLine();
        }

        private static void WriteLastAction(string? lastAction)
        {
            if (lastAction is not null)
            {
                Console.WriteLine(lastAction);
                Console.WriteLine();
            }
        }

        private static int WriteSpellMenu(Character character)
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

        private static void WriteEventLog(WorldState world)
        {
            Console.WriteLine("-- Recent Events --");
            foreach (string eventMessage in world.RecentEvents)
            {
                Console.WriteLine($"  {eventMessage}");
            }

            Console.WriteLine();
        }

        private static void WriteTaigaLine(WorldState world)
        {
            Console.WriteLine(TaigaCommentary.GetLine(world));
            Console.WriteLine();
        }
    }
}
