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
            double multiplier = caster.CastingRules.GetDamageMultiplier(world);

            if (multiplier != 1.0)
            {
                world.PushEvent(multiplier > 1.0
                    ? $"World interaction: {caster.Name}'s attack is empowered ({multiplier:0.##}x damage)."
                    : $"World interaction: {caster.Name}'s attack is resisted ({multiplier:0.##}x damage).");
            }

            int appliedDamage = world.ApplyDamage(Damage, multiplier);
            return new SpellEffectResult($"Dealt {appliedDamage} damage.", appliedDamage);
        }
    }
}
