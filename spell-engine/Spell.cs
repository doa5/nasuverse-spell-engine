namespace NasuverseSpellEngine
{
    public class Spell
    {
        public string Name { get; set; }
        public int ManaCost { get; set; }
        public int Damage { get; set; }

        public Spell(string name, int manaCost, int damage)
        {
            Name = name;
            ManaCost = manaCost;
            Damage = damage;
        }
    }
}
