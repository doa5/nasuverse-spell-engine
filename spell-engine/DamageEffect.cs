namespace NasuverseSpellEngine
{
    public class DamageEffect : ISpellEffect
    {
        public int Damage { get; }

        public DamageEffect(int damage)
        {
            Damage = damage;
        }

        public void Apply(Character caster, TaigaDojo target)
        {
            target.TargetHP -= Damage;
        }
    }
}
