using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[InitializeOnLoad]
public static class SnapToGround
{
    static SnapToGround()
    {
        SceneView.duringSceneGui += view =>
        {
            Snap();
        };
    }

    private static void Snap()
    {
        Event current = Event.current;
        if (current.type != EventType.KeyDown) return;

        if (current.keyCode == KeyCode.End)
        {
            if (Selection.transforms.Length != 0) Ground();
        }
    }

    public static void Ground()
    {
        foreach (var transform in Selection.transforms)
        {
            // Raycast down from above the object
            var hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 10f, SnapToGroundPreferences.layerMask);
            IEnumerable<RaycastHit> objects = hits.OrderByDescending(x => x.transform.position.y);

            foreach (var hit in objects)
            {
                if (hit.collider.gameObject == transform.gameObject)
                    continue;

                // Find the lowest point of the object in world space
                var meshFilter = transform.GetComponentInChildren<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    var mesh = meshFilter.sharedMesh;
                    var vertices = mesh.vertices;
                    float minY = float.MaxValue;
                    foreach (var v in vertices)
                    {
                        // Transform vertex to world space
                        var worldV = transform.TransformPoint(v);
                        if (worldV.y < minY)
                            minY = worldV.y;
                    }
                    float bottomOffset = transform.position.y - minY;
                    // Move so the bottom aligns with the hit point
                    transform.position = hit.point + Vector3.up * bottomOffset;
                }
                else
                {
                    // Fallback: just snap the pivot to the ground
                    transform.position = hit.point;
                }
                break;
            }
        }
    }
}

[InitializeOnLoad]
public static class SnapToGroundPreferences
{
    private const string key_layermask = "SnapToGround.LayerMask";

    public static LayerMask layerMask = 24369;

    static SnapToGroundPreferences()
    {
        layerMask = EditorPrefs.GetInt(key_layermask, 24369);
    }

    [SettingsProvider]
    public static SettingsProvider CreateSnapToGroundProvider()
    {
        // First parameter is the path in the Settings window.
        // Second parameter is the scope of this setting: it only appears in the Project Settings window.
        var provider = new SettingsProvider("Project/Snap To Ground", SettingsScope.Project)
        {
            // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
            guiHandler = (searchContext) => PreferencesGUI()

            // // Populate the search keywords to enable smart search filtering and label highlighting:
            // keywords = new HashSet<string>(new[] { "Number", "Some String" })
        };

        return provider;
    }

    private static void PreferencesGUI()
    {
        EditorGUILayout.HelpBox("A simple scene utility for Unity that will snap a selected object to an object underneath it. This can be used" +
            " to snap an object to the ground or stack objects on top of others", MessageType.Info);

        GUILayout.Space(20);

        layerMask = EditorGUILayout.MaskField("Culling Mask", layerMask.value, GetLayers(layerMask).ToArray());

        EditorGUILayout.SelectableLabel(layerMask.value.ToString());

        if (GUILayout.Button("Reset Mask"))
        {
            layerMask = 24369;
            GUI.changed = true;
        }

        if (GUI.changed)
        {
            EditorPrefs.SetInt(key_layermask, layerMask.value);
        }
    }

    public static List<string> GetLayers(LayerMask mask)
    {
        List<string> layerNames = new List<string>();
        for (int i = 0; i <= 31; i++) //user defined layers start with layer 8 and unity supports 31 layers
        {
            var layerN = LayerMask.LayerToName(i); //get the name of the layer
            if (layerN.Length > 0) //only add the layer if it has been named (comment this line out if you want every layer)
                layerNames.Add(layerN);
        }

        return layerNames;
    }
}