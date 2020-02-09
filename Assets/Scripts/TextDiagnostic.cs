using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class TextDiagnostic : ScriptableObject
{
    public string Phrase = "";
    public string[] sperator = { " ", ",","!","?","."};

    private string[] _stringSplit;
    public List<Word> wordList;
    
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/JAM2020/TextDiagnostic", false, int.MinValue)]
    public static void CreateAsset()
    {
        var asset = ScriptableObject.CreateInstance<TextDiagnostic>();
        UnityEditor.AssetDatabase.CreateAsset(asset, UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject) + "/TextDiagnostic.asset");
        UnityEditor.AssetDatabase.SaveAssets();

        UnityEditor.EditorUtility.FocusProjectWindow();

        UnityEditor.Selection.activeObject = asset;
    }
#endif

    public void BuildWordList()
    {
        
        _stringSplit = Phrase.Split(sperator,  
           StringSplitOptions.RemoveEmptyEntries); 

        wordList.Clear();

        foreach(String s in _stringSplit) 
        { 
            wordList.Add(new Word(s)); 
        } 
    }

    public List<BrainData> GatherBrainData()
    {
        HashSet<string> labels = new HashSet<string>();
        List<BrainData> brainDataList = new List<BrainData>();

        foreach (Word word in wordList)
        {
            if(word.Label.Length > 0)
            {
                if(!labels.Contains(word.Label))
                {
                    labels.Add(word.Label);
                    brainDataList.Add(new BrainData(word));
                }
            }
        }

        return brainDataList;
    }

    public List<string> GetAllLabels()
    {
        List<string> labels = new List<string>();

        foreach (Word word in wordList)
        {
            if (word.Label.Length > 0)
            {
                if(!labels.Contains(word.Label))
                {
                    labels.Add(word.Label);
                }
            }
        }

        return labels;
    }
}