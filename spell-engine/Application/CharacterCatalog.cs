namespace NasuverseSpellEngine.Application
{
    /// <summary>
    /// Source of truth for character/spell data. Keeping this separate from
    /// CharacterFactory means game balance can be read and tuned here without
    /// touching how a Character gets constructed.
    /// </summary>
    public static class CharacterCatalog
    {
        public static readonly CharacterDefinition Aoko = new(
            Name: "Aoko",
            StartingMana: 200,
            Spells:
            [
                new SpellDefinition("Earthlight Starbow", ManaCost: 50, new EffectDefinition(EffectType.Damage, Amount: 35)),
                new SpellDefinition("Snap & Draw", ManaCost: 20, new EffectDefinition(EffectType.Damage, Amount: 5)),
            ]);

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
                new SpellDefinition("Out of my Way!", ManaCost: 20, new EffectDefinition(EffectType.Damage, Amount: 35)),
                new SpellDefinition("Marble Phantasm - Seal", ManaCost: 150, new EffectDefinition(EffectType.Texture)),
                new SpellDefinition("Melty Blood", ManaCost: 300, new EffectDefinition(EffectType.Damage, Amount: 140)),
        ]);

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
                new SpellDefinition("Invisible Air", ManaCost: 25, new EffectDefinition(EffectType.Damage, Amount: 40)),
                new SpellDefinition("Mana Burst", ManaCost: 30, new EffectDefinition(EffectType.AtmosphericBurst)),
                new SpellDefinition("Excalibur", ManaCost: 300, new EffectDefinition(EffectType.Damage, Amount: 130)),
        ]);

    /// <summary>
    /// Debug-only character with simple, predictable spells for manually smoke-testing
    /// world-state thresholds (durability bands, destruction, etc.) without needing
    /// dozens of real casts.
    /// </summary>
    public static readonly CharacterDefinition Debug = new(
        Name: "Debug",
        StartingMana: 99999,
        Spells:
        [
            new SpellDefinition("Pinprick", ManaCost: 0, new EffectDefinition(EffectType.Damage, Amount: 1)),
            new SpellDefinition("Half Shatter", ManaCost: 0, new EffectDefinition(EffectType.Damage, Amount: 500)),
            new SpellDefinition("Obliterate", ManaCost: 0, new EffectDefinition(EffectType.Damage, Amount: 9999)),
            new SpellDefinition("Debug Entropy Surge", ManaCost: 0, new EffectDefinition(EffectType.Entropy, Amount: 25)),
            new SpellDefinition("Debug Texture Shift", ManaCost: 0, new EffectDefinition(EffectType.Texture)),
            new SpellDefinition("Debug Mana Saturate", ManaCost: 0, new EffectDefinition(EffectType.AtmosphericBurst)),
        ]);

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
            Debug,
        ];
    }
}
