using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain
{
    /// <summary>
    /// Per-character rules for how a spell's mana cost and eligibility are affected
    /// by the current <see cref="WorldState"/>. Implementations should only read
    /// from <see cref="WorldState"/> (passive rules) — they must not mutate it.
    /// Mutating the world is the job of <see cref="Effects.ISpellEffect"/>.
    /// </summary>
    public interface ICastingRules
    {
        /// <summary>
        /// Calculates the effective mana cost of casting the given spell given the
        /// current world state. Base implementations should return
        /// <see cref="Spell.ManaCost"/> unchanged unless a world condition applies.
        /// </summary>
        int GetManaCost(Spell spell, WorldState world);

        /// <summary>
        /// Determines whether the caster is allowed to cast the given spell given
        /// the current world state (e.g. a texture that suppresses certain magic).
        /// </summary>
        bool CanCast(Spell spell, WorldState world);

        /// <summary>
        /// Multiplier applied to a damage effect's raw damage before it is applied
        /// to <see cref="WorldState"/>, given the current world state (e.g. a texture
        /// that resists or empowers a particular caster's attacks).
        /// </summary>
        double GetDamageMultiplier(WorldState world);
    }
}
