using TMPro;
using UnityEngine;
using CardGame.Data;

namespace CardGame.UI
{
    public class MenuUI : AbstractUI
    {
        [SerializeField] private TMP_Text m_rulesText;
        [SerializeField] private TMP_InputField m_url;
        [SerializeField] private AssetWrapper m_rulesAssetWrapper;

        public override void Toggle(bool state)
        {
            base.Toggle(state);

            if (m_rulesAssetWrapper != null && m_rulesAssetWrapper.Asset != null)
            {
                m_rulesText.text = m_rulesAssetWrapper.Asset.text;
            }

            m_url.text = URLBuilder.GetUrl();
            m_rulesText.gameObject.SetActive(state);
        }
    }
}
