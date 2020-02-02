using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class QuestionButton : MonoBehaviour
{
    public int QuestionIndex = 0;
    public float AppearTimeSeconds = 0.25f;
    public LeanTweenType AppearTween = LeanTweenType.easeInOutBack;
    public TMP_Text TextMeshPro;
    public Image image;
    
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
        image = GetComponent<Image>();
        bEnabled = false;
        Label = string.Empty;

        if(QuestionIndex < definition.Questions.Count)
        {
            bEnabled = true;
            if (definition.Questions[QuestionIndex].QuestionIcon)
            {
                image.sprite = definition.Questions[QuestionIndex].QuestionIcon;
                TextMeshPro.gameObject.SetActive(false);
            }
            else
            {
                Label = definition.Questions[QuestionIndex].QuestionLabel;
                TextMeshPro.SetText(Label);
                TextMeshPro.gameObject.SetActive(true);
            }
            
        }


        if (QuestionIndex == definition.Questions.Count)
        {
            bEnabled = true;
            bIsSubmit = true;
            Label = "Submit Answer";
            if (definition.SubmitAnswerTexture)
            {
                image.sprite = definition.SubmitAnswerTexture;
                TextMeshPro.gameObject.SetActive(false);
            }
            else
            {
                TextMeshPro.SetText(Label);
                TextMeshPro.gameObject.SetActive(true);
            }
            
        }

		WorldButton button = GetComponent<WorldButton>();
        if(button != null)
        {
            button.enabled = bEnabled;
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
