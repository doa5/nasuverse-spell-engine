namespace NasuverseSpellEngine.Application
{
    /// <summary>
    /// The kind of effect a spell applies. Used to translate a data-only
    /// <see cref="EffectDefinition"/> into a concrete Domain.Effects.ISpellEffect.
    /// </summary>
    public enum EffectType
    {
        Damage,
        Vulnerable
    }

    /// <summary>Plain data describing one spell effect (no behavior, no Domain dependency).</summary>
    public sealed record EffectDefinition(EffectType Type, int Amount = 0);

    /// <summary>Plain data describing one spell (no behavior, no Domain dependency).</summary>
    public sealed record SpellDefinition(string Name, int ManaCost, EffectDefinition Effect);

    /// <summary>Plain data describing one character's starting stats and spell list.</summary>
    public sealed record CharacterDefinition(string Name, int StartingMana, IReadOnlyList<SpellDefinition> Spells);
}
