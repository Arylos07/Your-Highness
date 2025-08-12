using System;
using TMPro;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.UI;

public enum UIState
{
    Save,
    Grow,
    Dry,
    Craft,
    Sell
}
public class UIManager : MonoSingleton<UIManager>
{
    public UIState CurrentState { get; private set; } = UIState.Save;

    public event Action<UIState> OnUIStateChanged;

    [Header("UI Elements")]
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI DateText;

    public void SetUIState(UIState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        OnUIStateChanged?.Invoke(newState);
    }

    private void Update()
    {
        GoldText.text = GameManager.Instance.Money.ToString("#,##0");
        DateText.text = GameManager.Instance.CurrentDate;
    }

}
