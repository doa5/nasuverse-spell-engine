using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Effects
{
    public interface ISpellEffect
    {
        SpellEffectResult Apply(Character caster, WorldState world);
    }
}
