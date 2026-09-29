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

    public static readonly CharacterDefinition Arcueid = new(
        Name: "Arcueid",
        StartingMana: 500,
        Spells:
        [
                new SpellDefinition("Melty Blood", ManaCost: 20, new EffectDefinition(EffectType.Damage, Amount: 25)),
                new SpellDefinition("Marble Phantasm - Seal", ManaCost: 500, new EffectDefinition(EffectType.Damage, Amount: 200)),
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
            Debug,
        ];
    }
}
