using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionButton : MonoBehaviour
{
    public int QuestionIndex = 0;
    public float AppearTimeSeconds = 0.25f;
    public LeanTweenType AppearTween = LeanTweenType.easeInOutBack;
    public TMP_Text TextMeshPro;
    private bool bIsSubmit = false;
    private bool bEnabled = false;
    private string Label;
    private Vector3 _initialScale = Vector3.one;

    private void Awake()
    {
        _initialScale = transform.localScale;
    }

    public void Initialized(LevelDefinition definition)
    {
        bEnabled = false;
        Label = string.Empty;

        if(QuestionIndex < definition.Questions.Count)
        {
            bEnabled = true;
            Label = definition.Questions[QuestionIndex].QuestionLabel;
        }

        if (QuestionIndex == definition.Questions.Count)
        {
            bEnabled = true;
            bIsSubmit = true;
            Label = "Submit Answer";
        }

        TextMeshPro.SetText(Label);

        gameObject.SetActive(false);
        transform.localScale = Vector3.one * Mathf.Epsilon;
    }

    public void ScaleUp()
    {
        if (bEnabled)
        {
            gameObject.SetActive(true);
            LeanTween.scale(gameObject, _initialScale, AppearTimeSeconds).setEase(AppearTween).setOnComplete(() => TextMeshPro.SetAllDirty());
        }
    }
}
