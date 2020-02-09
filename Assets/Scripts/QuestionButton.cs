using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class QuestionButton : MonoBehaviour
{
    public enum ValidationState
    {
        Unknown,
        Invalid,
        Valid
    };

    public int QuestionIndex = 0;
    public float AppearTimeSeconds = 0.25f;
    public LeanTweenType AppearTween = LeanTweenType.easeInOutBack;
    public TMP_Text TextMeshPro;
    public Image image;
    public Image VerificationIcon;
    public Sprite DefaultButtonSprite;
    public Sprite UnknownVerifyState;
    public Sprite NotValidVerifyState;
    public Sprite ValidVerifyState;
    public List<string> AssociatedLabels = new List<string>();

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
        AssociatedLabels.Clear();

        if (QuestionIndex < definition.Questions.Count)
        {
            bEnabled = true;
            if (definition.Questions[QuestionIndex].QuestionIcon)
            {
                image.sprite = definition.Questions[QuestionIndex].QuestionIcon;
                var rt = (RectTransform)transform;
                rt.sizeDelta = new Vector2(image.sprite.texture.width, image.sprite.texture.height);
                TextMeshPro.gameObject.SetActive(false);
            }
            else
            {
                image.sprite = DefaultButtonSprite;
                Label = definition.Questions[QuestionIndex].QuestionLabel;
                var rt = (RectTransform)transform;
                rt.sizeDelta = new Vector2(256, 128);
                TextMeshPro.SetText(Label);
                TextMeshPro.gameObject.SetActive(true);
            }
            
            foreach(TextDiagnostic answer in definition.Questions[QuestionIndex].Answers)
            {
                List<string> labels = answer.GetAllLabels();
                foreach(string label in labels)
                {
                   if(!AssociatedLabels.Contains(label))
                    {
                        AssociatedLabels.Add(label);
                    }
                }
            }
        }

		WorldButton button = GetComponent<WorldButton>();
        if(button != null)
        {
            button.enabled = bEnabled;
        }

        SetValidationState(ValidationState.Unknown);

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

    public void SetValidationState(ValidationState newState)
    {
        switch(newState)
        {
            case ValidationState.Unknown:
                VerificationIcon.sprite = UnknownVerifyState;
                break;
            case ValidationState.Invalid:
                VerificationIcon.sprite = NotValidVerifyState;
                break;
            case ValidationState.Valid:
                VerificationIcon.sprite = ValidVerifyState;
                break;
        }
    }
}
