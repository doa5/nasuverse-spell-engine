namespace NasuverseSpellEngine.Domain
{
    /// <summary>
    /// Character-local transformation state (e.g. Aoko's Redshift). Unlike a
    /// <see cref="World.WorldState.ActiveTexture"/>, this is personal to the
    /// character holding it and is not shared/visible on the world state.
    /// </summary>
    public interface ITransformState
    {
        /// <summary>True while the transformation is currently active.</summary>
        bool IsActive { get; }

        /// <summary>
        /// Advances the transform by one turn, reverting it (and firing
        /// <paramref name="onRevert"/>) if its duration has elapsed.
        /// </summary>
        void Tick(Action onRevert);
    }
}
