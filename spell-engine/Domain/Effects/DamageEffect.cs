namespace NasuverseSpellEngine.Domain.Effects
{
    public class DamageEffect : ISpellEffect
    {
        public int Damage { get; }

        public DamageEffect(int damage)
        {
            Damage = damage;
        }

        public SpellEffectResult Apply(Character caster, TaigaDojo target)
        {
            int appliedDamage = target.TakeDamage(Damage);
            return new SpellEffectResult($"Dealt {appliedDamage} damage.", appliedDamage);
        }
    }
}
