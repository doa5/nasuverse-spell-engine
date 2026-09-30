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
    public sealed record SpellDefinition(string Name, int ManaCost, IReadOnlyList<EffectDefinition> Effects, string? Description = null);

    /// <summary>
    /// Plain data describing one character's starting stats and spell list.
    /// <paramref name="CastingRulesFactory"/> is an explicit, compiler-checked way to
    /// attach a character's <see cref="ICastingRules"/> at the definition site,
    /// instead of re-deriving it later from <paramref name="Name"/>.
    /// <paramref name="TransformedSpells"/> is an optional alternate spell list the
    /// character swaps to when a <see cref="EffectType.Transform"/> spell is cast
    /// (e.g. Adult Aoko's kit), and back from when the transform expires.
    /// <paramref name="TransformStateFactory"/> is the matching explicit factory for
    /// the <see cref="ITransformState"/> a <see cref="EffectType.Transform"/> spell
    /// should construct - required whenever <paramref name="TransformedSpells"/> is set.
    /// <parameref name="AccentColor"/> is a Spectre.Console markup color name (e.g. "red")
    /// used to give the character's dashboard a quick visual identity.
    /// <paramref name="TransformedAccentColor"/> is an optional alternate accent color
    /// used instead of <paramref name="AccentColor"/> while the character's
    /// <see cref="ITransformState"/> is active (e.g. Aoko turning red during Redshift).
    /// </summary>
    public sealed record CharacterDefinition(
        string Name,
        int StartingMana,
        IReadOnlyList<SpellDefinition> Spells,
        Func<ICastingRules>? CastingRulesFactory = null,
        IReadOnlyList<SpellDefinition>? TransformedSpells = null,
        Func<ITransformState>? TransformStateFactory = null,
        string AccentColor = "grey",
        string? TransformedAccentColor = null);
}
