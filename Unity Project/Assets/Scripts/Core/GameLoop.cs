using System;
using System.Threading;
using UnityEngine;
using CardGame.UI;
using CardGame.Data;
using CardGame.Abstract;
using CardGame.Helpers;
using CardGame.Natives;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace CardGame.Core
{
    public class GameLoop : IUpdatable
    {
        private const int BEFORE_PLAY_CARD_DELAY = 1000;
        private const int AFTER_PLAY_CARD_DELAY = 3000;
        private const int BEFORE_BOARD_CLEANUP_DELAY = 3000;
        private const int AFTER_PLAYER_PICK_CARD_DELAY = 100;
        
        public bool IsActive { get; private set; }

        private bool m_isPlayerTurn;
        private GameUI m_gameUI;
        private PlayerData m_playerData;
        private PlayerData m_enemyData;
        private CardData m_playerCard;
        private CardData m_enemyCard;

        private CancellationTokenSource m_cancellationTokenSource;

        public GameLoop(AbstractUI gameUI)
        {
            if (gameUI is not GameUI castedUI)
            {
                Debug.LogError($"[GameLoop] Couldn't cast UI Instance of type '{gameUI.GetType().Name}' to '{nameof(GameUI)}'");
                return;
            }

            m_gameUI = castedUI;
            m_gameUI.OnCardPlayed += CardPlayedHandler;
            m_gameUI.OnMoreInfoPressed += MoreInfoPressedHandler;
        }
        
        public void Update(float deltaTime)
        {
        }

        #region GameLoop
        
        public void Run(PlayerData playerData, PlayerData enemyData)
        {
            m_cancellationTokenSource = new CancellationTokenSource();
            
            m_playerData = playerData;
            m_enemyData = enemyData;
            
            m_gameUI.PopulatePlayer(playerData);
            m_gameUI.PopulateEnemy(enemyData);

            playerData.HealthData.OnPlayerKilled += PlayerKilledHandler;
            enemyData.HealthData.OnPlayerKilled += EnemyKilledHandler;
            
            IsActive = true;
            StartRoundForPlayer();
        }

        public void Stop()
        {
            m_cancellationTokenSource.Cancel();
            m_cancellationTokenSource.Dispose();
            
            IsActive = false;
            m_gameUI.IsInteractable = false;
            
            m_gameUI.Cleanup();
        }

        private void FinishGame(string message, bool isVictory)
        {
            m_playerData.HealthData.OnPlayerKilled -= PlayerKilledHandler;
            m_enemyData.HealthData.OnPlayerKilled -= EnemyKilledHandler;
            
            m_cancellationTokenSource.Cancel();
            m_cancellationTokenSource.Dispose();            
            
            IsActive = false;
            m_gameUI.IsInteractable = false;
            
            m_gameUI.FinishGame(message, isVictory);
        }
        
        private async void PlayCardPlayer(CardData cardData)
        {
            m_gameUI.PlayCard(cardData, true);
            m_playerCard = cardData;
            
            RemoveCardFromDeck(m_playerData, cardData.ID);
            
            if (m_isPlayerTurn)
            {
                DoEnemyMove();
            }
            else
            {
                try
                {
                    if (!m_cancellationTokenSource.IsCancellationRequested)
                        await UniTask.Delay(AFTER_PLAY_CARD_DELAY, cancellationToken: m_cancellationTokenSource.Token);
                }
                catch (Exception)
                { }
                
                CalculateOutcome();
            }
        }
        
        private async void DoEnemyMove()
        {
            if (!m_enemyData.HasCards())
            {
                FinishGame("Enemy has ran out of cards", true);
                return;
            }
            
            ShowEnemyTurnMessage();
            
            //In ideal world where I had more time I would implement it through a separate controller
            //which could be suitable for both AI and Multiplayer controls. But we don't live in that kind of world :)
            
            var rndCardIndex = Random.Range(0, m_enemyData.Deck.Count);
            var rndCard = m_enemyData.Deck[rndCardIndex];
            
            await UniTask.Delay(BEFORE_PLAY_CARD_DELAY, cancellationToken: m_cancellationTokenSource.Token);
            PlayCardEnemy(rndCard);
        }
        
        private async void PlayCardEnemy(CardData cardData)
        {
            m_gameUI.PlayCard(cardData, false);
            m_enemyCard = cardData;
            
            RemoveCardFromDeck(m_enemyData, cardData.ID);
            
            if (m_isPlayerTurn)
            {
                await UniTask.Delay(AFTER_PLAY_CARD_DELAY, cancellationToken: m_cancellationTokenSource.Token);
                CalculateOutcome();
            }
            else
            {
                ShowPlayerTurnMessage();
                
                m_gameUI.IsInteractable = true;
                
                if (!m_playerData.HasCards())
                {
                    FinishGame("Your ran out of cards", false);
                    return;
                }
                
                //Wait until player does its turn
            }
        }
        
        private async void CalculateOutcome()
        {
            ProcessDamageOutcome();
            
            //Not active means somebody was killed
            if (!IsActive)
                return;
            
            await UniTask.Delay(BEFORE_BOARD_CLEANUP_DELAY, cancellationToken: m_cancellationTokenSource.Token);
            m_gameUI.CleanupBoard();

            if (m_isPlayerTurn)
            {
                StartRoundForEnemy();
            }
            else
            {
                StartRoundForPlayer();
            }
        }
        
        #endregion

        #region Rounds

        private void StartRoundForPlayer()
        {
            m_gameUI.IsInteractable = true;
            m_isPlayerTurn = true;

            ShowPlayerTurnMessage();
        }

        private void StartRoundForEnemy()
        {
            m_gameUI.IsInteractable = false;
            m_isPlayerTurn = false;
            
            DoEnemyMove();
        }

        private void ShowPlayerTurnMessage() => AndroidWrapper.NativeToastSender.ShowToast("YOUR TURN");

        private void ShowEnemyTurnMessage() => AndroidWrapper.NativeToastSender.ShowToast("ENEMY'S TURN");
        
        #endregion
        
        #region Damage

        private void ProcessDamageOutcome()
        {
            //It should also we placed in either battle controller or even on the server.
            //Not done due to the lack of time
            
            var playerCardType = DataHelper.RawCardTypeToEnum(m_playerCard.Type);
            var enemyCardType = DataHelper.RawCardTypeToEnum(m_enemyCard.Type);
            
            if (playerCardType == DataHelper.ECardTypes.Attack)
            {
                switch (enemyCardType)
                {
                    //Both placed attack card - both receive damage
                    case DataHelper.ECardTypes.Attack:
                        DealDamage(m_enemyData.HealthData, m_playerCard.Power);
                        DealDamage(m_playerData.HealthData, m_enemyCard.Power);
                        
                        m_gameUI.DisplayOutcome(m_enemyCard.Power, true);
                        
                        break;
                    
                    //Enemy placed defence card when player attack - reduce income damage by the power of defence card
                    case DataHelper.ECardTypes.Defence:
                        int damage = CalculateDamage(m_playerCard.Power, m_enemyCard.Power);
                        DealDamage(m_enemyData.HealthData, damage);
                        
                        m_gameUI.DisplayOutcome(damage, false);
                        
                        break;
                }
            }
            else if (playerCardType == DataHelper.ECardTypes.Defence)
            {
                switch (enemyCardType)
                {
                    //Player placed defence card but enemy placed attack card - reduce income damage by the power of defence card
                    case DataHelper.ECardTypes.Attack:
                        int damage = CalculateDamage(m_enemyCard.Power, m_playerCard.Power);
                        DealDamage(m_enemyData.HealthData, damage);
                        
                        m_gameUI.DisplayOutcome(damage, true);
                        
                        break;
                    
                    //Do nothing if both placed defence card
                    case DataHelper.ECardTypes.Defence:
                        
                        m_gameUI.DisplayOutcome(-1, true);
                        
                        break;
                }
            }
        }
        
        private int CalculateDamage(int attackPower, int defencePower)
        {
            var damage = attackPower - defencePower;
            return Mathf.Clamp(damage, 0, damage);
        }
        
        private void DealDamage(HealthData data, int damage) => data.DecreaseHealth(damage);

        #endregion

        #region Tools

        private void RemoveCardFromDeck(PlayerData data, int cardId)
        {
            for (var i = 0; i < data.Deck.Count; i++)
            {
                if (data.Deck[i].ID == cardId)
                {
                    data.Deck.RemoveAt(i);
                    return;
                }
            }
        }

        #endregion

        #region Event Handlers

        private void PlayerKilledHandler() => FinishGame("Your were killed", false);

        private void EnemyKilledHandler() => FinishGame("Enemy has been killed", true);
        
        private async void CardPlayedHandler(CardData cardData)
        {
            m_gameUI.IsInteractable = false;

            await UniTask.Delay(AFTER_PLAYER_PICK_CARD_DELAY, cancellationToken: m_cancellationTokenSource.Token);
            PlayCardPlayer(cardData);
        }

        private void MoreInfoPressedHandler(CardData cardData)
        {
            cardData.Popularity++;
            
            var dataController = ServiceLocator.Get<DataController>();
            dataController.IncreaseCardPopularity(cardData.DataBaseId);
        }
        
        #endregion
    }
}
