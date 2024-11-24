using UnityEngine;
using Unity.VisualScripting;

namespace CardGame.UI
{
    public class UIController : MonoBehaviour, IInitializable
    {
        public event System.Action OnStartGame;
        public event System.Action OnStopGame;
        
        [SerializeField] private AbstractUI m_menuUI;
        [SerializeField] private AbstractUI m_gameUI;

        public AbstractUI GameUI => m_gameUI;
        
        public void Initialize()
        {
            m_menuUI.Initialize();
            m_menuUI.OnActionCalled += StartGameHandler;
            
            m_gameUI.Initialize();
            m_gameUI.OnActionCalled += StopGameHandler;
        }

        public void ToMenuUI()
        {
            m_menuUI.Toggle(true);
            m_gameUI.Toggle(false);
        }

        public void ToGameUI()
        {
            m_menuUI.Toggle(false);
            m_gameUI.Toggle(true);
        }

        private void StartGameHandler() => OnStartGame?.Invoke();

        private void StopGameHandler() => OnStopGame?.Invoke();
    }
}
