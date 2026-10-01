using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain
{
    /// <summary>
    /// Shared, stateless helpers for <see cref="ICastingRules"/> implementations.
    /// Each character's rules class composes these by calling into them directly,
    /// rather than inheriting a shared base class - there is no "is-a" relationship
    /// between characters' casting rules, just reused calculations.
    /// </summary>
    public static class CastingRuleDefaults
    {
        /// <summary>Mana cost multiplier while the Heat Death Void is active - even the most efficient magecraft struggles in a dead world.</summary>
        private const double HeatDeathVoidManaMultiplier = 2.0;

        /// <summary>Damage multiplier applied to anyone but the Castle's manifester while the Millennial Castle is active.</summary>
        private const double MillennialCastleDamageMultiplier = 0.5;

        /// <summary>Default damage multiplier when no world condition applies.</summary>
        private const double NormalDamageMultiplier = 1.0;

        /// <summary>
        /// Doubles <paramref name="cost"/> while the Heat Death Void is active. Every
        /// <see cref="ICastingRules.GetManaCost"/> implementation should route its
        /// final result through this so the tax always applies, regardless of
        /// whatever character-specific discount/surcharge logic ran first.
        /// </summary>
        public static int ApplyHeatDeathTax(int cost, WorldState world) =>
            world.ActiveTexture == RealityTexture.HeatDeathVoid ? (int)(cost * HeatDeathVoidManaMultiplier) : cost;

        /// <summary>
        /// The Millennial Castle overwrites the room to resist outside magic, halving
        /// damage for everyone except the one who manifested it (see <c>ArcueidCastingRules</c>).
        /// </summary>
        public static double GetDamageMultiplier(WorldState world) =>
            world.ActiveTexture == RealityTexture.MillennialCastle ? MillennialCastleDamageMultiplier : NormalDamageMultiplier;
    }
}
