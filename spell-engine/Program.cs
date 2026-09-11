namespace NasuverseSpellEngine
{
    class Program 
    { 
        static Character CreateAoko()
        {
            Character aoko = new Character("Aoko", new ResourcePool(initialMana: 100));

            Spell snapAndDraw = new Spell("Snap & Draw", manaCost: 20, damage: 5);
            Spell earthlightStarbow = new Spell("Earthlight Starbow", manaCost: 50, damage: 35);

            aoko.AvailableSpells.Add(earthlightStarbow);
            aoko.AvailableSpells.Add(snapAndDraw);

            return aoko;
        }
        static void Main(string[] args)
        {
            TaigaDojo dojo = new TaigaDojo(100);
            Character aoko = CreateAoko();

            Console.WriteLine($"Welcome to the Taiga Dojo with {aoko.Name}!");
            Console.WriteLine($"Initial Mana: {aoko.Resources.Mana}, Dojo HP: {dojo.TargetHP}\n");
        }

    }
}
