namespace NasuverseSpellEngine
{
    public class Spell
    {
        public string Name { get; }
        public int ManaCost { get; }
        public List<ISpellEffect> Effects { get; }

        public int Damage => Effects.OfType<DamageEffect>().Sum(effect => effect.Damage);

        public Spell(string name, int manaCost, DamageEffect damageEffect)
        {
            Name = name;
            ManaCost = manaCost;
            Effects = new List<ISpellEffect> { damageEffect };
        }
    }
}
