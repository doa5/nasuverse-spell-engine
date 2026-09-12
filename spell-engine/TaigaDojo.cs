using Microsoft.Extensions.Logging;

namespace NasuverseSpellEngine
{
    public class TaigaDojo
    {
        private int _targetHP;
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
    }
}
