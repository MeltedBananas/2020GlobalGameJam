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
}