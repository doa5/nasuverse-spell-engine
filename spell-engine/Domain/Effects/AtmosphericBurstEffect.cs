using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Effects
{
    /// <summary>Saturates the atmosphere with dense mana, altering casting conditions for a time.</summary>
    public class AtmosphericBurstEffect : ISpellEffect
    {
        public SpellEffectResult Apply(Character caster, WorldState world)
        {
            world.TriggerManaBurst();
            return new SpellEffectResult("The atmosphere saturates with dense mana.");
        }
    }
}
