using NasuverseSpellEngine.Domain;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Application
{
    /// <summary>
    /// Typed bundle of everything <see cref="DashboardRenderer"/> needs to draw one
    /// full screen. Pure data — no behavior, no console I/O. Built fresh by
    /// <see cref="TrainingSession"/> each time the screen needs to be redrawn.
    /// </summary>
    public record DashboardView(
        Character ActiveCharacter,
        WorldState World,
        string? LastAction);
}
