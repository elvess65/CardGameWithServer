using UnityEngine;
using UnityEngine.UI;
using CardGame.Abstract;
using UnityEngine.Assertions;

public abstract class AbstractUI : MonoBehaviour, IInitializable, IToggleble
{
    public event System.Action OnActionCalled;
        
    [SerializeField] private Button m_actionButton;
        
    public virtual void Initialize()
    {
        Assert.IsFalse(m_actionButton == null, "[AbstractUI] Action Button is not assigned");
        m_actionButton.onClick.AddListener(ActionButtonHandler);
    }

    public virtual void Toggle(bool state)
    {
        gameObject.SetActive(state);
    }

    protected virtual void ActionButtonHandler()
    {
        OnActionCalled?.Invoke();
    }
}
