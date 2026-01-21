// Attach to the prefab for easier component access by the UI Scripts.
// Otherwise we would need slot.GetChild(0).GetComponentInChildren<Text> etc.
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    //public UIShowToolTip tooltip;
    public Button button;
    public TextMeshProUGUI text;
    public Image image;
    public GameObject amountOverlay;
    public TextMeshProUGUI amountText;
    public Tooltip tooltip;
}
