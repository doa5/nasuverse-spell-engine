using NasuverseSpellEngine.Domain;

namespace NasuverseSpellEngine.Domain.Effects
{
    public interface ISpellEffect
    {
        SpellEffectResult Apply(Character caster, TaigaDojo target);
    }
}
