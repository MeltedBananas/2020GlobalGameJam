using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextAnimate : MonoBehaviour
{
    [SerializeField] private string _textShown = "";
    [SerializeField] private float _appearDelay = 0.15f;

    private TMP_Text _text = null;
    private float _deltaTime = 0f;
    private int _charIndex = 0;
    private string _textDisplayed = "";
    
    public bool IsAnimated { get; set; }

    private void Awake()
    {
        _deltaTime = _appearDelay;

        _text = GetComponent<TMP_Text>();
        ClearText();
    }

    public void ClearText()
    {
        _text.text = "";
        _textDisplayed = "";
    }
    
    private void Update()
    {
        if (IsAnimated)
        {
            _deltaTime += Time.deltaTime;

            if (_deltaTime >= _appearDelay)
            {
                _textDisplayed += _textShown[_charIndex++];
                _text.text = _textDisplayed;
                _deltaTime = 0f;
            }

            if (_charIndex >= _textShown.Length)
            {
                IsAnimated = false;
                _charIndex = 0;
                _deltaTime = 0f;
                _textDisplayed = "";
            }
        }
    }
}