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
            ICastingRules? castingRules = definition.CastingRulesFactory?.Invoke();
            Character character = new Character(definition.Name, pool, logger, castingRules);

            foreach (SpellDefinition spellDefinition in definition.Spells)
            {
                character.AvailableSpells.Add(CreateSpell(spellDefinition, definition.TransformStateFactory));
            }

            if (definition.TransformedSpells is not null)
            {
                character.AlternateSpells = definition.TransformedSpells
                    .Select(spellDefinition => CreateSpell(spellDefinition, definition.TransformStateFactory))
                    .ToList();
            }

            return character;
        }

        /// <summary>
        /// Builds a <see cref="Character"/> for every definition in <paramref name="roster"/>,
        /// preserving order (e.g. <see cref="CharacterCatalog.Roster"/>).
        /// </summary>
        public static IReadOnlyList<Character> CreateRoster(IReadOnlyList<CharacterDefinition> roster, ILogger<Character> logger, ILogger<ResourcePool> resourceLogger)
        {
            return roster
                .Select(definition => Create(definition, logger, resourceLogger))
                .ToList();
        }

        private static Spell CreateSpell(SpellDefinition definition, Func<ITransformState>? transformStateFactory)
        {
            ISpellEffect[] effects = definition.Effects.Select(effect => CreateEffect(effect, transformStateFactory)).ToArray();
            return new Spell(definition.Name, definition.ManaCost, effects);
        }

        private static ISpellEffect CreateEffect(EffectDefinition definition, Func<ITransformState>? transformStateFactory) => definition.Type switch
        {
            EffectType.Damage => new DamageEffect(definition.Amount),
            EffectType.Entropy => new EntropyEffect(definition.Amount),
            EffectType.Texture => new TextureEffect(),
            EffectType.AtmosphericBurst => new AtmosphericBurstEffect(),
            EffectType.Transform => new TransformEffect(transformStateFactory
                ?? throw new InvalidOperationException("A Transform effect requires the character's CharacterDefinition.TransformStateFactory to be set.")),
            EffectType.ExtendTransform => new ExtendTransformEffect(),
            EffectType.RestoreMana => new RestoreManaEffect(definition.Amount),
            _ => throw new ArgumentOutOfRangeException(nameof(definition), definition.Type, "Unknown effect type"),
        };
    }
}
