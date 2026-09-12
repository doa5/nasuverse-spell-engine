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

                foreach (ISpellEffect effect in spell.Effects)
                {
                    _logger.LogDebug("{Character} applies {EffectType} from {Spell}", Name, effect.GetType().Name, spell.Name);
                    effect.Apply(this, dojo);
                }

                _logger.LogInformation("{Character} successfully cast {Spell} with {EffectCount} effect(s) (DojoHP {Old} -> {New}). Remaining mana: {Mana}", Name, spell.Name, spell.Effects.Count, oldHp, dojo.TargetHP, Resources.Mana);
                return $"{Name} cast {spell.Name}! Dealt {spell.Damage} damage.";
            }

            _logger.LogWarning("{Character} failed to cast {Spell}: insufficient mana (has {Mana}, needs {Cost})", Name, spell.Name, Resources.Mana, spell.ManaCost);
            return $"{Name} tried to cast {spell.Name} but couldn't afford the mana cost.";
        }
    }
}
