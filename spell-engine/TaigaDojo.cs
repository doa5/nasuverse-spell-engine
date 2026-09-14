using Microsoft.Extensions.Logging;

namespace NasuverseSpellEngine
{
    public class TaigaDojo
    {
        private int _targetHP;
        private bool _isVulnerable;
        private readonly ILogger<TaigaDojo> _logger;

        public int TargetHP
        {
            get => _targetHP;
            set
            {
                int old = _targetHP;
                int clamped = Math.Max(0, value);
                _targetHP = clamped;
                _logger.LogInformation("TaigaDojo HP changed: {Old} -> {New}", old, _targetHP);
                if (_targetHP == 0 && old > 0)
                {
                    _logger.LogWarning("TaigaDojo has been destroyed");
                }
            }
        }

        public TaigaDojo(int initialHP, ILogger<TaigaDojo> logger)
        {
            _logger = logger;
            TargetHP = initialHP;
        }

        public void ApplyVulnerability()
        {
            if (_isVulnerable)
            {
                _logger.LogInformation("TaigaDojo is already vulnerable");
                return;
            }

            _isVulnerable = true;
            _logger.LogInformation("TaigaDojo is now vulnerable to the next attack");
        }

        public int TakeDamage(int damage)
        {
            int multiplier = _isVulnerable ? 2 : 1;
            int appliedDamage = damage * multiplier;
            _isVulnerable = false;

            _logger.LogInformation("TaigaDojo took {Damage} base damage x{Multiplier} = {AppliedDamage}", damage, multiplier, appliedDamage);
            if (multiplier > 1)
            {
                _logger.LogInformation("TaigaDojo vulnerability consumed");
            }

            TargetHP -= appliedDamage;
            return appliedDamage;
        }
    }
}
