namespace NasuverseSpellEngine
{
    public record SpellEffectResult(
        bool Success,
        string Message,
        int DamageDealt = 0,
        string? StatusApplied = null);
}
