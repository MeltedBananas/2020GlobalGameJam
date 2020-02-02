using System;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer = null;
    [SerializeField] private float _fadeTime = 1f;

    private bool _isAnimated = false;
    private bool _isFadingIn = true;
    private float _deltaTime = 0f;
    
    private static readonly Color BlackTransparent = new Color(0f, 0f, 0f, 0f); 

    public void FadeOut()
    {
        _isAnimated = true;
        _isFadingIn = false;
        _deltaTime = 0f;
    }

    public void FadeIn()
    {
        _isAnimated = true;
        _isFadingIn = true;
        _deltaTime = 0f;
    }

    private void Update()
    {
        if (_isAnimated)
        {
            _deltaTime += Time.deltaTime;
            _renderer.color = Color.Lerp(_isFadingIn ? BlackTransparent : Color.black,
                _isFadingIn ? Color.black : BlackTransparent, _deltaTime / _fadeTime);
            
            if (_deltaTime >= _fadeTime)
            {
                _isAnimated = false;
            }
        }
    }
}