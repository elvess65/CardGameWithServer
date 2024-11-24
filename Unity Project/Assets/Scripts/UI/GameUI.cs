using System.Collections.Generic;
using CardGame.Abstract;
using CardGame.Data;
using CardGame.Views;
using UnityEngine;
using UnityEngine.Assertions;

namespace CardGame.UI
{
    public class GameUI : AbstractUI, IInteractable
    {
        public event System.Action<CardData> OnCardPlayed;
        public event System.Action<CardData> OnMoreInfoPressed;
        
        [SerializeField] private RectTransform m_deckRoot;
        [SerializeField] private RectTransform m_healthRoot;
        [SerializeField] private BoardUI m_boardUI;
        [SerializeField] private FinishGameUI m_finishGameUI;
        
        [Header("Prefabs")]
        [SerializeField] private CardView m_cardPrefab;
        [SerializeField] private HealthView m_playerHealthViewPrefab;
        [SerializeField] private HealthView m_enemyHealthViewPrefab;

        private bool m_isInteractable = false;
        private Dictionary<int, CardView> m_cardViews;
        private List<HealthView> m_healthViews;
        
        public bool IsInteractable
        {
            get => m_isInteractable;
            set
            {
                m_isInteractable = value;

                if (m_cardViews == null)
                    return;
                
                foreach (var cardView in m_cardViews.Values)
                {
                    cardView.IsInteractable = value;
                }
            }
        }

        #region Main Logic
        
        public void PopulatePlayer(PlayerData playerData)
        {
            if (!m_deckRoot.gameObject.activeSelf)
                m_deckRoot.gameObject.SetActive(true);
            
            if (m_finishGameUI.gameObject.activeSelf)
                m_finishGameUI.Toggle(false);
            
            PopulatePlayerHealth(playerData.HealthData);
            PopulatePlayerDeck(playerData.Deck);
        }

        public void PopulateEnemy(PlayerData enemyData)
        {
            PopulateEnemyHealth(enemyData.HealthData);
        }

        public void PlayCard(CardData cardData, bool isPlayer)
        {
            TryRemoveCardView(cardData);
            AddCardToBoard(cardData, isPlayer);
        }

        public void FinishGame(string message, bool isVictory)
        {
            m_finishGameUI.Show(message, isVictory);
            m_finishGameUI.Toggle(true);
        }
        
        public void Cleanup()
        {
            //Dispose card views
            foreach (var view in m_cardViews.Values)
            {
                if (view == null)
                    continue;
                
                Destroy(view.gameObject);
            }
            
            m_cardViews.Clear();
            
            //Dispose health views
            foreach (var view in m_healthViews)
            {
                if (view == null)
                    continue;
                
                Destroy(view.gameObject);
            }
            
            m_healthViews.Clear();
            m_finishGameUI.Toggle(false);
            m_deckRoot.gameObject.SetActive(false);
            
            CleanupBoard();
        }

        public void CleanupBoard()
        {
            m_boardUI.Cleanup();
            m_boardUI.HideOutcome();
        }

        public void Interact() { }
        
        #endregion

        #region Populate
        
        private void PopulatePlayerDeck(List<CardData> deck)
        {
            Assert.IsFalse(m_cardPrefab == null, "[GameUI] CardPrefab is not assigned");
            
            m_cardViews = new();

            foreach (var cardData in deck)
            {
                var view = CreateCardView(cardData, m_deckRoot, true);
                view.AsInteractable().WithMoreInfoButton();
                view.OnCardPlayed += CardPlayedHandler;
                view.OnMoreInfoPressed += MoreInfoPressedHandler;
            }
        }

        private void PopulatePlayerHealth(HealthData playerHealthData)
        {
            m_healthViews ??= new();
            
            var view = CreateHealthView(m_playerHealthViewPrefab, playerHealthData);
            m_healthViews.Add(view);
        }

        private void PopulateEnemyHealth(HealthData enemyHealthData)
        {
            m_healthViews ??= new();
            
            var view = CreateHealthView(m_enemyHealthViewPrefab, enemyHealthData);
            m_healthViews.Add(view);
        }
        
        #endregion
        
        #region Views
        
        private CardView CreateCardView(CardData cardData, RectTransform root, bool isPlayer)
        {
            //I would have implemented it as a pool however due to the time restrictions please accept it as it is :)

            var view = Instantiate(m_cardPrefab, root);
            view.Initialize(cardData, isPlayer);
            m_cardViews.Add(cardData.ID, view);
            
            return view;
        }

        private void TryRemoveCardView(CardData cardData)
        {
            if (!TryGetCardViewById(cardData.ID, out var view))
                return;

            m_cardViews.Remove(cardData.ID);
            Destroy(view.gameObject);
        }
        
        private bool TryGetCardViewById(int id, out CardView view) => m_cardViews.TryGetValue(id, out view) && view != null;

        private HealthView CreateHealthView(HealthView prefab, HealthData data)
        {
            //I would have implemented it as a pool however due to the time restrictions please accept it as it is :)

            var view = Instantiate(prefab, m_healthRoot);
            view.Initialize(data);
            
            return view;
        }
        
        #endregion

        #region Board

        public void DisplayOutcome(int damage, bool isDamage) => m_boardUI.ShowOutcome(damage, isDamage);
        
        private void AddCardToBoard(CardData cardData, bool isPlayer)
        {
            var view = CreateCardView(cardData, m_boardUI.BoardRoot, isPlayer);
            m_boardUI.AddView(view);
            
            m_boardUI.Toggle(true);
        }

        #endregion
        
        #region Handlers
        
        private void CardPlayedHandler(CardData data) => OnCardPlayed?.Invoke(data);
        private void MoreInfoPressedHandler(CardData data) => OnMoreInfoPressed?.Invoke(data);

        #endregion

        #region Other Logic

        public override void Toggle(bool state)
        {
            base.Toggle(state);
            
            //Always disable board UI if GameUI is disabling
            if (!state)
                m_boardUI.Toggle(false);
        }

        #endregion
    }
}
