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
        public virtual int GetManaCost(Spell spell, WorldState world) => spell.ManaCost;

        public virtual bool CanCast(Spell spell, WorldState world) => true;

        /// <summary>
        /// The Millennial Castle overwrites the room to resist outside magic, halving
        /// damage for everyone except the one who manifested it (see <c>ArcueidCastingRules</c>).
        /// </summary>
        public virtual double GetDamageMultiplier(WorldState world) =>
            world.ActiveTexture == RealityTexture.MillennialCastle ? 0.5 : 1.0;
    }
}
