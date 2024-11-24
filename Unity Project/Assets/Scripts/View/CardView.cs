using TMPro;
using UnityEngine;
using CardGame.Data;
using UnityEngine.UI;
using CardGame.Helpers;
using CardGame.Abstract;

namespace CardGame.Views
{
    public class CardView : MonoBehaviour, IInteractable
    {
        public event System.Action<CardData> OnCardPlayed;
        public event System.Action<CardData> OnMoreInfoPressed;
        
        [Header("Person")]
        [SerializeField] private Image m_body;
        [SerializeField] private Image m_face;
        [SerializeField] private Image m_hair;
        [SerializeField] private Image m_kit;

        [Header("Texts")]
        [SerializeField] private TMP_Text m_cardName;
        [SerializeField] private TMP_Text m_cardPower;
        [SerializeField] private TMP_Text m_cardDetails;
        
        [Header("More/Less Info")]
        [SerializeField] private Button m_moreInfoButton;
        [SerializeField] private Button m_lessInfoButton;
        [SerializeField] private RectTransform m_mainViewRoot;
        [SerializeField] private RectTransform m_detailedViewRoot;
        
        [Header("Interaction")]
        [SerializeField] private Button m_interactionButton;

        [Header("Other")] 
        [SerializeField] private Image m_cardType;
        [SerializeField] private Image m_background;
        [SerializeField] private Color m_playerColor = Color.green;
        [SerializeField] private Color m_enemyColor = Color.red;

        private bool m_isInteractionAllowed = false;
        private bool m_isMoreInfoAllowed = false;
        private CardData m_cardData;
        
        
        public bool IsInteractable { get; set; }
        
        public void Initialize(CardData data, bool isPlayer)
        {
            m_cardData = data;
            ApplyFromCardData(m_cardData);

            m_background.color = isPlayer ? m_playerColor : m_enemyColor;
            
            m_interactionButton.onClick.AddListener(Interact);
            m_moreInfoButton.onClick.AddListener(MoreInfoPressHandler);
            m_lessInfoButton.onClick.AddListener(LessInfoPressHandler);
        }

        public CardView AsInteractable()
        {
            m_isInteractionAllowed = true;
            return this;
        }

        public CardView WithMoreInfoButton()
        {
            m_isMoreInfoAllowed = true;
            m_moreInfoButton.gameObject.SetActive(m_isMoreInfoAllowed);
            
            return this;
        }

        public void Interact()
        {
            if (!m_isInteractionAllowed || !IsInteractable)
                return;
            
            OnCardPlayed?.Invoke(m_cardData);
        }
        
        #region View

        private void ApplyFromCardData(CardData data)
        {  
            LoadAndAssignSprite(m_body, DataHelper.EResourceTypes.Body, data.BodyId);
            LoadAndAssignSprite(m_face, DataHelper.EResourceTypes.Face, data.FaceId);
            LoadAndAssignSprite(m_hair, DataHelper.EResourceTypes.Hair, data.HairId);
            LoadAndAssignSprite(m_kit, DataHelper.EResourceTypes.Kit, data.KitId);

            UpdatePopularityText();
            
            m_cardName.text = data.CardName;
            m_cardPower.text = data.Power.ToString();

            m_moreInfoButton.gameObject.SetActive(m_isMoreInfoAllowed);
            
            //Yes, I know :) 
            //In a production I would have gone addressables way and cache loaded asset somewhere for reusage
            //But for the sake of simplicity due to restricted time for the test task please accept it as it is :) 
            var sprite = Resources.Load<Sprite>(DataHelper.RawCardTypeToEnum(data.Type).ToString());
            if (sprite != null)
                m_cardType.sprite = sprite;
        }
        
        private void ToggleViews(RectTransform toActivate, RectTransform toDeactivate)
        {
            toActivate.gameObject.SetActive(true);
            toDeactivate.gameObject.SetActive(false);
        }

        private void LoadAndAssignSprite(Image imgToAssign, DataHelper.EResourceTypes resourceType, int resourceId)
        {
            //Yes, I know :) 
            //In a production I would have gone addressables way and loaded assets at one go in a batch from bundles
            //But for the sake of simplicity due to restricted time for the test task please accept it as it is :) 

            if (!DataHelper.TryGetResourceName(resourceType, out var resourceName))
            {
                Debug.LogError($"[CardView] Couldn't find resource name for '{resourceType}'");
                return;
            }
            
            var resourceRequest = $"{resourceName}/{resourceName}_{resourceId}";
            var sprite = Resources.Load<Sprite>(resourceRequest);
            if (sprite == null)
            {
                Debug.LogError($"[CardView] Couldn't load resource '{resourceRequest}'");
                return;
            }

            imgToAssign.sprite = sprite;
        }

        private void UpdatePopularityText() => m_cardDetails.text = $"Popularity: {m_cardData.Popularity}"; 
        
        #endregion
        
        #region ButtonHandlers

        private void MoreInfoPressHandler()
        {
            ToggleViews(m_detailedViewRoot, m_mainViewRoot);
            OnMoreInfoPressed?.Invoke(m_cardData);
            
            UpdatePopularityText();
        }

        private void LessInfoPressHandler() => ToggleViews(m_mainViewRoot, m_detailedViewRoot);
        
        #endregion
    }
}
