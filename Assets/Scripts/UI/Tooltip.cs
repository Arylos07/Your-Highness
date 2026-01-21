// Instantiates a tooltip while the cursor is over this UI element.
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPrefab;
    [TextArea(1, 30)] public string text = "";

    [Header("DEBUG")]
    [SerializeField] private bool retainTooltip;

    // instantiated tooltip
    GameObject current;
    Canvas canvas;

    void CreateToolTip()
    {
        if (text == string.Empty) return;
        if (current != null) return; //just in case we retained it somewhere.

        // instantiate
        current = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();

        // put to foreground
        current.transform.SetParent(canvas.transform, true); // canvas
        current.transform.SetAsLastSibling(); // last one means foreground
    }

    // helper function to check if the tooltip is currently shown
    // -> useful to only calculate item/skill/etc. tooltips when really needed
    public bool IsVisible() => current != null;

    void ShowToolTip(float delay)
    {
        Invoke(nameof(CreateToolTip), delay);
    }

    void DestroyToolTip()
    {
        if (retainTooltip) return;
        // stop any running attempts to show it
        CancelInvoke(nameof(CreateToolTip));

        // destroy it
        Destroy(current);
    }

    public void OnPointerEnter(PointerEventData d)
    {
        ShowToolTip(0.5f);
    }

    public void OnPointerExit(PointerEventData d)
    {
        DestroyToolTip();
    }

    void Update()
    {
        // always copy text to tooltip. it might change dynamically when
        // swapping items etc., so setting it once is not enough.
        if (current)
        {
            //Gold
            string tip = text;

            current.GetComponentInChildren<Text>().text = tip;

            RectTransform rect = current.GetComponent<RectTransform>();

            Vector3 input = transform.position;
            input.y -= (rect.rect.height * canvas.scaleFactor) / 2;
            if (input.y < 0) input.y += 10;
            else input.y -= 10;
            rect.position = input;
        }
    }

    void OnDisable()
    {
        DestroyToolTip();
    }

    void OnDestroy()
    {
        DestroyToolTip();
    }

    private void OnValidate()
    {
        if (tooltipPrefab == null) tooltipPrefab = Resources.Load("UI/Tooltip") as GameObject;
    }
}