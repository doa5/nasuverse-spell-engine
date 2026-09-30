namespace NasuverseSpellEngine.Domain
{
    /// <summary>
    /// Tracks Aoko's Redshift transformation: how many turns of Adult Aoko remain,
    /// and how many times "I need more time!" has been used this activation (each
    /// extension costs more entropy than the last).
    /// </summary>
    public class AokoTransformState : ITransformState
    {
        /// <summary>How many turns Redshift lasts before reverting to Teen Aoko.</summary>
        public const int BaseDurationTurns = 5;

        /// <summary>How many turns each "I need more time!" cast adds.</summary>
        public const int ExtensionTurns = 3;

        /// <summary>
        /// Entropy cost of the Nth "I need more time!" cast this activation
        /// (index 0 = first cast). Escalates steeply so extending Redshift
        /// indefinitely isn't free.
        /// </summary>
        private static readonly int[] ExtensionEntropyCosts = [20, 35, 55, 80];

        public int TurnsRemaining { get; private set; }

        public int ExtensionCount { get; private set; }

        public bool IsActive => TurnsRemaining > 0;

        public AokoTransformState()
        {
            // The turn Redshift is cast is spent activating it, not ticking down its
            // duration, so the counter starts one higher than the advertised duration
            // (mirrors WorldState.ToTurnCounter).
            TurnsRemaining = BaseDurationTurns + 1;
        }

        public void Tick(Action onRevert)
        {
            if (!IsActive)
            {
                return;
            }

            TurnsRemaining--;

            if (TurnsRemaining <= 0)
            {
                ExtensionCount = 0;
                onRevert();
            }
        }

        /// <summary>Entropy cost of the next "I need more time!" cast.</summary>
        public int GetNextExtensionCost()
        {
            int index = Math.Min(ExtensionCount, ExtensionEntropyCosts.Length - 1);
            return ExtensionEntropyCosts[index];
        }

        /// <summary>Adds <see cref="ExtensionTurns"/> and records the extension.</summary>
        public void Extend()
        {
            TurnsRemaining += ExtensionTurns;
            ExtensionCount++;
        }
    }
}
