using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Characters
{
    /// <summary>
    /// Aoko's Fifth Magic (Redshift) runs on borrowed time/entropy and costs no mana
    /// at all, so only her standard magecraft reacts to the room's rules the way any
    /// human magecraft would: Saber's atmospheric mana feeds it for free, while
    /// Arcueid's Millennial Castle suppresses it and taxes it for double mana.
    /// </summary>
    public class AokoCastingRules : ICastingRules
    {
        /// <summary>Mana cost multiplier for standard magecraft while the Millennial Castle suppresses it.</summary>
        private const int MillennialCastleManaCostMultiplier = 2;

        public bool CanCast(Spell spell, WorldState world) => true;

        public double GetDamageMultiplier(WorldState world) => CastingRuleDefaults.GetDamageMultiplier(world);

        public int GetManaCost(Spell spell, WorldState world)
        {
            if (world.AtmosphericMana)
            {
                // Saber's dragon-mana saturation lets Aoko draw free energy from the air.
                // Even a dead world can't tax energy that costs nothing to begin with.
                return 0;
            }

            if (world.ActiveTexture == RealityTexture.MillennialCastle)
            {
                // The Castle overwrites the room's rules, suppressing standard human magecraft.
                return CastingRuleDefaults.ApplyHeatDeathTax(spell.ManaCost * MillennialCastleManaCostMultiplier, world);
            }

            return CastingRuleDefaults.ApplyHeatDeathTax(spell.ManaCost, world);
        }
    }
}
