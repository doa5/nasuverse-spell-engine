using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain.Effects;
using NasuverseSpellEngine.Domain.World;

namespace NasuverseSpellEngine.Domain
{
    public class Character
    {
        public string Name { get; }
        public string AccentColor { get; }
        public ResourcePool Resources { get; }
        public List<Spell> AvailableSpells { get; private set; }
        public ICastingRules CastingRules { get; }

        /// <summary>
        /// Character-local transformation state (e.g. Aoko's Redshift). Null for
        /// characters that never transform.
        /// </summary>
        public ITransformState? Transform { get; set; }

        /// <summary>
        /// The spell list to swap to/from when <see cref="Transform"/> activates or
        /// reverts (e.g. Adult Aoko's kit). Null for characters that never transform.
        /// </summary>
        public List<Spell>? AlternateSpells { get; set; }

        private readonly ILogger<Character> _logger;

        public Character(string name, ResourcePool resources, ILogger<Character> logger, ICastingRules? castingRules = null, string accentColor = "grey")
        {
            Name = name;
            AccentColor = accentColor;
            Resources = resources;
            AvailableSpells = new List<Spell>();
            CastingRules = castingRules ?? new DefaultCastingRules();
            _logger = logger;
        }

        /// <summary>
        /// Swaps <see cref="AvailableSpells"/> with <see cref="AlternateSpells"/>
        /// (e.g. Teen Aoko &lt;-&gt; Adult Aoko). No-op if there is no alternate kit.
        /// </summary>
        public void SwapSpellSet()
        {
            if (AlternateSpells is null)
            {
                return;
            }

            (AvailableSpells, AlternateSpells) = (AlternateSpells, AvailableSpells);
        }

        /// <summary>
        /// Advances this character's <see cref="Transform"/> by one turn, if any,
        /// swapping back to the original spell set when it expires. While
        /// transformed and the world is dangerously entropic, borrowed power keeps
        /// bleeding entropy passively even without casting anything.
        /// </summary>
        public void TickTransform(WorldState world)
        {
            if (Transform is AokoTransformState { IsActive: true } && world.Entropy >= AokoTransformState.PassiveEscalationEntropyThreshold)
            {
                world.AddEntropy(AokoTransformState.PassiveEscalationEntropyPerTurn);
                world.PushEvent($"World interaction: {Name}'s borrowed time bleeds entropy on its own ({AokoTransformState.PassiveEscalationEntropyPerTurn}% passively) at {world.Entropy}% entropy.");
            }

            Transform?.Tick(SwapSpellSet);
        }

        public CastSpellResult CastSpell(Spell spell, WorldState world)
        {
            if (!CastingRules.CanCast(spell, world))
            {
                _logger.LogWarning("{Character} is not allowed to cast {Spell} given current world conditions", Name, spell.Name);
                return new CastSpellResult(Success: false, Caster: this, Spell: spell, EffectResults: []);
            }

            int manaCost = CastingRules.GetManaCost(spell, world);

            _logger.LogDebug("{Character} attempts to cast {Spell} (cost {Cost}), current mana {Mana}", Name, spell.Name, manaCost, Resources.Mana);

            if (manaCost != spell.ManaCost)
            {
                world.PushEvent(manaCost < spell.ManaCost
                    ? $"World interaction: {Name}'s mana cost for {spell.Name} dropped from {spell.ManaCost} to {manaCost}."
                    : $"World interaction: {Name}'s mana cost for {spell.Name} rose from {spell.ManaCost} to {manaCost}.");
            }

            if (Resources.TryConsume(manaCost))
            {
                int oldDurability = world.Durability;
                List<SpellEffectResult> effectResults = new List<SpellEffectResult>();

                foreach (ISpellEffect effect in spell.Effects)
                {
                    effectResults.Add(effect.Apply(this, world));
                }

                int totalDamageDealt = effectResults.Sum(result => result.DamageDealt);

                _logger.LogInformation("{Character} successfully cast {Spell} with {EffectCount} effect(s) (Durability {Old} -> {New}, Damage {Damage}). Remaining mana: {Mana}", Name, spell.Name, spell.Effects.Count, oldDurability, world.Durability, totalDamageDealt, Resources.Mana);

                return new CastSpellResult(Success: true, Caster: this, Spell: spell, EffectResults: effectResults);
            }

            _logger.LogWarning("{Character} failed to cast {Spell}: insufficient mana (has {Mana}, needs {Cost})", Name, spell.Name, Resources.Mana, manaCost);
            return new CastSpellResult(Success: false, Caster: this, Spell: spell, EffectResults: []);
        }
    }
}
