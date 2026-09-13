namespace NasuverseSpellEngine
{
    public class VulnerableEffect : ISpellEffect
    {
        public void Apply(Character caster, TaigaDojo target)
        {
            target.ApplyVulnerability();
        }
    }
}
