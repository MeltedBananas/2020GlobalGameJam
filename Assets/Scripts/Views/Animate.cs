using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animate : MonoBehaviour
{
    [SerializeField] private Image _image = null;
    [SerializeField] private Texture _spriteSheet = null;
    [SerializeField] private float _secondsPerSheet = 0.15f;
    [SerializeField] private bool _usesRandomTimer = false;
    [SerializeField] private Vector2 _randomBetweenSprite = new Vector2(0.025f, 0.05f);

    private Sprite _initialSprite = null;
    private Sprite[] _sprites;
    private bool _isAnimated = false;
    private Coroutine _animatedCoroutine;
    private int _indexSprites = 0;
    private float _speedMultiplier = 1f;
    
    private void Awake()
    {
        _initialSprite = _image.sprite;
        _sprites = Resources.LoadAll<Sprite>(_spriteSheet.name);
    }

    public void Play()
    {
        _isAnimated = true;

        if (_animatedCoroutine == null)
            _animatedCoroutine = StartCoroutine(AnimateSprite());
    }

    /// <summary>
    /// To reset, set <see cref="multiplier"/> as 1
    /// </summary>
    public void SpeedUp(float multiplier)
    {
        _speedMultiplier = multiplier;
    }
    
    public void Stop()
    {
        _isAnimated = false;
    }

    private IEnumerator AnimateSprite()
    {
        while (_isAnimated)
        {
            ++_indexSprites;
            if (_indexSprites >= _sprites.Length)
                _indexSprites = 0;
            _image.sprite = _sprites[_indexSprites];

            var timeBetween = _secondsPerSheet + (_usesRandomTimer
                ? UnityEngine.Random.Range(_randomBetweenSprite.x, _randomBetweenSprite.y)
                : 0f);
            yield return new WaitForSeconds(timeBetween / _speedMultiplier);   
        }
        
        _image.sprite = _initialSprite;
        _indexSprites = 0;
        _animatedCoroutine = null;
    }
}
