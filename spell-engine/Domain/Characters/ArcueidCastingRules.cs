using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Characters
{
    /// <summary>
    /// Arcueid's Marble Phantasm draws directly on the Earth's backing, so her own
    /// attacks are empowered rather than resisted while the Millennial Castle is
    /// active, unlike everyone else's magic.
    /// </summary>
    public class ArcueidCastingRules : ICastingRules
    {
        /// <summary>Damage multiplier for Arcueid's own attacks while the Millennial Castle she manifested is active.</summary>
        private const double MillennialCastleOwnDamageMultiplier = 2.0;

        /// <summary>Default damage multiplier when no world condition applies.</summary>
        private const double NormalDamageMultiplier = 1.0;

        /// <summary>Mana cost multiplier for manifesting the Millennial Castle once entropy reaches 100%.</summary>
        private const int HeatDeathCastleManaCostMultiplier = 2;

        /// <summary>Mana cost multiplier applied while Saber's atmospheric mana is interfering with Arcueid's environmental manipulation.</summary>
        private const double AtmosphericInterferenceManaCostMultiplier = 1.5;

        public bool CanCast(Spell spell, WorldState world) => true;

        public double GetDamageMultiplier(WorldState world) =>
            world.ActiveTexture == RealityTexture.MillennialCastle ? MillennialCastleOwnDamageMultiplier : NormalDamageMultiplier;

        public int GetManaCost(Spell spell, WorldState world)
        {
            int baseCost = spell.ManaCost;

            if (IsMillennialCastle(spell))
            {
                // A decaying, entropy-ridden world weakens Arcueid's connection to the Earth,
                // making the Castle far more expensive to manifest.
                return CastingRuleDefaults.ApplyHeatDeathTax(
                    world.Entropy >= WorldState.MaxEntropy ? baseCost * HeatDeathCastleManaCostMultiplier : baseCost,
                    world);
            }

            if (world.AtmosphericMana)
            {
                // Saber's high-density dragon mana creates noise in the natural order,
                // making Arcueid's environmental manipulation harder until it's purged.
                return CastingRuleDefaults.ApplyHeatDeathTax((int)(Math.Ceiling(baseCost * AtmosphericInterferenceManaCostMultiplier)), world);
            }

            return CastingRuleDefaults.ApplyHeatDeathTax(baseCost, world);
        }

        private static bool IsMillennialCastle(Spell spell) => spell.Name.Contains("Marble Phantasm");
    }
}
