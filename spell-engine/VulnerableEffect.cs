namespace NasuverseSpellEngine
{
    public class VulnerableEffect : ISpellEffect
    {
        public SpellEffectResult Apply(Character caster, TaigaDojo target)
        {
            target.ApplyVulnerability();
            return new SpellEffectResult("The dojo is now vulnerable. The next attack will deal 2x the damage.", 0, "Vulnerable");
        }
    }
}
