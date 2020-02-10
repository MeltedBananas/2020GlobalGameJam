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
    public Image Icon;
    public Image VerificationIcon;
    public Sprite DefaultButtonSprite;
    public Sprite UnknownVerifyState;
    public Sprite NotValidVerifyState;
    public Sprite ValidVerifyState;
    public List<string> AssociatedLabels = new List<string>();
    public AudioSource ValidAudio;
    public AudioSource InValidAudio;

    private bool bEnabled = false;
    private Vector3 _initialScale = Vector3.one;

    public ValidationState ButtonState;

    bool bInvalidBuzzing = false;
    float InvalidBuzzingRatio = 0.0f;
    public float InvalidBuzzingSpeed = 10.0f;
    public float InvalidBuzzingDistance = 5.0f;

    bool bValidBuzzing = false;
    float ValidBuzzingRatio = 0.0f;
    public float ValidBuzzingSpeed = 0.75f;
    public float ValidBuzzingDistance = 5.0f;

    private bool bInitialized = false;
    private Vector3 InitiLocation = Vector3.zero;


    private void Awake()
    {
        _initialScale = transform.localScale;
        
    }

    public void Initialized(LevelDefinition definition)
    {
        bEnabled = false;
        AssociatedLabels.Clear();

        if (QuestionIndex < definition.Questions.Count)
        {
            bEnabled = true;
            if (definition.Questions[QuestionIndex].QuestionIcon)
            {
                Icon.sprite = definition.Questions[QuestionIndex].QuestionIcon;
                var rt = (RectTransform)transform;
                rt.sizeDelta = new Vector2(Icon.sprite.texture.width, Icon.sprite.texture.height);
            }
            else
            {
                Icon.sprite = DefaultButtonSprite;
                var rt = (RectTransform)transform;
                rt.sizeDelta = new Vector2(256, 128);
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

        bInitialized = true;
        InitiLocation = ((RectTransform)VerificationIcon.gameObject.transform).localPosition;
    }

    public void ScaleUp()
    {
        if (bEnabled)
        {
            gameObject.SetActive(true);
            LeanTween.scale(gameObject, _initialScale, AppearTimeSeconds).setEase(AppearTween);
        }
    }

    private IEnumerator BuzzInvalid(float duration)
    {
        bInvalidBuzzing = true;
        yield return new WaitForSeconds(duration);
        bInvalidBuzzing = false;
    }

    private IEnumerator BuzzValid(float duration)
    {
        bValidBuzzing = true;
        yield return new WaitForSeconds(duration);
        bValidBuzzing = false;
    }

    public void SetValidationState(ValidationState newState)
    {
        ButtonState = newState;
        switch (ButtonState)
        {
            case ValidationState.Unknown:
                VerificationIcon.sprite = UnknownVerifyState;
                break;
            case ValidationState.Invalid:
                VerificationIcon.sprite = NotValidVerifyState;
                InValidAudio.Play();
                StartCoroutine(BuzzInvalid(1.0f));
                break;
            case ValidationState.Valid:
                VerificationIcon.sprite = ValidVerifyState;
                ValidAudio.Play();
                StartCoroutine(BuzzValid(0.325f));
                break;
        }
    }

    private void Update()
    {
        if(!bInitialized)
        {
            return;
        }

        float buzzingInterpTime = 0.25f;
        if (bInvalidBuzzing)
        {
            InvalidBuzzingRatio = InvalidBuzzingRatio + Time.deltaTime / buzzingInterpTime;
        }
        else
        {
            InvalidBuzzingRatio = InvalidBuzzingRatio - Time.deltaTime / buzzingInterpTime;
        }
        InvalidBuzzingRatio = Mathf.Clamp01(InvalidBuzzingRatio);
        Vector3 invalidDelta = new Vector3(InvalidBuzzingRatio * InvalidBuzzingDistance * Mathf.Cos(Time.timeSinceLevelLoad * Mathf.PI * 2.0f * InvalidBuzzingSpeed), 0.0f, 0.0f);

        if (bValidBuzzing)
        {
            ValidBuzzingRatio = ValidBuzzingRatio + Time.deltaTime / buzzingInterpTime;
        }
        else
        {
            ValidBuzzingRatio = ValidBuzzingRatio - Time.deltaTime / buzzingInterpTime;
        }
        ValidBuzzingRatio = Mathf.Clamp01(ValidBuzzingRatio);
        Vector3 validDelta = new Vector3(0.0f, (Mathf.Cos(ValidBuzzingRatio * Mathf.PI + Mathf.PI) + 1.0f) * 0.5f * ValidBuzzingDistance, 0.0f);

        var rt = (RectTransform)VerificationIcon.gameObject.transform;
        rt.localPosition = InitiLocation + invalidDelta + validDelta;
    }
}
