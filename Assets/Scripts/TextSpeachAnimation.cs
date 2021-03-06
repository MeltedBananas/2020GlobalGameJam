﻿using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections;

public class TextSpeachAnimation : MonoBehaviour
{
    public int lastLetterfontSize = 50;
    public float delayBetweenLetters = 0.5f;
    public float fastDelayBetweenLetters = 0.05f;
    private float _letterTimer = 0.0f;
    public TextDiagnostic TextDiagnostic;
    List<WordDisplayText> wordDisplayList = new List<WordDisplayText>();
    public TMP_Text textBox;

    string result = "";
    private int _currentWordIndex = 0;
    public Action OnTextComplete = null;

    public BootLoader BootLoader;

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

    public void ClearLine()
    {
        _currentWordIndex = 0;
        wordDisplayList.Clear();
        textBox.SetText("");
        result = "";
    }

    public void SetupLine(TextDiagnostic td)
    {
       ClearLine();
       foreach(Word w in td.wordList)
       {
           wordDisplayList.Add(new WordDisplayText(w,lastLetterfontSize, BootLoader));
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
                if (wordDisplayList[i].IsFullyDisplayed() && wordDisplayList[i].word.Text.Length > 0)
                {
                    result += " ";
                }
            }

            if (_currentWordIndex < wordDisplayList.Count && wordDisplayList[_currentWordIndex].IsFullyDisplayed() )
            {
                ++_currentWordIndex;

                bool bDone = false;
                if (_currentWordIndex < wordDisplayList.Count && !bDone)
                {
                    wordDisplayList[_currentWordIndex].Refresh();
                    if(wordDisplayList[_currentWordIndex].word.Text.Length == 0)
                    {
                        // Disabled
                        ++_currentWordIndex;
                    }
                    else
                    {
                        bDone = true;
                    }
                }
            }

            _letterTimer += Input.GetButton("FastTalk") ? fastDelayBetweenLetters : delayBetweenLetters;
        }
        
        textBox.SetText(result);

        if (_currentWordIndex == wordDisplayList.Count)
            OnTextComplete?.Invoke();
    }
}
