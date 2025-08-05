using System;
using UnityCommunity.UnitySingleton;
using UnityEngine;

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

    public void SetUIState(UIState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        OnUIStateChanged?.Invoke(newState);
    }

}
