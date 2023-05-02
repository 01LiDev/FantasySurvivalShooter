using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;

public class AnimationImportSettingsEditor : EditorWindow
{
    [MenuItem("Tools/Change Animation Import Settings")]
    public static void ShowWindow()
    {
        GetWindow<AnimationImportSettingsEditor>("Change Animation Import Settings");
    }

    private string folderPath = "Assets/Animations";
    private bool applyRecursively = true;

    private void OnGUI()
    {
        GUILayout.Label("Animation Import Settings", EditorStyles.boldLabel);

        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
        applyRecursively = EditorGUILayout.Toggle("Apply Recursively", applyRecursively);

        if (GUILayout.Button("Apply Settings"))
        {
            ChangeAnimationImportSettings(folderPath, applyRecursively);
        }
    }

    private void ChangeAnimationImportSettings(string folderPath, bool applyRecursively)
    {
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;

            if (importer != null)
            {
                // Change the root motion settings
                importer.generateRootMotionCurves = false;
                importer.bakeSimulation = true;
                importer.animationType = ModelImporterAnimationType.Generic;

                // Set the root motion node settings
                SerializedObject serializedImporter = new SerializedObject(importer);
                SerializedProperty rootMotionNodeSettings = serializedImporter.FindProperty("m_AnimationRootMotionSettings");
                rootMotionNodeSettings.FindPropertyRelative("m_BakeIntoPose").boolValue = true;
                rootMotionNodeSettings.FindPropertyRelative("m_BasedUpon").enumValueIndex = 0; // 0: Original, 1: Body Orientation, 2: Ground Node
                serializedImporter.ApplyModifiedProperties();

                // Apply the changes
                importer.SaveAndReimport();

                Debug.Log($"Changed animation import settings for: {assetPath}");
            }
        }
    }
}
