using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections;

public class TextSpeachAnimation : MonoBehaviour
{
    public float delayBetweenLetters = 0.5f;
    private float _letterTimer = 0.0f;
    public TextDiagnostic TextDiagnostic;
    List<WordDisplayText> wordDisplayList = new List<WordDisplayText>();
    public TMP_Text textBox;

    string result = "";
    private int _currentWordIndex = 0;
    public Action OnTextComplete = null; 

    private void Awake()
    {
        // Clear text on play
        textBox.SetText("");
    }

    // Start is called before the first frame update
    void Start()
    {
        _letterTimer = 0.5f;
        //SetupLine(TextDiagnostic);
    }
    public void SetupLine(TextDiagnostic td)
    {
       wordDisplayList.Clear();
       foreach(Word w in td.wordList)
       {
           wordDisplayList.Add(new WordDisplayText(w));
       }  
    }
    // Update is called once per frame
    void Update()
    {
        if (_currentWordIndex < wordDisplayList.Count)
            AnimateText();
    }
    
    void  AnimateText()
    {
        _letterTimer -= Time.deltaTime;
        if (_letterTimer <= 0.0f )
        {
            result = "";
            for (int i = 0; i <= _currentWordIndex && i < wordDisplayList.Count;++i )
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

            _letterTimer = delayBetweenLetters;
        }
        
        textBox.SetText(result);

        if (_currentWordIndex == wordDisplayList.Count)
            OnTextComplete?.Invoke();
    }
}
