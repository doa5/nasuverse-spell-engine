using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.Effects;

namespace NasuverseSpellEngine.Application
{
    public static class CharacterFactory
    {
        public static Character Create(CharacterDefinition definition, ILogger<Character> logger, ILogger<ResourcePool> resourceLogger)
        {
            var pool = new ResourcePool(definition.StartingMana, resourceLogger);
            Character character = new Character(definition.Name, pool, logger);

            foreach (SpellDefinition spellDefinition in definition.Spells)
            {
                character.AvailableSpells.Add(CreateSpell(spellDefinition));
            }

            return character;
        }

        private static Spell CreateSpell(SpellDefinition definition)
        {
            ISpellEffect effect = CreateEffect(definition.Effect);
            return new Spell(definition.Name, definition.ManaCost, effect);
        }

        private static ISpellEffect CreateEffect(EffectDefinition definition) => definition.Type switch
        {
            EffectType.Damage => new DamageEffect(definition.Amount),
            _ => throw new ArgumentOutOfRangeException(nameof(definition), definition.Type, "Unknown effect type"),
        };
    }
}
