namespace NasuverseSpellEngine
{
    public class Character
    {
        public string Name { get; }
        public ResourcePool Resources { get; }

        public Character(string name, ResourcePool resources)
        {
            Name = name;
            Resources = resources;
        }

        public string CastSpell(Spell spell, TaigaDojo dojo)
        {
            if (Resources.TryConsume(spell.ManaCost))
            { 
                dojo.TargetHP -= spell.Damage;
                return $"{Name} cast {spell.Name}! Dealt {spell.Damage} damage.";
            }

            return $"{Name} tried to cast {spell.Name} but couldn't afford the mana cost.";
        }
    }
}
