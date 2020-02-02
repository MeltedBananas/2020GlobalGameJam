using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private Animate _eyes = null;
    [SerializeField] private Animate _mouth = null;

    public LevelDefinition CurrentLevelDefinition;
    public TextSpeachAnimation CurrentSpeachBubble;

    List<int> AnswersIndex = new List<int>();
    List<List<int>> AnswersOrder = new List<List<int>>();

    private void Start()
    {
        if (_eyes != null) _eyes.Play();
    }

    public void Init(LevelDefinition levelDefinition, TextSpeachAnimation speechBubble)
    {
        CurrentLevelDefinition = levelDefinition;
        CurrentSpeachBubble = speechBubble;

        foreach(LevelQuestion question in CurrentLevelDefinition.Questions)
        {
            AnswersOrder.Add(new List<int>());
            AnswersIndex.Add(0);
            for (int i = 0; i < question.Answers.Count; ++i)
            {
                AnswersOrder[AnswersOrder.Count - 1].Add(i);
            }

            // Randomize - lol
            for (int i = 0; i < AnswersOrder[AnswersOrder.Count - 1].Count; ++i)
            {
                int randomIdx = UnityEngine.Random.Range(0, AnswersOrder[AnswersOrder.Count - 1].Count);
                int value = AnswersOrder[AnswersOrder.Count - 1][randomIdx];
                AnswersOrder[AnswersOrder.Count - 1].RemoveAt(randomIdx);
                AnswersOrder[AnswersOrder.Count - 1].Add(value);
            }
        }
    }

    public void Talk()
    {
        
        if (_mouth != null) _mouth.Play();
    }

    public void Shutup()
    {
        if (_mouth != null) _mouth.Stop();
    }

    public void AskQuestion(int questionIndex)
    {
        int answerIdx = AnswersOrder[questionIndex][AnswersIndex[questionIndex]];
        AnswersIndex[questionIndex] = (AnswersIndex[questionIndex] + 1) % AnswersOrder[questionIndex].Count;
        CurrentSpeachBubble.SetupLine(CurrentLevelDefinition.Questions[questionIndex].Answers[answerIdx]);
    }
}
