using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animate : MonoBehaviour
{
    [SerializeField] private Image _image = null;
    [SerializeField] private Texture _spriteSheet = null;
    [SerializeField] private float _secondsPerSheet = 0.5f;

    private Sprite _initialSprite = null;
    private Sprite[] _sprites;
    private bool _isAnimated = false;
    private Coroutine _animatedCoroutine;
    private int _indexSprites = 0;
    
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

    private IEnumerator AnimateSprite()
    {
        while (_isAnimated)
        {
            ++_indexSprites;
            if (_indexSprites >= _sprites.Length)
                _indexSprites = 0;
            _image.sprite = _sprites[_indexSprites];
            yield return new WaitForSeconds(_secondsPerSheet);   
        }
        
        _image.sprite = _initialSprite;
        _indexSprites = 0;
        _animatedCoroutine = null;
    }

    public void Stop()
    {
        _isAnimated = false;
    }
}
