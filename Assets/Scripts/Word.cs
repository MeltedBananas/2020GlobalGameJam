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

   
    public Word( string txt)
    {
        
        Text = txt;
        ExtractLabel();

    }
    public void ExtractLabel()
    {
        if (Text.Contains("[") || Text.Contains("]"))
        {
           Label = Text.Substring(Text.IndexOf('[')+1,Text.IndexOf(']')-1);
           Text = "undefined";
        }

    }


   
   

}
