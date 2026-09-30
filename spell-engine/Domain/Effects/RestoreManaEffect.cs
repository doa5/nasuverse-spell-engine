using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Effects
{
    /// <summary>
    /// Restores the caster's mana to full at the cost of world entropy (e.g.
    /// Aoko's "I need more mana!").
    /// </summary>
    public class RestoreManaEffect : ISpellEffect
    {
        public int EntropyCost { get; }

        public RestoreManaEffect(int entropyCost)
        {
            EntropyCost = entropyCost;
        }

        public SpellEffectResult Apply(Character caster, WorldState world)
        {
            caster.Resources.RestoreToFull();
            world.AddEntropy(EntropyCost);

            world.PushEvent($"World interaction: {caster.Name} trades {EntropyCost}% entropy to refill their mana.");
            return new SpellEffectResult($"{caster.Name}'s mana is restored (+{EntropyCost}% entropy).");
        }
    }
}
