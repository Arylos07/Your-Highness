using System;
using UnityEngine;

public enum UIState
{
    Save,
    Grow,
    Dry,
    Craft,
    Sell
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public UIState CurrentState { get; private set; } = UIState.Save;

    public event Action<UIState> OnUIStateChanged;

    public void SetUIState(UIState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        OnUIStateChanged?.Invoke(newState);
    }

}
