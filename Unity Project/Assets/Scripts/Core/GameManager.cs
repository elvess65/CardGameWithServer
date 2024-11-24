using UnityEngine;
using CardGame.UI;
using CardGame.Data;
using CardGame.Natives;
using CardGame.Abstract;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace CardGame.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIController m_uiController;
        [Tooltip("Whether to run data processing locally bypassing SDK")]
        [SerializeField] private bool m_isLocalMode = false;
        [Tooltip("Whether to bypass server on SDK (local but using SDK")]
        [SerializeField] private bool m_simulateNativeServer;

        private const int DECK_SIZE = 7;
        
        private GameLoop m_gameLoop;
        private List<IUpdatable> m_updatables = new();

        public bool IsLocalMode => m_isLocalMode;
        public bool SimulateNativeServer => m_simulateNativeServer;
        
        private void Start()
        {
            Initialize();
            
            m_uiController.ToMenuUI();
        }

        #region Main Flow
        
        private void StartGame()
        {
            var dataController = ServiceLocator.Get<DataController>();
            var player = dataController.GetPlayer(DECK_SIZE, false);
            var enemy = dataController.GetPlayer(DECK_SIZE, true);

            if (player == null || enemy == null)
            {
                Debug.LogError("[SDK] [CLIENT] Couldn't receive data");
                return;
            }
            
            m_uiController.ToGameUI();
            m_gameLoop.Run(player, enemy);
        }

        private void StopGame()
        {
            m_uiController.ToMenuUI();
            m_gameLoop.Stop();
        }
        
        #endregion

        #region Initialization

        public void SetURL(string newUrl)
        {
            URLBuilder.SetNewUrl(newUrl);
            AndroidWrapper.NativeDataController.UpdateUrl(newUrl);
        }

        private void Initialize()
        {
            AndroidWrapper.Init();
            
            InitializeDataSource();
            
            InitializeUI();
            InitializeGameLoop();
        }

        private void InitializeDataSource()
        {
            ServiceLocator.Register<DataController>(new(m_isLocalMode, m_simulateNativeServer));
        }
        
        private void InitializeGameLoop()
        {
            m_gameLoop = new GameLoop(m_uiController.GameUI);
            m_updatables.Add(m_gameLoop);
        }
        
        private void InitializeUI()
        {
            Assert.IsFalse(m_uiController == null, "[GameManager] UI Controller is not assigned");
            
            m_uiController.Initialize();
            m_uiController.OnStartGame += StartGame;
            m_uiController.OnStopGame += StopGame;
        }
        
        
        #endregion

        #region Update
        
        private void Update()
        {
            ProcessUpdatables();
        }

        private void ProcessUpdatables()
        {
            foreach (var updatable in m_updatables)
            {
                if (!updatable.IsActive)
                    continue;
                
                updatable.Update(Time.deltaTime);
            }
        }
        
        #endregion
    }
}
