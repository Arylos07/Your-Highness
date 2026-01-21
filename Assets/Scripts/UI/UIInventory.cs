// Note: this script has to be on an always-active UI parent, so that we can
// always react to the hotkey.
using UnityEngine;
using UnityEngine.UI;

public partial class UIInventory : MonoBehaviour
{
    public static UIInventory instance;
    GameManager gameManager;

    //public KeyCode hotKey = KeyCode.I;
    //public GameObject panel;
    public UIInventorySlot slotPrefab;
    public Transform content;
    //public Text goldText;

    private void Start()
    {
        instance = this;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        // instantiate/destroy enough slots
        UIUtils.BalancePrefabs(slotPrefab.gameObject, gameManager.FlowerInventory.Count, content);

        // refresh all items
        for (int i = 0; i < gameManager.FlowerInventory.Count; ++i)
        {
            UIInventorySlot slot = content.GetChild(i).GetComponent<UIInventorySlot>();
            //slot.dragAndDropable.name = i.ToString(); // drag and drop index
            FlowerSlot itemSlot = gameManager.FlowerInventory[i];

            if (itemSlot.amount > 0)
            {
                // refresh valid item
                int icopy = i; // needed for lambdas, otherwise i is Count
                /*
                slot.button.onClick.SetListener(() => {
                    if (itemSlot.item.data is UsableItem &&
                        ((UsableItem)itemSlot.item.data).CanUse(player, icopy))
                        player.CmdUseInventoryItem(icopy);
                });
                */
                //slot.tooltip.enabled = true;
                //slot.tooltip.text = itemSlot.ToolTip();
                //slot.dragAndDropable.dragable = true;
                //slot.image.color = Color.white;
                //slot.image.sprite = itemSlot.item.image;
                slot.text.text = itemSlot.flower.Name;
                slot.amountOverlay.SetActive(itemSlot.amount > 1);
                slot.amountText.text = itemSlot.amount.ToString();
                slot.tooltip.text = itemSlot.ToolTip();
                slot.tooltip.enabled = true;
            }
            else
            {
                // refresh invalid item
                //slot.button.onClick.RemoveAllListeners();
                //slot.tooltip.enabled = false;
                //slot.dragAndDropable.dragable = false;
                //slot.image.color = Color.clear;
                slot.image.sprite = null;
                slot.text.text = string.Empty;
                slot.amountOverlay.SetActive(false);
                slot.tooltip.enabled = false;
            }
        }
    }
}
