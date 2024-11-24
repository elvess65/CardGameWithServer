namespace CardGame.Data
{
    [System.Serializable]
    public class HealthData
    {
        public event System.Action<int, int> OnHealthChanged;
        public event System.Action OnPlayerKilled;

        public int CurrentHealth;
        public int MaxHealth;

        public HealthData(int maxHealth)
        {
            CurrentHealth = maxHealth;
            MaxHealth = maxHealth;
        }

        public void DecreaseHealth(int amount)
        {
            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                OnPlayerKilled?.Invoke();
            }
            else
            {
                OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            }
        }
    }
}
