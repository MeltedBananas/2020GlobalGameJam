using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDisplayText 
{

    Word word;
    string result;
    private int letterIndex = 0;
    // Start is called before the first frame update
    public WordDisplayText(Word w)
    {
        word = w;
    }
    public bool IsFullyDisplayed()
    {
        return letterIndex >= word.Text.Length;
    }

     public string Animate()
    {
        result = "";
        if (letterIndex < word.Text.Length)
        {
            result  = word.Prefix + word.Text.Substring(0,++letterIndex ) + word.Suffix;
        }
        else
        {
            result  = word.Prefix + word.Text.Substring(0, word.Text.Length ) + word.Suffix;
        }

        return result;

    }
}
