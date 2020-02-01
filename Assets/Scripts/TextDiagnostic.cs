using UnityEngine;

public class TextDiagnostic : ScriptableObject
{
    public string Phrase = "";
    
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/JAM2020/TextDiagnostic", false, int.MinValue)]
    public static void CreateAsset()
    {
        var asset = ScriptableObject.CreateInstance<TextDiagnostic>();
        UnityEditor.AssetDatabase.CreateAsset(asset, "Assets/GameObject/TextDiagnostic.asset");
        UnityEditor.AssetDatabase.SaveAssets();

        UnityEditor.EditorUtility.FocusProjectWindow();

        UnityEditor.Selection.activeObject = asset;
    }
#endif
}