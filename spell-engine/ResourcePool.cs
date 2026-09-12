using Microsoft.Extensions.Logging;

namespace NasuverseSpellEngine
{
    public class ResourcePool
    {
        public int Mana { get; private set; }

        private readonly ILogger<ResourcePool> _logger;

        public ResourcePool(int initialMana, ILogger<ResourcePool> logger)
        {
            Mana = initialMana;
            _logger = logger;
            _logger.LogDebug("ResourcePool created with initial mana {Mana}", Mana);
        }

        public bool TryConsume(int amount)
        {
            if (amount < 0)
            {
                _logger.LogWarning("TryConsume called with negative amount: {Amount}", amount);
                return false;
            }
            if (Mana >= amount)
            {
                Mana -= amount;
                _logger.LogDebug("Consumed {Amount} mana, remaining {Mana}", amount, Mana);
                return true;
            }
            _logger.LogInformation("Failed to consume {Amount} mana: only {Mana} available", amount, Mana);
            return false;
        }

        public void Add(int amount)
        {
            if (amount < 0)
            {
                _logger.LogWarning("Add called with negative amount: {Amount}", amount);
                return;
            }
            Mana += amount;
            _logger.LogDebug("Added {Amount} mana, new total {Mana}", amount, Mana);
        }
    }
}
