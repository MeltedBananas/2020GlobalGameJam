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
        if (word.Prefix != null && word.Prefix.Contains("color"))
        {
            return word.Prefix.Substring(word.Prefix.IndexOf("=") + 1, word.Prefix.IndexOf(">") - word.Prefix.IndexOf("=")-1);
        }
        return "black";
    }
     public string Animate()
    {
        result = "";
        letterIndex = Mathf.Min(letterIndex + 1, word.Text.Length);
        result  = word.Prefix + ShowCharacters() + word.Suffix;

        return result;

    }
    public string ShowLastLetter()
    {
        return  "<size="+lastLetterfontSize+">" + word.Text.Substring(letterIndex,1)+ "</size>";
    }
    public string ShowCharacters()
    {
        string characters;
        if(word.Label.Length > 0)
        {
            characters = "<b><i><u><color=" + ColorFromPrefix() + ">" + word.Text.Substring(0, letterIndex) + "</color></u>" +
                    "<color=white>" + word.Text.Substring(letterIndex, word.Text.Length - letterIndex) + "</color></i></b>";
        }
        else
        {
            characters = "<color=" + ColorFromPrefix() + ">" + word.Text.Substring(0, letterIndex) + "</color>" +
                                "<color=white>" + word.Text.Substring(letterIndex, word.Text.Length - letterIndex) + "</color>";
        }

        return characters;
    }

    public void Refresh()
    {
        if(word.Label.Length > 0)
        {
            BootLoader._brain.Refresh(ref word);
        }
    }
}
