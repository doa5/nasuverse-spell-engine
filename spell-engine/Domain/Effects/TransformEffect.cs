using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Effects
{
    /// <summary>
    /// Activates the caster's <see cref="ITransformState"/>, swapping in their
    /// alternate spell set (e.g. Aoko's Redshift -> Adult Aoko). If the caster is
    /// already transformed, this simply refreshes/no-ops rather than stacking.
    /// </summary>
    public class TransformEffect : ISpellEffect
    {
        private readonly Func<ITransformState> _createState;

        public TransformEffect(Func<ITransformState> createState)
        {
            _createState = createState;
        }

        public SpellEffectResult Apply(Character caster, WorldState world)
        {
            if (caster.Transform is { IsActive: true })
            {
                return new SpellEffectResult($"{caster.Name} is already transformed.");
            }

            caster.Transform = _createState();
            caster.SwapSpellSet();

            world.PushEvent($"World interaction: {caster.Name}'s transformation reshapes their power.");
            return new SpellEffectResult($"{caster.Name} transforms.");
        }
    }
}
