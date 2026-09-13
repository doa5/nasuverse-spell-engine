namespace NasuverseSpellEngine
{
    public interface ISpellEffect
    {
        SpellEffectResult Apply(Character caster, TaigaDojo target);
    }
}
