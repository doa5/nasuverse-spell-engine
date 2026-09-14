using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.Effects;

namespace NasuverseSpellEngine.Application
{
    public static class CharacterFactory
    {
        public static Character CreateAoko(ILogger<Character> logger, ILogger<ResourcePool> resourceLogger)
        {
            var pool = new ResourcePool(200, resourceLogger);
            Character aoko = new Character("Aoko", pool, logger);

            Spell snapAndDraw = new Spell("Snap & Draw", manaCost: 20, new DamageEffect(5));
            Spell earthlightStarbow = new Spell("Earthlight Starbow", manaCost: 50, new DamageEffect(35));

            aoko.AvailableSpells.Add(earthlightStarbow);
            aoko.AvailableSpells.Add(snapAndDraw);

            return aoko;
        }

        public static Character CreateArcueid(ILogger<Character> logger, ILogger<ResourcePool> resourceLogger)
        {
            var pool = new ResourcePool(500, resourceLogger);
            Character arcueid = new Character("Arcueid", pool, logger);

            Spell mysticEyes = new Spell("Mystic Eyes of Enchantment", manaCost: 15, new VulnerableEffect());
            Spell meltyBlood = new Spell("Melty Blood", manaCost: 20, new DamageEffect(25));

            arcueid.AvailableSpells.Add(meltyBlood);
            arcueid.AvailableSpells.Add(mysticEyes);

            return arcueid;
        }
    }
}
