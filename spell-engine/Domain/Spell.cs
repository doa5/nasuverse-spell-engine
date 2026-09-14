using NasuverseSpellEngine.Domain.Effects;

namespace NasuverseSpellEngine.Domain
{
    public class Spell
    {
        public string Name { get; }
        public int ManaCost { get; }
        public List<ISpellEffect> Effects { get; }

        public int Damage => Effects.OfType<DamageEffect>().Sum(effect => effect.Damage);

        public Spell(string name, int manaCost, params ISpellEffect[] effects)
        {
            Name = name;
            ManaCost = manaCost;
            Effects = new List<ISpellEffect>(effects);
        }

        public Spell(string name, int manaCost, DamageEffect damageEffect) // If there is only one effect, we can use this constructor for convenience
            : this(name, manaCost, (ISpellEffect)damageEffect)
        {
        }
    }
}
