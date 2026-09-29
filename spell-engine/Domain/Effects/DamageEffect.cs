using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Effects
{
    public class DamageEffect : ISpellEffect
    {
        public int Damage { get; }

        public DamageEffect(int damage)
        {
            Damage = damage;
        }

        public SpellEffectResult Apply(Character caster, WorldState world)
        {
            int appliedDamage = world.ApplyDamage(Damage);
            return new SpellEffectResult($"Dealt {appliedDamage} damage.", appliedDamage);
        }
    }
}
