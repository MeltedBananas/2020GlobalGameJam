using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Word 
{
    public string Text;
    public string Label;
    public string Prefix;
    public string Suffix;

    private int letterIndex = 0;
    public Word( string txt)
    {
        Text = txt;
    }
    public bool IsFullyDisplayed()
    {
        return letterIndex >= Text.Length;
    }
    public string Animate()
    {

        string result = Prefix + Text.Substring(0,(++letterIndex < Text.Length)?letterIndex :Text.Length) + Suffix;
        Debug.Log("Animate word :"+ result);
        return result;

    }

}
