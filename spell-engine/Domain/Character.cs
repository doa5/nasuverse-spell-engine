using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain.Effects;

namespace NasuverseSpellEngine.Domain
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

        public CastSpellResult CastSpell(Spell spell, TaigaDojo dojo)
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

                _logger.LogInformation("{Character} successfully cast {Spell} with {EffectCount} effect(s) (DojoHP {Old} -> {New}, Damage {Damage}). Remaining mana: {Mana}", Name, spell.Name, spell.Effects.Count, oldHp, dojo.TargetHP, totalDamageDealt, Resources.Mana);

                return new CastSpellResult(Success: true, Caster: this, Spell: spell, EffectResults: effectResults);
            }

            _logger.LogWarning("{Character} failed to cast {Spell}: insufficient mana (has {Mana}, needs {Cost})", Name, spell.Name, Resources.Mana, spell.ManaCost);
            return new CastSpellResult(Success: false, Caster: this, Spell: spell, EffectResults: []);
        }
    }
}
