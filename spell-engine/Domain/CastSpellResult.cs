namespace NasuverseSpellEngine.Domain
{
    /// <summary>
    /// Structured outcome of a spell-casting attempt. Kept free of any display/formatting
    /// concerns so callers (e.g. a console UI) can decide how to present it.
    /// </summary>
    public record CastSpellResult(
        bool Success,
        Character Caster,
        Spell Spell,
        IReadOnlyList<SpellEffectResult> EffectResults)
    {
        public int TotalDamageDealt => EffectResults.Sum(result => result.DamageDealt);
    }
}
