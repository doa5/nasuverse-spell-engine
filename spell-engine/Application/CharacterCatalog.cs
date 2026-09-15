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
                new SpellDefinition("Mystic Eyes of Enchantment", ManaCost: 15, new EffectDefinition(EffectType.Vulnerable)),
            ]);
    }
}
