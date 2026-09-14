namespace NasuverseSpellEngine
{
    public record SpellEffectResult(
        string Message,
        int DamageDealt = 0,
        string? StatusApplied = null);
}
