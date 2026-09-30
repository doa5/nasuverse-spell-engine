using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.Characters
{
    /// <summary>
    /// Aoko's Fifth Magic runs on borrowed time/entropy rather than mana efficiency,
    /// so her standard magecraft reacts to the room's rules the way any human
    /// magecraft would: Saber's atmospheric mana feeds it for free, while Arcueid's
    /// Millennial Castle suppresses it and taxes it for double mana.
    /// </summary>
    public class AokoCastingRules : DefaultCastingRules
    {
        public override int GetManaCost(Spell spell, WorldState world)
        {
            if (IsFifthMagic(spell))
            {
                return ApplyHeatDeathTax(spell.ManaCost, world);
            }

            if (world.AtmosphericMana)
            {
                // Saber's dragon-mana saturation lets Aoko draw free energy from the air.
                // Even a dead world can't tax energy that costs nothing to begin with.
                return 0;
            }

            if (world.ActiveTexture == RealityTexture.MillennialCastle)
            {
                // The Castle overwrites the room's rules, suppressing standard human magecraft.
                return ApplyHeatDeathTax(spell.ManaCost * 2, world);
            }

            return ApplyHeatDeathTax(spell.ManaCost, world);
        }

        private static bool IsFifthMagic(Spell spell) => spell.Name.Contains("Fifth Magic");
    }
}
