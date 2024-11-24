using System.Collections.Generic;
using CardGame.Abstract;
using CardGame.Views;
using TMPro;
using UnityEngine;

namespace CardGame.UI
{
    public class BoardUI : MonoBehaviour, IToggleble
    {
        [SerializeField] private RectTransform m_boardRoot;
        [SerializeField] private TMP_Text m_outcomeText;
        [SerializeField] private Color m_receiveDamageColor = Color.red;    
        [SerializeField] private Color m_dealDamageColor = Color.green;    
        [SerializeField] private Color m_noDamageColor = Color.yellow;

        private const string RECEIVE_DAMAGE_MESSAGE = "Receive {0} damage";
        private const string DEAL_DAMAGE_MESSAGE = "Deal {0} damage";
        private const string NO_DAMAGE_MESSAGE = "No damage (Draw)";
        
        private List<CardView> m_cardViews = new();
        
        public RectTransform BoardRoot => m_boardRoot;
        
        public void AddView(CardView cardView)
        {
            m_cardViews.Add(cardView);
        }

        public void ShowOutcome(int damage, bool isDamage)
        {
            //No damage
            if (damage <= 0)
            {
                ShowOutcomeMessage(NO_DAMAGE_MESSAGE, m_noDamageColor);
                return;
            }

            var message = isDamage ? 
                string.Format(RECEIVE_DAMAGE_MESSAGE, damage) : 
                string.Format(DEAL_DAMAGE_MESSAGE, damage);
            
            var color = isDamage ? 
                m_receiveDamageColor : 
                m_dealDamageColor;
            
            ShowOutcomeMessage(message, color);
        }

        public void HideOutcome() => m_outcomeText.gameObject.SetActive(false);

        public void Toggle(bool isActive)
        {
            //By default outcomeText is always disabled
            HideOutcome();
            gameObject.SetActive(isActive);
        }

        public void Cleanup()
        {
            foreach (var view in m_cardViews)
            {
                if (view == null)
                    continue;
                
                Destroy(view.gameObject);
            }
            
            m_cardViews.Clear();
            Toggle(false);
        }
        
        private void ShowOutcomeMessage(string message, Color color)
        {
            m_outcomeText.text = message;
            m_outcomeText.color = color;
            m_outcomeText.gameObject.SetActive(true);
        }
    }
}
