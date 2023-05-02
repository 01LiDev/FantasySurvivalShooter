using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class UncheckExitTime
{
    [MenuItem("Tools/Uncheck Exit Time for All Transitions")]
    public static void UncheckAllExitTimes()
    {
        AnimatorController controller = Selection.activeObject as AnimatorController;

        if (controller == null)
        {
            Debug.LogWarning("Please select an Animator Controller to modify.");
            return;
        }

        foreach (var layer in controller.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                foreach (var transition in state.state.transitions)
                {
                    transition.hasExitTime = false;
                }
            }
        }

        Debug.Log("All exit times have been unchecked for the selected Animator Controller.");
    }
}
