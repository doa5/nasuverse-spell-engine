using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain
{
    /// <summary>
    /// Plain <see cref="ICastingRules"/> implementation used until a character has
    /// its own world-state interaction rules defined. Applies no cost changes beyond
    /// the shared <see cref="CastingRuleDefaults"/> and never restricts casting.
    /// Sealed - this is just one implementation of <see cref="ICastingRules"/> among
    /// others (e.g. <c>AokoCastingRules</c>), not a base class to inherit from.
    /// </summary>
    public sealed class StandardCastingRules : ICastingRules
    {
        public int GetManaCost(Spell spell, WorldState world) =>
            CastingRuleDefaults.ApplyHeatDeathTax(spell.ManaCost, world);

        public bool CanCast(Spell spell, WorldState world) => true;

        public double GetDamageMultiplier(WorldState world) =>
            CastingRuleDefaults.GetDamageMultiplier(world);
    }
}
