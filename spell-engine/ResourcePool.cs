namespace NasuverseSpellEngine
{
    public class ResourcePool
    {
        public int Mana { get; private set; }

        public ResourcePool(int initialMana)
        {
            Mana = initialMana;
        }

        public bool TryConsume(int amount)
        {
            if (amount < 0) return false;
            if (Mana >= amount)
            {
                Mana -= amount;
                return true;
            }
            return false;
        }

        public void Add(int amount)
        {
            if (amount < 0) return;
            Mana += amount;
        }
    }
}
