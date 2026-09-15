using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.Effects;

namespace NasuverseSpellEngine.Application
{
    public static class CharacterFactory
    {
        private const int AokoStartingMana = 200;
        private const int SnapAndDrawManaCost = 20;
        private const int SnapAndDrawDamage = 5;
        private const int EarthlightStarbowManaCost = 50;
        private const int EarthlightStarbowDamage = 35;

        private const int ArcueidStartingMana = 500;
        private const int MysticEyesManaCost = 15;
        private const int MeltyBloodManaCost = 20;
        private const int MeltyBloodDamage = 25;

        public static Character CreateAoko(ILogger<Character> logger, ILogger<ResourcePool> resourceLogger)
        {
            var pool = new ResourcePool(AokoStartingMana, resourceLogger);
            Character aoko = new Character("Aoko", pool, logger);

            Spell snapAndDraw = new Spell("Snap & Draw", manaCost: SnapAndDrawManaCost, new DamageEffect(SnapAndDrawDamage));
            Spell earthlightStarbow = new Spell("Earthlight Starbow", manaCost: EarthlightStarbowManaCost, new DamageEffect(EarthlightStarbowDamage));

            aoko.AvailableSpells.Add(earthlightStarbow);
            aoko.AvailableSpells.Add(snapAndDraw);

            return aoko;
        }

        public static Character CreateArcueid(ILogger<Character> logger, ILogger<ResourcePool> resourceLogger)
        {
            var pool = new ResourcePool(ArcueidStartingMana, resourceLogger);
            Character arcueid = new Character("Arcueid", pool, logger);

            Spell mysticEyes = new Spell("Mystic Eyes of Enchantment", manaCost: MysticEyesManaCost, new VulnerableEffect());
            Spell meltyBlood = new Spell("Melty Blood", manaCost: MeltyBloodManaCost, new DamageEffect(MeltyBloodDamage));

            arcueid.AvailableSpells.Add(meltyBlood);
            arcueid.AvailableSpells.Add(mysticEyes);

            return arcueid;
        }
    }
}
