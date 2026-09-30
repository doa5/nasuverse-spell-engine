using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain.World
{
    /// <summary>
    /// Pure lookup that maps the current <see cref="WorldState"/> to one of Taiga's
    /// reactive flavor-text lines. Reads state only — never mutates it.
    /// </summary>
    public static class TaigaCommentary
    {
        public static string GetLine(WorldState world)
        {
            if (world.Durability <= 0)
            {
                return "Taiga: THE DOJO! MY DOJO! You're paying for this, I swear!!";
            }

            if (world.ActiveTexture == RealityTexture.HeatDeathVoid && world.TextureChangedThisTurn)
            {
                return "Taiga: I-it's freezing... and everything feels so still. What did you DO?!";
            }

            if (world.ActiveTexture == RealityTexture.MillennialCastle && world.TextureChangedThisTurn)
            {
                return "Taiga: Wait, why do the walls look like they're a thousand years old?!";
            }

            if (world.AtmosphericMana)
            {
                return "Taiga: The air feels thick... like breathing static. Is that normal?!";
            }

            if (world.Durability <= 199)
            {
                return "Taiga: Okay, okay, that's enough! One more hit and this place is scrap!";
            }

            if (world.Durability <= 499)
            {
                return "Taiga: H-hey, careful! Dad's going to kill me if you wreck this place!";
            }

            if (world.Durability <= 799)
            {
                return "Taiga: Whoa, easy! That one left a mark!";
            }

            return "Taiga: Alright, looking good! Show me what you've got!";
        }
    }
}
