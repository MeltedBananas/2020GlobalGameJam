﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Word 
{
    public string Text;
    public string Label;
    public string Prefix;
    public string Suffix;
    public bool SilentWhenCancelled = false;


    public Word( string txt)
    {
        Text = txt;
        ExtractLabel();
    }

    public Word(Word word)
    {
        Text = word.Text;
        Label = word.Label;
        Prefix = word.Prefix;
        Suffix = word.Suffix;
    }

    public void ExtractLabel()
    {
        if (Text.Contains("[") || Text.Contains("]"))
        {
           Label = Text.Substring(Text.IndexOf('[')+1,Text.IndexOf(']')-1);
           Text = "!*!*undefined*!*!";
        }

    }


   
   public override string ToString()
    {
        return string.Format("{0}: {1}{2}{3}", Label, Prefix, Text, Suffix);
    }

}
