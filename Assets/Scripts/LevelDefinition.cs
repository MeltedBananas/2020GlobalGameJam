using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LevelQuestion
{
    public Sprite QuestionIcon;
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

[System.Serializable]
public class LevelSolution
{
    public bool IsEnabled = false;
    public string Text;
    public string Label;
    public bool ValidateBrain(List<BrainNode> brainNodes)
    {
        foreach(BrainNode node in brainNodes)
        {
            if(ValidateSolution(node))
            {
                return true;
            }
        }
        return false;
    }
    public bool ValidateSolution(BrainNode brainNode)
    {
        return brainNode.data.Word.Label == Label && brainNode.data.Word.Text == Text && brainNode.BrainNodeEnabled == IsEnabled;
    }
    
}
public class LevelDefinition  : ScriptableObject
{
    public Client Client;
    public TextDiagnostic ClientDescription;
    public string SetupDescription = "This is your assignment - please fix the issue";
    public GameObject ItemPrefab;
    public Sprite SubmitAnswerTexture;
    public Sprite ItemSprite;
    public List<LevelQuestion> Questions;
    public List<LevelBrainNode> BrainNodes;
    public List<LevelProblem> Problems;
    public List<LevelSolution> Solutions;
    public List<BrainToolType> AvailableTools = new List<BrainToolType>() { BrainToolType.SwapStart };


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
                        brainDataList.Add(new BrainData(new Word(braindData.Word)));
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

    public void FuckUp(Brain brain)
    {
        HashSet<string> SwappedList = new HashSet<string>();

        foreach(LevelProblem problem in Problems)
        {
            if (problem.WordSwap)
            {
                BrainNode swapA = brain.GetFromLabel(problem.WordSwapLabel);

                List<Word> wordSwapPossibilities = new List<Word>(problem.WordSwapPossibilities);
                for(int i = wordSwapPossibilities.Count - 1; i >= 0; --i)
                {
                    if(SwappedList.Contains(wordSwapPossibilities[i].Label))
                    {
                        wordSwapPossibilities.RemoveAt(i);
                    }
                }

                if (wordSwapPossibilities.Count >= 0)
                {
                    int rndIdx = UnityEngine.Random.Range(0, wordSwapPossibilities.Count);
                    BrainNode swapB = brain.GetFromLabel(wordSwapPossibilities[rndIdx].Label);

                    if (swapA != null && swapB != null)
                    {
                        swapA.Swap(swapB);
                        SwappedList.Add(swapA.data.Word.Label);
                        SwappedList.Add(swapB.data.Word.Label);
                    }
                }
            }
        }
    }
    
}
