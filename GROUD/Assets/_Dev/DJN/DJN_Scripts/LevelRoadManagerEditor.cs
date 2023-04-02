using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelRoadManager))]
public class LevelRoadManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private LevelRoadManager.RoadStep currentStep;
    
    void SetSelectedStep(LevelRoadManager.RoadStep step)
    {
        currentStep = step;
    }
    
    private void OnSceneGUI()
    {
        LevelRoadManager manager = (LevelRoadManager)target;

        if(target == null) return;

        for (int i = 0; i < manager.steps.Count; i++)
        {
            if (Handles.Button( manager.steps[i].position + (Vector3.up * 2), Quaternion.identity, 0.3f, 0.5f, Handles.SphereHandleCap))
            {
                SetSelectedStep(manager.steps[i]);
            }
        }
        
        Handles.BeginGUI();
        
        Rect toolsArea = new Rect(10, 10, 500, 200);
        EditorGUI.DrawRect(toolsArea, new Color(1,1,1, 0.1f));
        
        GUILayout.BeginArea(toolsArea);
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Step Count");
            manager.stepCount = (int)EditorGUILayout.Slider(manager.stepCount, 1, 100);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("SubStep by Step");
            manager.subStepByStep = (int)EditorGUILayout.Slider(manager.subStepByStep, 1, 50);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Step Distance");
            manager.stepDistance = EditorGUILayout.Slider(manager.stepDistance, 0.1f, 5f);
            GUILayout.EndHorizontal();

            if (currentStep != null)
            {
                currentStep.stepAction =
                    (LevelRoadManager.RoadStep.StepAction)EditorGUILayout.EnumPopup("Step Action",
                        currentStep.stepAction);
            }
        }

        GUILayout.EndArea();

        Handles.EndGUI();
    }
}
