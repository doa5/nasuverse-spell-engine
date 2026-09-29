namespace NasuverseSpellEngine.Application
{
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
        AtmosphericBurst
    }

    /// <summary>Plain data describing one spell effect (no behavior, no Domain dependency).</summary>
    public sealed record EffectDefinition(EffectType Type, int Amount = 0);

    /// <summary>Plain data describing one spell (no behavior, no Domain dependency).</summary>
    public sealed record SpellDefinition(string Name, int ManaCost, EffectDefinition Effect);

    /// <summary>Plain data describing one character's starting stats and spell list.</summary>
    public sealed record CharacterDefinition(string Name, int StartingMana, IReadOnlyList<SpellDefinition> Spells);
}
