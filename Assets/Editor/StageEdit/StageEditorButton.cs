using System.Collections;
using System.Collections.Generic;
using StageCreator;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageEditor))]
public class StageEditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var stageEditor = (StageEditor) target;

        if (GUILayout.Button("JsonLoad"))
        {
            stageEditor.LoadJson();
        }
        else if (GUILayout.Button("StageLoad"))
        {
            stageEditor.LoadStage();
        }
        else if (GUILayout.Button("Save"))
        {
            stageEditor.Save();
        }
    }
}
