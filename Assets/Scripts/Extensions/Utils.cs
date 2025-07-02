// This class contains some helper functions.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class Utils
{
    // Mathf.Clamp only works for float and int. we need some more versions:
    public static long Clamp(long value, long min, long max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    // is any of the keys UP?
    public static bool AnyKeyUp(KeyCode[] keys)
    {
        return keys.Any(k => Input.GetKeyUp(k));
    }

    // is any of the keys DOWN?
    public static bool AnyKeyDown(KeyCode[] keys)
    {
        return keys.Any(k => Input.GetKeyDown(k));
    }

    // is any of the keys PRESSED?
    public static bool AnyKeyPressed(KeyCode[] keys)
    {
        return keys.Any(k => Input.GetKey(k));
    }

    // helper function to calculate a bounds radius in WORLD SPACE
    // -> collider.radius is local scale
    // -> collider.bounds is world scale
    // -> use x+y extends average just to be sure (for capsules, x==y extends)
    // -> use 'extends' instead of 'size' because extends are the radius.
    //    in other words: if we come from the right, we only want to stop at
    //    the radius aka half the size, not twice the radius aka size.
    public static float BoundsRadius(Bounds bounds) =>
        (bounds.extents.x + bounds.extents.z) / 2;

    // Distance between two ClosestPoints
    // this is needed in cases where entites are really big. in those cases,
    // we can't just move to entity.transform.position, because it will be
    // unreachable. instead we have to go the closest point on the boundary.
    //
    // Vector3.Distance(a.transform.position, b.transform.position):
    //    _____        _____
    //   |     |      |     |
    //   |  x==|======|==x  |
    //   |_____|      |_____|
    //
    //
    // Utils.ClosestDistance(a.collider, b.collider):
    //    _____        _____
    //   |     |      |     |
    //   |     |x====x|     |
    //   |_____|      |_____|
    //
    public static float ClosestDistance(Collider a, Collider b)
    {
        // return 0 if both intersect or if one is inside another.
        // ClosestPoint distance wouldn't be > 0 in those cases otherwise.
        if (a.bounds.Intersects(b.bounds))
            return 0;

        // Unity offers ClosestPointOnBounds and ClosestPoint.
        // ClosestPoint is more accurate. OnBounds often doesn't get <1 because
        // it uses a point at the top of the player collider, not in the center.
        // (use Debug.DrawLine here to see the difference)
        return Vector3.Distance(a.ClosestPoint(b.transform.position),
                                b.ClosestPoint(a.transform.position));
    }

    // raycast while ignoring self (by setting layer to "Ignore Raycasts" first)
    // => setting layer to IgnoreRaycasts before casting is the easiest way to do it
    // => raycast + !=this check would still cause hit.point to be on player
    // => raycastall is not sorted and child objects might have different layers etc.
    public static bool RaycastWithout(Ray ray, out RaycastHit hit, GameObject ignore, int cullingMask)
    {
        // remember layers
        Dictionary<Transform, int> backups = new Dictionary<Transform, int>();

        // set all to ignore raycast
        foreach (Transform tf in ignore.GetComponentsInChildren<Transform>(true))
        {
            backups[tf] = tf.gameObject.layer;
            tf.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        // raycast
        bool result = Physics.Raycast(ray, out hit, 100, cullingMask);

        // restore layers
        foreach (KeyValuePair<Transform, int> kvp in backups)
            kvp.Key.gameObject.layer = kvp.Value;

        return result;
    }

    // calculate encapsulating bounds of all child renderers
    public static Bounds CalculateBoundsForAllRenderers(GameObject go)
    {
        Bounds bounds = new Bounds();
        bool initialized = false;
        foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
        {
            // initialize or encapsulate
            if (!initialized)
            {
                bounds = rend.bounds;
                initialized = true;
            }
            else bounds.Encapsulate(rend.bounds);
        }
        return bounds;
    }

    // pretty print seconds as hours:minutes:seconds(.milliseconds/100)s
    public static string PrettySeconds(float seconds, bool displayMilliseconds = true)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        string res = "";
        if (t.Days > 0) res += t.Days + "d";
        if (t.Hours > 0) res += " " + t.Hours + "h";
        if (t.Minutes > 0) res += " " + t.Minutes + "m";
        // 0.5s, 1.5s etc. if any milliseconds. 1s, 2s etc. if any seconds
        if (t.Milliseconds > 0 && displayMilliseconds && t.Hours == 0) res += " " + t.Seconds + "." + (t.Milliseconds / 100) + "s";
        else if (t.Seconds > 0 && t.Hours == 0) res += " " + t.Seconds + "s";
        // if the string is still empty because the value was '0', then at least
        // return the seconds instead of returning an empty string
        return res != "" ? res : "0s";
    }

    // hard mouse scrolling that is consistent between all platforms
    //   Input.GetAxis("Mouse ScrollWheel") and
    //   Input.GetAxisRaw("Mouse ScrollWheel")
    //   both return values like 0.01 on standalone and 0.5 on WebGL, which
    //   causes too fast zooming on WebGL etc.
    // normally GetAxisRaw should return -1,0,1, but it doesn't for scrolling
    public static float GetAxisRawScrollUniversal()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll < 0) return -1;
        if (scroll > 0) return 1;
        return 0;
    }

    // two finger pinch detection
    // source: https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
    public static float GetPinch()
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            return touchDeltaMag - prevTouchDeltaMag;
        }
        return 0;
    }

    // universal zoom: mouse scroll if mouse, two finger pinching otherwise
    public static float GetZoomUniversal()
    {
        if (UnityEngine.InputSystem.Mouse.current != null)
            return GetAxisRawScrollUniversal();
        else if (UnityEngine.InputSystem.Touchscreen.current != null)
            return GetPinch();
        return 0;
    }

    // parse last upper cased noun from a string, e.g.
    //   EquipmentWeaponBow => Bow
    //   EquipmentShield => Shield
    public static string ParseLastNoun(string text)
    {
        MatchCollection matches = new Regex(@"([A-Z][a-z]*)").Matches(text);
        return matches.Count > 0 ? matches[matches.Count - 1].Value : "";
    }

    // check if the cursor is over a UI or OnGUI element right now
    // note: for UI, this only works if the UI's CanvasGroup blocks Raycasts
    // note: for OnGUI: hotControl is only set while clicking, not while zooming
    public static bool IsCursorOverUserInterface()
    {
        // IsPointerOverGameObject check for left mouse (default)
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        // IsPointerOverGameObject check for touches
        for (int i = 0; i < UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count; ++i)
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                return true;

        // OnGUI check
        return GUIUtility.hotControl != 0;
    }

    public static bool IsCursorOverObject(GameObject gameObject)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            List<RaycastResult> raycasts = GetEventSystemRaycastResults();
            if (raycasts.Count != 0)
            {
                foreach (RaycastResult result in raycasts)
                {
                    if (result.gameObject == gameObject || result.gameObject.transform.parent.gameObject == gameObject)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    //replace this with RNGesus later
    [Obsolete("Replace with RNGesus functions later.")]
    public static bool RollChance(float chance)
    {
        return UnityEngine.Random.value < chance;
    }
}