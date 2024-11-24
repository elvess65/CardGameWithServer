using TMPro;
using UnityEngine;
using CardGame.Abstract;

namespace CardGame.UI
{
    public class FinishGameUI : MonoBehaviour, IToggleble
    {
        [SerializeField] private TMP_Text m_gameResultText;
        [SerializeField] private TMP_Text m_reasonText;
        [SerializeField] private Color m_victoryColor = Color.green;
        [SerializeField] private Color m_notVictoryColor = Color.red;

        private const string VICTORY_MESSAGE = "VICTORY :)";
        private const string NOT_VICTORY_MESSAGE = "Ooopps :(";
        
        public void Show(string message, bool isVictory)
        {
            m_gameResultText.color = isVictory ? m_victoryColor : m_notVictoryColor;
            m_reasonText.color = isVictory ? m_victoryColor : m_notVictoryColor;

            m_gameResultText.text = isVictory ? VICTORY_MESSAGE : NOT_VICTORY_MESSAGE;
            m_reasonText.text = message;
        }

        public void Toggle(bool isActive) => gameObject.SetActive(isActive);
    }
}
