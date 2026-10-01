using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Characters
{
    /// <summary>
    /// Saber's Dragon Core produces mana internally, so unlike Aoko she is never
    /// starved by the Millennial Castle suppressing "standard human magecraft" -
    /// her interactions with the room are mostly handled by <see cref="WorldState"/>
    /// itself (the Castle purges her atmospheric mana the instant it manifests, and
    /// shortens a burst cast during an active Castle to a single turn). Once her own
    /// atmospheric mana is saturating the room, she draws on it directly and pays
    /// nothing to cast.
    /// </summary>
    public class SaberCastingRules : ICastingRules
    {
        public bool CanCast(Spell spell, WorldState world) => true;

        public double GetDamageMultiplier(WorldState world) => CastingRuleDefaults.GetDamageMultiplier(world);

        public int GetManaCost(Spell spell, WorldState world) =>
            world.AtmosphericMana ? 0 : CastingRuleDefaults.ApplyHeatDeathTax(spell.ManaCost, world);
    }
}
