using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class CopyAnimatorParameters
{
    [MenuItem("Tools/Copy Animator Parameters")]
    public static void CopyAllParameters()
    {
        AnimatorController controller = Selection.activeObject as AnimatorController;

        if (controller == null)
        {
            Debug.LogWarning("Please select an Animator Controller to copy parameters from.");
            return;
        }

        string parameters = "";
        foreach (var parameter in controller.parameters)
        {
            parameters += parameter.name + "\n";
        }

        EditorGUIUtility.systemCopyBuffer = parameters;

        Debug.Log("All parameter names have been copied to the clipboard.");
    }
}
