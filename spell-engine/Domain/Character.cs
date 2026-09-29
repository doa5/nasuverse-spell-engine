using Microsoft.Extensions.Logging;
using NasuverseSpellEngine.Domain.Effects;
using NasuverseSpellEngine.Domain.World;

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

        public CastSpellResult CastSpell(Spell spell, WorldState world)
        {
            _logger.LogDebug("{Character} attempts to cast {Spell} (cost {Cost}), current mana {Mana}", Name, spell.Name, spell.ManaCost, Resources.Mana);

            if (Resources.TryConsume(spell.ManaCost))
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

            _logger.LogWarning("{Character} failed to cast {Spell}: insufficient mana (has {Mana}, needs {Cost})", Name, spell.Name, Resources.Mana, spell.ManaCost);
            return new CastSpellResult(Success: false, Caster: this, Spell: spell, EffectResults: []);
        }
    }
}
