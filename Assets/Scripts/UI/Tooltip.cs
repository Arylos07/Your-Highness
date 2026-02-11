// Instantiates a tooltip while the cursor is over this UI element.
// Updated to use a single tooltip instance present on the Canvas (disabled when not used)
// rather than instantiating/destroying at runtime to avoid Canvas Scaler issues.
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("If you have a Tooltip GameObject present on the Canvas, reference its prefab or leave empty. The runtime will try to find a disabled child on the Canvas with the same name.")]
    public GameObject tooltipPrefab;

    [TextArea(1, 30)] public string text = "";

    [Header("DEBUG")]
    [SerializeField] private bool retainTooltip;

    // Shared tooltip instance that lives under the Canvas (should be disabled by default in scene)
    static GameObject s_tooltip;
    static Canvas s_canvas;
    static RectTransform s_tooltipRect;
    static Text s_tooltipText;
    static Tooltip s_owner; // which Tooltip component currently "owns" the visible tooltip

    // instance state
    bool isPointerOver;

    const float DefaultDelay = 0.5f;

    void EnsureCanvas()
    {
        if (s_canvas != null) return;
        var go = GameObject.FindGameObjectWithTag("Canvas");
        if (go != null) s_canvas = go.GetComponent<Canvas>();
        else s_canvas = FindAnyObjectByType<Canvas>();
    }

    void FindOrCreateTooltip()
    {
        if (s_tooltip != null) return;

        EnsureCanvas();
        if (s_canvas == null) return;

        // Try to find a child on the canvas that matches the prefab name (search inactive included)
        string targetName = tooltipPrefab != null ? tooltipPrefab.name : "Tooltip";

        var rects = s_canvas.GetComponentsInChildren<RectTransform>(true);
        foreach (var r in rects)
        {
            if (r.gameObject.name == targetName)
            {
                s_tooltip = r.gameObject;
                break;
            }
        }

        // If not found and a prefab is provided, instantiate it as a child of the canvas (keeps correct scaler)
        if (s_tooltip == null && tooltipPrefab != null)
        {
            s_tooltip = Instantiate(tooltipPrefab, s_canvas.transform, false);
            s_tooltip.name = targetName; // normalize name so further finds work
        }

        if (s_tooltip != null)
        {
            s_tooltipRect = s_tooltip.GetComponent<RectTransform>();
            s_tooltipText = s_tooltip.GetComponentInChildren<Text>(true);
            // ensure tooltip is disabled initially
            s_tooltip.SetActive(false);
        }
    }

    void CreateToolTipImmediate()
    {
        if (string.IsNullOrEmpty(text)) return;
        if (!isPointerOver) return;

        FindOrCreateTooltip();
        if (s_tooltip == null) return;

        // register ownership
        s_owner = this;
        s_tooltip.SetActive(true);
        UpdateTooltipContentsAndPosition();
    }

    // helper function to check if the tooltip is currently shown for this owner
    // -> useful to only calculate item/skill/etc. tooltips when really needed
    public bool IsVisible() => s_tooltip != null && s_tooltip.activeSelf && s_owner == this;

    void ShowToolTip(float delay)
    {
        // schedule showing after delay; Show will verify pointer/ownership when invoked
        CancelInvoke(nameof(CreateToolTipImmediate));
        Invoke(nameof(CreateToolTipImmediate), delay);
    }

    void HideTooltip()
    {
        CancelInvoke(nameof(CreateToolTipImmediate));
        if (s_tooltip == null) return;
        if (retainTooltip)
        {
            // keep tooltip present but hidden
            s_tooltip.SetActive(false);
            if (s_owner == this) s_owner = null;
            return;
        }

        // keep the shared instance but simply hide it
        s_tooltip.SetActive(false);
        if (s_owner == this) s_owner = null;
    }

    public void OnPointerEnter(PointerEventData d)
    {
        isPointerOver = true;
        ShowToolTip(DefaultDelay);
    }

    public void OnPointerExit(PointerEventData d)
    {
        isPointerOver = false;
        // Only hide if this instance owns the tooltip
        if (s_owner == this) HideTooltip();
    }

    void UpdateTooltipContentsAndPosition()
    {
        if (s_tooltip == null || s_tooltipRect == null || s_canvas == null) return;
        if (s_tooltipText != null) s_tooltipText.text = text;

        // Convert this object's world position to canvas local position
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
            s_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : s_canvas.worldCamera,
            transform.position);

        RectTransform canvasRect = s_canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            s_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : s_canvas.worldCamera,
            out localPoint);

        // Place tooltip slightly below the element. Use tooltip rect height for offset if available.
        float yOffset = 10f;
        if (s_tooltipRect != null)
        {
            yOffset += (s_tooltipRect.rect.height * 0.5f);
        }

        s_tooltipRect.anchoredPosition = localPoint + new Vector2(0f, -yOffset);

        // ensure tooltip stays within canvas bounds (basic clamp)
        Vector2 anchored = s_tooltipRect.anchoredPosition;
        Vector2 canvasSize = canvasRect.rect.size;
        Vector2 tooltipSize = s_tooltipRect.rect.size;

        float minX = -canvasSize.x * 0.5f + tooltipSize.x * 0.5f;
        float maxX = canvasSize.x * 0.5f - tooltipSize.x * 0.5f;
        float minY = -canvasSize.y * 0.5f + tooltipSize.y * 0.5f;
        float maxY = canvasSize.y * 0.5f - tooltipSize.y * 0.5f;

        anchored.x = Mathf.Clamp(anchored.x, minX, maxX);
        anchored.y = Mathf.Clamp(anchored.y, minY, maxY);

        s_tooltipRect.anchoredPosition = anchored;
    }

    void Update()
    {
        // If this component owns the tooltip and it's visible, keep it updated every frame
        if (IsVisible())
        {
            UpdateTooltipContentsAndPosition();
        }
    }

    void OnDisable()
    {
        if (s_owner == this) HideTooltip();
    }

    void OnDestroy()
    {
        if (s_owner == this) HideTooltip();
    }

    private void OnValidate()
    {
        if (tooltipPrefab == null) tooltipPrefab = Resources.Load("UI/Tooltip") as GameObject;
    }
}