using TMPro;
using UnityEngine;
using CardGame.Data;
using UnityEngine.UI;

namespace CardGame.Views
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image m_healthBar;
        [SerializeField] private TMP_Text m_healthText;
        
        public void Initialize(HealthData healthData)
        {
            healthData.OnHealthChanged += OnHealthChanged;
            healthData.OnPlayerKilled += OnPlayerKilled;
            
            UpdateHealth(healthData.CurrentHealth, healthData.MaxHealth);
        }

        private void UpdateHealth(int currentHealth, int maxHealth)
        {
            if (maxHealth < 0)
            {
                m_healthText.text = string.Empty;
                m_healthBar.fillAmount = 0;
                return;
            }

            m_healthText.text = $"{currentHealth}\n/\n{maxHealth}";
            m_healthBar.fillAmount = currentHealth / (float)maxHealth;
        }

        private void OnHealthChanged(int currentHealth, int maxHealth) => UpdateHealth(currentHealth, maxHealth);

        private void OnPlayerKilled() => UpdateHealth(0, -1);
    }
}
