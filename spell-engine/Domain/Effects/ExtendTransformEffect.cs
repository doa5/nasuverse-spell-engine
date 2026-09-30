using NasuverseSpellEngine.Domain.Characters;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Effects
{
    /// <summary>
    /// Extends the caster's active transform (e.g. Aoko's "I need more time!"),
    /// costing escalating world entropy each time it's used during the same
    /// transformation. No-op (refunds nothing) if the caster isn't transformed.
    /// </summary>
    public class ExtendTransformEffect : ISpellEffect
    {
        public SpellEffectResult Apply(Character caster, WorldState world)
        {
            if (caster.Transform is not AokoTransformState { IsActive: true } transform)
            {
                return new SpellEffectResult($"{caster.Name} isn't transformed - there's no time to borrow.");
            }

            int entropyCost = transform.GetNextExtensionCost();
            transform.Extend();
            world.AddEntropy(entropyCost);

            world.PushEvent($"World interaction: {caster.Name} borrows more time, adding {entropyCost}% entropy.");
            return new SpellEffectResult($"{caster.Name} extends their transformation by {AokoTransformState.ExtensionTurns} turns (+{entropyCost}% entropy).");
        }
    }
}
