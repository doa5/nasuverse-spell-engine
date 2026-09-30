namespace NasuverseSpellEngine.Application
{
    using NasuverseSpellEngine.Domain;

    /// <summary>
    /// The kind of effect a spell applies. Used to translate a data-only
    /// <see cref="EffectDefinition"/> into a concrete Domain.Effects.ISpellEffect.
    /// </summary>
    public enum EffectType
    {
        /// <summary>Deals raw damage to the dojo's durability. Uses <see cref="EffectDefinition.Amount"/>.</summary>
        Damage,

        /// <summary>Adds entropy to the world, pushing it toward Heat Death Void. Uses <see cref="EffectDefinition.Amount"/>.</summary>
        Entropy,

        /// <summary>Attempts to manifest the Millennial Castle texture. Ignores <see cref="EffectDefinition.Amount"/>.</summary>
        Texture,

        /// <summary>Saturates the atmosphere with dragon mana. Ignores <see cref="EffectDefinition.Amount"/>.</summary>
        AtmosphericBurst,

        /// <summary>Activates the character's alternate spell set/transform (e.g. Aoko's Redshift). Ignores <see cref="EffectDefinition.Amount"/>.</summary>
        Transform,

        /// <summary>Extends the character's active transform (e.g. "I need more time!"). Ignores <see cref="EffectDefinition.Amount"/>.</summary>
        ExtendTransform,

        /// <summary>Restores the caster's mana to full at the cost of world entropy. Uses <see cref="EffectDefinition.Amount"/> as the entropy cost.</summary>
        RestoreMana
    }

    /// <summary>Plain data describing one spell effect (no behavior, no Domain dependency).</summary>
    public sealed record EffectDefinition(EffectType Type, int Amount = 0);

    /// <summary>Plain data describing one spell (no behavior, no Domain dependency).</summary>
    public sealed record SpellDefinition(string Name, int ManaCost, params EffectDefinition[] Effects);

    /// <summary>
    /// Plain data describing one character's starting stats and spell list.
    /// <paramref name="CastingRulesFactory"/> is an explicit, compiler-checked way to
    /// attach a character's <see cref="ICastingRules"/> at the definition site,
    /// instead of re-deriving it later from <paramref name="Name"/>.
    /// <paramref name="TransformedSpells"/> is an optional alternate spell list the
    /// character swaps to when a <see cref="EffectType.Transform"/> spell is cast
    /// (e.g. Adult Aoko's kit), and back from when the transform expires.
    /// </summary>
    public sealed record CharacterDefinition(
        string Name,
        int StartingMana,
        IReadOnlyList<SpellDefinition> Spells,
        Func<ICastingRules>? CastingRulesFactory = null,
        IReadOnlyList<SpellDefinition>? TransformedSpells = null);
}
