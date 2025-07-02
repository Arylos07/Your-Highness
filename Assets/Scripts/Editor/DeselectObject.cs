using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class DeselectObject
{
    static DeselectObject()
    {
        SceneView.duringSceneGui += view =>
        {
            Deselect();
        };
    }

    private static void Deselect()
    {
        Event current = Event.current;
        if (current.type != EventType.KeyDown) return;

        if (current.keyCode == KeyCode.Escape)
        {
            Selection.objects = new Object[0];
        }
    }
}