using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Effects
{
    /// <summary>Attempts to manifest the Millennial Castle reality texture.</summary>
    public class TextureEffect : ISpellEffect
    {
        public SpellEffectResult Apply(Character caster, WorldState world)
        {
            bool activated = world.TryActivateMillennialCastle();
            return activated
                ? new SpellEffectResult("The Millennial Castle Brunestud manifests, overwriting reality.")
                : new SpellEffectResult("The texture failed to manifest.");
        }
    }
}
