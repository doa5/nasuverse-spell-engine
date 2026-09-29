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
        public int GetManaCost(Spell spell, WorldState world) => spell.ManaCost;

        public bool CanCast(Spell spell, WorldState world) => true;
    }
}
