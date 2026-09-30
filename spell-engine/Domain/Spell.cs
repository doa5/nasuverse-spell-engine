using NasuverseSpellEngine.Domain.Effects;

namespace NasuverseSpellEngine.Domain
{
    public class Spell
    {
        public string Name { get; }
        public int ManaCost { get; }
        public List<ISpellEffect> Effects { get; }

        /// <summary>Short flavor text describing what this spell does, shown in the spell menu.</summary>
        public string? Description { get; }

        public int Damage => Effects.OfType<DamageEffect>().Sum(effect => effect.Damage);

        public Spell(string name, int manaCost, ISpellEffect[] effects, string? description = null)
        {
            Name = name;
            ManaCost = manaCost;
            Effects = new List<ISpellEffect>(effects);
            Description = description;
        }

        public Spell(string name, int manaCost, params ISpellEffect[] effects)
            : this(name, manaCost, effects, description: null)
        {
        }

        public Spell(string name, int manaCost, DamageEffect damageEffect) // If there is only one effect, we can use this constructor for convenience
            : this(name, manaCost, (ISpellEffect)damageEffect)
        {
        }
    }
}
