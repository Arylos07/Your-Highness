using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlowerTemplate))]
public class FlowerTemplateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get the target object
        FlowerTemplate flowerTemplate = (FlowerTemplate)target;

        // Add a header indicating the fields are automatically generated
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Note: The Description and Overview fields below are automatically generated.", EditorStyles.helpBox);
        EditorGUILayout.Space();

        // Display the Description property as a read-only text box with word wrap
        EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
        EditorGUILayout.SelectableLabel(flowerTemplate.Description, EditorStyles.textArea, GUILayout.Height(80));

        // Display the Overview property as a read-only text box with word wrap
        EditorGUILayout.LabelField("Overview", EditorStyles.boldLabel);
        EditorGUILayout.SelectableLabel(flowerTemplate.Overview, EditorStyles.textArea, GUILayout.Height(80));
    }
}