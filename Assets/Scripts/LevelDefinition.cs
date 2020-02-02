using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelQuestion
{
    public Texture2D QuestionIcon;
    public string QuestionLabel;
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
        UnityEditor.AssetDatabase.CreateAsset(asset, UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject) + "/LevelDefinition.asset");
        UnityEditor.AssetDatabase.SaveAssets();

        UnityEditor.EditorUtility.FocusProjectWindow();

        UnityEditor.Selection.activeObject = asset;
    }
#endif

    public List<BrainData> GenerateBrainDataList()
    {
        HashSet<string> labels = new HashSet<string>();
        List<BrainData> brainDataList = new List<BrainData>();

        foreach(LevelQuestion question in Questions)
        {
            foreach(TextDiagnostic answer in question.Answers)
            {
                List<BrainData> answerBrainData = answer.GatherBrainData();

                foreach(BrainData braindData in answerBrainData)
                {
                    if (!labels.Contains(braindData.Word.Label))
                    {
                        labels.Add(braindData.Word.Label);
                        brainDataList.Add(new BrainData(braindData.Word));
                    }
                }
            }
        }

        foreach(LevelBrainNode levelBrainNode in BrainNodes)
        {
            for(int i = 0; i < levelBrainNode.NumberOfNode; ++i)
            {
                Word newWord = new Word("");
                newWord.Label = string.Format("{0}{1}", levelBrainNode.Label, (levelBrainNode.StartingIndex + i));

                if (levelBrainNode.PossibleNames.Count > 0)
                {
                    List<Word> possibleNames = new List<Word>(levelBrainNode.PossibleNames);
                    int randomIdx = UnityEngine.Random.Range(0, levelBrainNode.PossibleNames.Count);
                    newWord.Text = levelBrainNode.PossibleNames[randomIdx].Text;
                    newWord.Prefix = levelBrainNode.PossibleNames[randomIdx].Prefix;
                    newWord.Suffix = levelBrainNode.PossibleNames[randomIdx].Suffix;
                    possibleNames.RemoveAt(randomIdx);
                }
                else
                {
                    newWord.Text = "!Undefined!";
                }

                brainDataList.Add(new BrainData(newWord));
                
            }
        }

        return brainDataList;
    }
}
