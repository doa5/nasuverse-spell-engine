using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain
{
    /// <summary>
    /// Arcueid's Marble Phantasm draws directly on the Earth's backing, so her own
    /// attacks are empowered rather than resisted while the Millennial Castle is
    /// active, unlike everyone else's magic.
    /// </summary>
    public class ArcueidCastingRules : DefaultCastingRules
    {
        public override double GetDamageMultiplier(WorldState world) =>
            world.ActiveTexture == RealityTexture.MillennialCastle ? 2.0 : 1.0;

        public override int GetManaCost(Spell spell, WorldState world)
        {
            int baseCost = spell.ManaCost;

            if (IsMillennialCastle(spell))
            {
                // A decaying, entropy-ridden world weakens Arcueid's connection to the Earth,
                // making the Castle far more expensive to manifest.
                return ApplyHeatDeathTax(world.Entropy >= 100 ? baseCost * 2 : baseCost, world);
            }

            if (world.AtmosphericMana)
            {
                // Saber's high-density dragon mana creates noise in the natural order,
                // making Arcueid's environmental manipulation harder until it's purged.
                return ApplyHeatDeathTax((int)(baseCost * 1.5), world);
            }

            return ApplyHeatDeathTax(baseCost, world);
        }

        private static bool IsMillennialCastle(Spell spell) => spell.Name.Contains("Marble Phantasm");
    }
}
