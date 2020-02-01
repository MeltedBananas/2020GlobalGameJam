using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSpeachAnimation : MonoBehaviour
{
    public float delayBetweenLetters = 0.5f;
    public float letterTimer = 0.0f;
    public TextDiagnostic TextDiagnostic;

    public TMP_Text textBox;

    string result = "";
    private int _currentWordIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        letterTimer = 0.5f;
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
            for (int i = 0; i<=_currentWordIndex && i<TextDiagnostic.wordList.Count;++i )
            {
                result += TextDiagnostic.wordList[i].Animate();
                result += " ";
            }

            if (_currentWordIndex < TextDiagnostic.wordList.Count && TextDiagnostic.wordList[_currentWordIndex].IsFullyDisplayed() )
            {
                ++_currentWordIndex; 
            }

            letterTimer = delayBetweenLetters;
        }
        textBox.SetText(result);
    }

    
}
