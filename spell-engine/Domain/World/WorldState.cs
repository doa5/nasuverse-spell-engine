using System.Collections.Generic;

namespace NasuverseSpellEngine.Domain.World
{
    public sealed class WorldState
    {
        private const int MaxDurability = 1000;
        private const int MaxEntropy = 100;
        private const int MaxRecentEvents = 6;
        private const int HeatDeathVoidPassiveDecay = 20;

        private const int MillennialCastleDurationTurns = 4;
        private const int AtmosphericManaDurationTurns = 3;
        private const int AtmosphericManaDurationInCastleTurns = 2;
        private const int AtmosphericManaDurationInHeatDeathVoidTurns = 1;

        private readonly Queue<string> _recentEvents = new Queue<string>();

        private int _durability = MaxDurability;
        private int _entropy;
        private int _castleTurnsRemaining;
        private int _atmosphericTurnsRemaining;

        public int Durability
        {
            get => _durability;
            private set => _durability = Math.Max(0, value);
        }

        public int Entropy
        {
            get => _entropy;
            private set => _entropy = Math.Clamp(value, 0, MaxEntropy);
        }

        public bool AtmosphericMana { get; private set; }
        public RealityTexture ActiveTexture { get; private set; } = RealityTexture.Normal;
        public int TurnCount { get; private set; }

        public IReadOnlyCollection<string> RecentEvents => _recentEvents;

        public int ApplyDamage(int rawDamage)
        {
            int actualDamage = ActiveTexture == RealityTexture.MillennialCastle
                ? rawDamage / 2
                : rawDamage;

            Durability -= actualDamage;

            PushEvent($"Dojo took {actualDamage} damage.");
            return actualDamage;
        }

        public void AddEntropy(int amount)
        {
            // Heat Death Void is permanent and irreversible: entropy is locked at 100 forever.
            if (ActiveTexture == RealityTexture.HeatDeathVoid)
            {
                return;
            }

            Entropy += amount;

            if (Entropy >= MaxEntropy)
            {
                ActiveTexture = RealityTexture.HeatDeathVoid;
                _castleTurnsRemaining = 0;
                _atmosphericTurnsRemaining = 0;
                AtmosphericMana = false;
                PushEvent("Entropy reached 100%! The room has collapsed into a permanent Heat Death Void!");
            }
        }

        public bool TryActivateMillennialCastle()
        {
            if (ActiveTexture == RealityTexture.HeatDeathVoid)
            {
                PushEvent("Millennial Castle failed to manifest — the Earth is dead in this void.");
                return false;
            }

            ActiveTexture = RealityTexture.MillennialCastle;
            _castleTurnsRemaining = MillennialCastleDurationTurns;

            // Millennial Castle overwrites the room's rules and clears any ambient atmospheric mana.
            AtmosphericMana = false;
            _atmosphericTurnsRemaining = 0;

            PushEvent($"Millennial Castle Brunestud manifests, overwriting the room for {MillennialCastleDurationTurns} turns.");
            return true;
        }

        public void TriggerManaBurst()
        {
            int duration = ActiveTexture switch
            {
                RealityTexture.HeatDeathVoid => AtmosphericManaDurationInHeatDeathVoidTurns,
                RealityTexture.MillennialCastle => AtmosphericManaDurationInCastleTurns,
                _ => AtmosphericManaDurationTurns,
            };

            AtmosphericMana = true;
            _atmosphericTurnsRemaining = duration;

            PushEvent($"The atmosphere saturates with dense dragon mana for {duration} turn(s).");
        }

        public void AdvanceTurn()
        {
            if (ActiveTexture == RealityTexture.HeatDeathVoid)
            {
                Durability -= HeatDeathVoidPassiveDecay;
                PushEvent($"The Heat Death Void gnaws at the dojo. (-{HeatDeathVoidPassiveDecay} Durability)");
            }

            if (ActiveTexture == RealityTexture.MillennialCastle && _castleTurnsRemaining > 0)
            {
                _castleTurnsRemaining--;

                if (_castleTurnsRemaining == 0)
                {
                    ActiveTexture = RealityTexture.Normal;
                    PushEvent("Millennial Castle fades, and the dojo's walls return to normal.");
                }
            }

            if (AtmosphericMana && _atmosphericTurnsRemaining > 0)
            {
                _atmosphericTurnsRemaining--;

                if (_atmosphericTurnsRemaining == 0)
                {
                    AtmosphericMana = false;
                    PushEvent("The atmospheric mana dissipates.");
                }
            }

            TurnCount++;
        }

        public void PushEvent(string message)
        {
            _recentEvents.Enqueue(message);

            if (_recentEvents.Count > MaxRecentEvents)
            {
                _recentEvents.Dequeue();
            }
        }

        public void Reset()
        {
            Durability = MaxDurability;
            Entropy = 0;
            AtmosphericMana = false;
            ActiveTexture = RealityTexture.Normal;
            _castleTurnsRemaining = 0;
            _atmosphericTurnsRemaining = 0;
            _recentEvents.Clear();

            PushEvent("The dojo has been repaired and the world state reset.");
        }
    }
}

