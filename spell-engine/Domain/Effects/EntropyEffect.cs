using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Effects
{
    /// <summary>Adds entropy to the world, pushing it toward a permanent Heat Death Void.</summary>
    public class EntropyEffect : ISpellEffect
    {
        public int Amount { get; }

        public EntropyEffect(int amount)
        {
            Amount = amount;
        }

        public SpellEffectResult Apply(Character caster, WorldState world)
        {
            world.AddEntropy(Amount);
            return new SpellEffectResult($"Entropy increased by {Amount} (now {world.Entropy}%).");
        }
    }
}
