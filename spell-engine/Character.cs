using Microsoft.Extensions.Logging;

namespace NasuverseSpellEngine
{
    public class Character
    {
        public string Name { get; }
        public ResourcePool Resources { get; }
        public List<Spell> AvailableSpells { get; }

        private readonly ILogger<Character> _logger;

        public Character(string name, ResourcePool resources, ILogger<Character> logger)
        {
            Name = name;
            Resources = resources;
            AvailableSpells = new List<Spell>();
            _logger = logger;
        }

        public string CastSpell(Spell spell, TaigaDojo dojo)
        {
            _logger.LogDebug("{Character} attempts to cast {Spell} (cost {Cost}), current mana {Mana}", Name, spell.Name, spell.ManaCost, Resources.Mana);

            if (Resources.TryConsume(spell.ManaCost))
            {
                int oldHp = dojo.TargetHP;
                List<SpellEffectResult> effectResults = new List<SpellEffectResult>();

                foreach (ISpellEffect effect in spell.Effects)
                {
                    effectResults.Add(effect.Apply(this, dojo));
                }

                int totalDamageDealt = effectResults.Sum(result => result.DamageDealt);
                string effectSummary = string.Join(" ", 
                    effectResults
                    .Select(result => result.Message)
                    .Where(message => !string.IsNullOrWhiteSpace(message)));

                _logger.LogInformation("{Character} successfully cast {Spell} with {EffectCount} effect(s) (DojoHP {Old} -> {New}, Damage {Damage}). Remaining mana: {Mana}", Name, spell.Name, spell.Effects.Count, oldHp, dojo.TargetHP, totalDamageDealt, Resources.Mana);

                return string.IsNullOrWhiteSpace(effectSummary)
                    ? $"{Name} cast {spell.Name}!"
                    : $"{Name} cast {spell.Name}! {effectSummary}";
            }

            _logger.LogWarning("{Character} failed to cast {Spell}: insufficient mana (has {Mana}, needs {Cost})", Name, spell.Name, Resources.Mana, spell.ManaCost);
            return $"{Name} tried to cast {spell.Name} but couldn't afford the mana cost.";
        }
    }
}
