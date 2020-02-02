using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDisplayText 
{
    public int lastLetterfontSize;
    public Word word;
    string result;
    private int letterIndex = 0;
    private BootLoader BootLoader;
    // Start is called before the first frame update
    public WordDisplayText(Word w, int fontSize, BootLoader BootLoader)
    {
        word = w;
        lastLetterfontSize = fontSize;
        this.BootLoader = BootLoader;
    }
    public bool IsFullyDisplayed()
    {
        return letterIndex >= word.Text.Length;
    }
    public string ColorFromPrefix()
    {
        if (word.Prefix.Contains("color"))
        {
            return word.Prefix.Substring(word.Prefix.IndexOf("=") + 1, word.Prefix.IndexOf(">") - word.Prefix.IndexOf("=")-1);
        }
        return "black";
    }
     public string Animate()
    {
        result = "";
        ++letterIndex;
        
        if (letterIndex < word.Text.Length)
        {
            result  = word.Prefix + ShowCharacters() + word.Suffix;
        }
        else
        {
            result  = word.Prefix + word.Text.Substring(0, word.Text.Length ) + word.Suffix;
        }

        return result;

    }
    public string ShowLastLetter()
    {
        return  "<size="+lastLetterfontSize+">" + word.Text.Substring(letterIndex,1)+ "</size>";
    }
    public string ShowCharacters()
    {
        return  "<color="+ColorFromPrefix()+">" + word.Text.Substring(0,letterIndex )+ ShowLastLetter() + "</color>"+
        "<color=white>"+ word.Text.Substring(letterIndex,word.Text.Length-letterIndex) + "</color>";
    }

    public void Refresh()
    {
        if(word.Label.Length > 0)
        {
            BootLoader._brain.RefreshFromLabel(word.Label, ref word);
        }
    }
}
