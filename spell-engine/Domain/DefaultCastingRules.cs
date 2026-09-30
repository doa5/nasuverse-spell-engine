using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain
{
    /// <summary>
    /// Default <see cref="ICastingRules"/> used until a character has its own
    /// world-state interaction rules defined. Applies no cost changes and never
    /// restricts casting.
    /// </summary>
    public class DefaultCastingRules : ICastingRules
    {
        /// <summary>Mana cost multiplier while the Heat Death Void is active - even the most efficient magecraft struggles in a dead world.</summary>
        protected const double HeatDeathVoidManaMultiplier = 2.0;

        public virtual int GetManaCost(Spell spell, WorldState world) => ApplyHeatDeathTax(spell.ManaCost, world);

        public virtual bool CanCast(Spell spell, WorldState world) => true;

        /// <summary>
        /// The Millennial Castle overwrites the room to resist outside magic, halving
        /// damage for everyone except the one who manifested it (see <c>ArcueidCastingRules</c>).
        /// </summary>
        public virtual double GetDamageMultiplier(WorldState world) =>
            world.ActiveTexture == RealityTexture.MillennialCastle ? 0.5 : 1.0;

        /// <summary>
        /// Doubles <paramref name="cost"/> while the Heat Death Void is active. Every
        /// character's <see cref="GetManaCost"/> override should route its final
        /// result through this so the tax always applies, regardless of whatever
        /// character-specific discount/surcharge logic ran first.
        /// </summary>
        protected static int ApplyHeatDeathTax(int cost, WorldState world) =>
            world.ActiveTexture == RealityTexture.HeatDeathVoid ? (int)(cost * HeatDeathVoidManaMultiplier) : cost;
    }
}
