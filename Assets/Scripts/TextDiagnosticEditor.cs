using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextDiagnostic))]
public class TextDiagnosticEditor : Editor {
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TextDiagnostic diagnostic = (TextDiagnostic)target;
        if(GUILayout.Button("Split sentence"))
        {
            diagnostic.BuildWordList();
        }
    }

}
