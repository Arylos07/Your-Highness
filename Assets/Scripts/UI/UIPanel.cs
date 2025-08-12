using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public UIState panelState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.OnUIStateChanged += UIState;
        UIState(UIManager.Instance.CurrentState);
    }

    void UIState(UIState newState)
    {
        if (newState == panelState)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
