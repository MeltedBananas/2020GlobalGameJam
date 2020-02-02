using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelQuestion
{
    public Texture2D QuestionIcon;
    public List<TextDiagnostic> Answers;
}

[System.Serializable]
public class LevelBrainNode
{
    public string Label;
    public int StartingIndex = 0;
    public int NumberOfNode;
    public List<Word> PossibleNames;
}

[System.Serializable]
public class LevelProblem
{
    public bool WordSwap = false;
    public string WordSwapLabel;
    public List<Word> WordSwapPossibilities;
}

public class LevelDefinition  : ScriptableObject
{
    public Client Client;
    public TextDiagnostic ClientDescription;
    public string SetupDescription = "This is your assignment - please fix the issue";
    public Texture2D InventoryTexture;
    public List<LevelQuestion> Questions;
    public List<LevelBrainNode> BrainNodes;
    public List<LevelProblem> Problems;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/JAM2020/LevelDefinition", false, int.MinValue)]
    public static void CreateAsset()
    {
        var asset = ScriptableObject.CreateInstance<LevelDefinition>();
        UnityEditor.AssetDatabase.CreateAsset(asset, "Assets/Levels/LevelDefinition.asset");
        UnityEditor.AssetDatabase.SaveAssets();

        UnityEditor.EditorUtility.FocusProjectWindow();

        UnityEditor.Selection.activeObject = asset;
    }
#endif
}
