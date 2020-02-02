using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _bg = null;
    [SerializeField] private float _fadeOutTime = 2f;
    [SerializeField] private float _beforeFadeOut = 1f;
    
    [Header("Logo")]
    [SerializeField] private float _scaleUpTime = 1f;
    [SerializeField] private float _scaleBackTime = 0.6f;
    [SerializeField] private Transform _logo = null;
    [SerializeField] private Image _logoImage = null;
    [SerializeField] private float _fadeInTime = 1f;
    [SerializeField, Range(0f, 1f)] private float _startSequenceAfterPercentage = 0.5f;

    [Header("Texts")]
    [SerializeField] private float _waitBeforeTitle = 0.2f;
    [SerializeField] private float _waitBeforePresents = 0.2f;
    [SerializeField] private TextAnimate _title = null;
    [SerializeField] private TextAnimate _presents = null;
    [SerializeField] private TMP_Text _titleTxt = null;
    [SerializeField] private TMP_Text _presentsTxt = null;

    private float _deltaTime = 0f, _colorDelta = 0f;
    private Vector3 _logoInitialScale = Vector3.one;
    private Vector3 _twiceSize = Vector3.one;
    private Vector3 _halfSize = Vector3.one;
    private bool _hasSequenceStarted = false;
    private static readonly Color QuarterTransparent = new Color(1f, 1f, 1f, 0.25f);
    private static readonly Color Transparent = new Color(1f, 1f, 1f, 0f);
    private bool _isFadingOut = false;

    private void Awake()
    {
        _logoInitialScale = _logo.localScale;
        _halfSize = _logoInitialScale / 2f;
        _twiceSize = _logoInitialScale * 2f;
        _logo.localScale = _halfSize;
        
        _logoImage.color = QuarterTransparent;

        _title.ClearText();
        _presents.ClearText();
    }

    private void Update()
    {
        if (!_isFadingOut)
        {
            _colorDelta += Time.deltaTime;
            float progress = _colorDelta / _fadeInTime;
            _logoImage.color = Color.Lerp(QuarterTransparent, Color.white, progress);

            if (progress > _startSequenceAfterPercentage && !_hasSequenceStarted)
            {
                StartSequences();
                _hasSequenceStarted = true;
            }
        }
        else
        {
            _colorDelta += Time.deltaTime;
            Color c = Color.Lerp(Color.white, Transparent, _colorDelta / _fadeOutTime);
            _bg.color = c;
            _logoImage.color = c;
            _titleTxt.color = c;
            _presentsTxt.color = c;

            if (_colorDelta >= _fadeOutTime)
            {
                gameObject.SetActive(false);
                SceneManager.UnloadSceneAsync("Loading");
            }
        }
    }

    private void StartSequences()
    {
        StartCoroutine(LoadingScreenSequence());
        StartCoroutine(ScaleLogo());
    }

    private IEnumerator ScaleLogo()
    {
        while (_deltaTime <= _scaleUpTime)
        {
            _deltaTime += Time.deltaTime;
            _logo.localScale = Vector3.Lerp(_logoInitialScale, _twiceSize, _deltaTime / _scaleUpTime);
            yield return null;
        }

        _deltaTime = 0f;

        while (_deltaTime <= _scaleBackTime)
        {
            _deltaTime += Time.deltaTime;
            _logo.localScale = Vector3.Lerp(_twiceSize, _logoInitialScale, _deltaTime / _scaleBackTime);
            yield return null;
        }
    }

    private IEnumerator LoadingScreenSequence()
    {
        yield return new WaitForSeconds(_waitBeforeTitle);
        _title.IsAnimated = true;
        yield return new WaitUntil(() => !_title.IsAnimated);

        yield return new WaitForSeconds(_waitBeforePresents);
        _presents.IsAnimated = true;
        yield return new WaitUntil(() => !_presents.IsAnimated);
        
        yield return new WaitForSeconds(_beforeFadeOut);

        // Go To Next
        var asyncOp = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
        asyncOp.completed += FadeOutThisScene;
    }

    private void FadeOutThisScene(AsyncOperation asyncOp)
    {
        // fade out !
        _colorDelta = 0f;
        _isFadingOut = true;
    }
}