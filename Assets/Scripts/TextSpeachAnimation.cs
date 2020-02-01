using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSpeachAnimation : MonoBehaviour
{
    public float delayBetweenLetters = 0.5f;
    public float letterTimer = 0.0f;
    public TextDiagnostic TextDiagnostic;
    List<WordDisplayText> wordDisplayList = new List<WordDisplayText>();
    public TMP_Text textBox;

    string result = "";
    private int _currentWordIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        letterTimer = 0.5f;
        SetupLine(TextDiagnostic);
    }
    void SetupLine(TextDiagnostic td)
    {
       wordDisplayList.Clear();
       foreach(Word w in TextDiagnostic.wordList)
       {
           wordDisplayList.Add(new WordDisplayText(w));
       }  
    }
    // Update is called once per frame
    void Update()
    {
        AnimateText();
    }
    void  AnimateText()
    {
        
        letterTimer -= Time.deltaTime;
        if (letterTimer <= 0.0f )
        {
            result = "";
            for (int i = 0; i < _currentWordIndex && i < wordDisplayList.Count;++i )
            {
                result += wordDisplayList[i].Animate();
                if (wordDisplayList[i].IsFullyDisplayed())
                {
                    result += " ";
                }
                
            }

            if (_currentWordIndex < wordDisplayList.Count && wordDisplayList[_currentWordIndex].IsFullyDisplayed() )
            {
                ++_currentWordIndex;
                 
            }

            letterTimer = delayBetweenLetters;
        }
        Debug.Log(result);
        textBox.SetText(result);
    }

    
}
