namespace NasuverseSpellEngine.Application
{
    using NasuverseSpellEngine.Domain;
    using NasuverseSpellEngine.Domain.Characters;

    /// <summary>
    /// Source of truth for character/spell data. Keeping this separate from
    /// CharacterFactory means game balance can be read and tuned here without
    /// touching how a Character gets constructed.
    /// </summary>
    public static class CharacterCatalog
    {
    /// <summary>
    /// The Fifth Magician. As Teen Aoko she fights with basic magecraft and can cast
    /// Redshift to borrow her future self's power, becoming Adult Aoko - hitting far
    /// harder at the cost of entropy, for a limited number of turns.
    /// </summary>
    public static readonly CharacterDefinition Aoko = new(
            Name: "Aoko",
            StartingMana: 250,
            Spells:
            [
                new SpellDefinition("Right Hook", ManaCost: 0, [new EffectDefinition(EffectType.Damage, Amount: 10)], Description: "A quick, mana-free jab. Weak, but always available."),
                new SpellDefinition("Starmine", ManaCost: 40, [new EffectDefinition(EffectType.Damage, Amount: 60)], Description: "Teen Aoko's strongest normal magecraft - a focused explosive bolt."),
                new SpellDefinition("Redshift", ManaCost: 30, [new EffectDefinition(EffectType.Transform), new EffectDefinition(EffectType.Entropy, Amount: 20)], Description: "Borrows her future self's power, transforming into Adult Aoko for several turns at the cost of entropy."),
            ],
            CastingRulesFactory: () => new AokoCastingRules(),
            TransformedSpells:
            [
                new SpellDefinition("Starmine - Octogram", ManaCost: 25, [new EffectDefinition(EffectType.Damage, Amount: 35), new EffectDefinition(EffectType.Entropy, Amount: 10)], Description: "Adult Aoko's upgraded Starmine - now just a solid normal attack."),
                new SpellDefinition("Earthlight Starbow", ManaCost: 150, [new EffectDefinition(EffectType.Damage, Amount: 150), new EffectDefinition(EffectType.Entropy, Amount: 25)], Description: "Adult Aoko's ultimate destruction sorcery."),
                new SpellDefinition("I need more time!", ManaCost: 0, [new EffectDefinition(EffectType.ExtendTransform)], Description: "Throws more of her time to the future, extending Redshift at an escalating entropy cost."),
                new SpellDefinition("I need more mana!", ManaCost: 0, [new EffectDefinition(EffectType.RestoreMana, Amount: 30)], Description: "Refills her mana to full at the cost of entropy."),
            ],
            TransformStateFactory: () => new AokoTransformState(),
            AccentColor: "blue");

    /// <summary>
    /// True Ancestor and last remaining Brunestud. Marble Phantasm - Seal overwrites the
    /// room's reality texture into the Millennial Castle, under which her own attacks
    /// deal double damage; Melty Blood is her true-form finisher.
    /// </summary>
    public static readonly CharacterDefinition Arcueid = new(
        Name: "Arcueid",
        StartingMana: 500,
        Spells:
        [
                new SpellDefinition("Out of my Way!", ManaCost: 20, [new EffectDefinition(EffectType.Damage, Amount: 35)], Description: "A dismissive but forceful strike."),
                new SpellDefinition("Marble Phantasm - Seal", ManaCost: 150, [new EffectDefinition(EffectType.Texture)], Description: "Overwrites the room's reality into the Millennial Castle."),
                new SpellDefinition("Melty Blood", ManaCost: 300, [new EffectDefinition(EffectType.Damage, Amount: 140)], Description: "Her true-form finisher, drawing on vampiric power."),
        ],
        CastingRulesFactory: () => new ArcueidCastingRules(),
        AccentColor: "white");

    /// <summary>
    /// King of Knights. Standard attack (Invisible Air) is an unseen wind-blade strike;
    /// Mana Burst saturates the atmosphere with dense dragon mana ahead of her Noble
    /// Phantasm; Excalibur is her NP-tier finisher.
    /// </summary>
    public static readonly CharacterDefinition Saber = new(
        Name: "Saber",
        StartingMana: 350,
        Spells:
        [
                new SpellDefinition("Invisible Air", ManaCost: 25, [new EffectDefinition(EffectType.Damage, Amount: 40)], Description: "An unseen wind-blade strike."),
                new SpellDefinition("Mana Burst", ManaCost: 30, [new EffectDefinition(EffectType.AtmosphericBurst)], Description: "Saturates the atmosphere with dense dragon mana."),
                new SpellDefinition("Excalibur", ManaCost: 300, [new EffectDefinition(EffectType.Damage, Amount: 130)], Description: "Her Noble Phantasm-tier finisher."),
        ],
        CastingRulesFactory: () => new SaberCastingRules(),
        AccentColor: "gold1");

    /// <summary>
    /// Debug-only character with simple, predictable spells for manually smoke-testing
    /// world-state thresholds (durability bands, destruction, etc.) without needing
    /// dozens of real casts. Intentionally excluded from <see cref="Roster"/> so it
    /// never shows up in normal play, but kept available for future debugging.
    /// </summary>
    public static readonly CharacterDefinition Debug = new(
        Name: "Debug",
        StartingMana: 99999,
        Spells:
        [
            new SpellDefinition("Pinprick", ManaCost: 0, [new EffectDefinition(EffectType.Damage, Amount: 1)], Description: "A tiny, deliberate amount of damage."),
            new SpellDefinition("Half Shatter", ManaCost: 0, [new EffectDefinition(EffectType.Damage, Amount: 500)], Description: "A large chunk of damage for testing durability bands."),
            new SpellDefinition("Obliterate", ManaCost: 0, [new EffectDefinition(EffectType.Damage, Amount: 9999)], Description: "Overkill damage for testing dojo destruction."),
            new SpellDefinition("Debug Entropy Surge", ManaCost: 0, [new EffectDefinition(EffectType.Entropy, Amount: 25)], Description: "Adds entropy for testing Heat Death Void."),
            new SpellDefinition("Debug Texture Shift", ManaCost: 0, [new EffectDefinition(EffectType.Texture)], Description: "Manifests the Millennial Castle for testing."),
            new SpellDefinition("Debug Mana Saturate", ManaCost: 0, [new EffectDefinition(EffectType.AtmosphericBurst)], Description: "Saturates the atmosphere for testing."),
        ],
        AccentColor: "grey");

    /// <summary>
    /// Every playable character definition, in menu display order. Adding a new
    /// character only requires adding it here (and defining it above) - no other
    /// code needs to change to make it selectable.
    /// </summary>
    public static readonly IReadOnlyList<CharacterDefinition> Roster =
        [
            Aoko,
            Arcueid,
            Saber,
        ];
    }
}
