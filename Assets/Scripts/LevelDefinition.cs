using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelQuestion
{
    public Texture2D QuestionIcon;
    List<TextDiagnostic> Answers;
}

public class LevelBrainNode
{
    public string Label;
    public int StartingIndex = 0;
    public int NumberOfNode;
    public List<string> PossibleNames;
}

public class LevelProblem
{
    public bool bWordSwap = false;
    public string WordSwapLabel;
    public List<string> WordSwapPossibilities;
}

public class LevelDefinition
{
    public GameObject Client;
    public string SetupDescription = "This is your assignment - please fix the issue";
    public Texture2D InventoryTexture;
    public List<LevelQuestion> Questions;
    public List<LevelBrainNode> Nodes;
    public List<LevelProblem> Problems;
}
