namespace NasuverseSpellEngine
{
    public class TaigaDojo
    {
        private int _targetHP;
        public int TargetHP
        {
            get => _targetHP;
            set => _targetHP = Math.Max(0, value);  // Clamp to 0 minimum
        }

        public TaigaDojo(int initialHP = 100)
        {
            TargetHP = initialHP;
        }
    }
}
