using System;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _renderer = null;
    [SerializeField] private float _fadeTime = 1f;

    public GameObject AssociatedGameObject;

    private bool _isAnimated = false;
    private bool _isFadingIn = true;
    private float _deltaTime = 0f;
    
    private static readonly Color BlackTransparent = new Color(0f, 0f, 0f, 0f); 

    public void FadeIn()
    {
        if (!_isFadingIn)
        {
            _isAnimated = true;
            _isFadingIn = true;
            _deltaTime = 0f;

            if (AssociatedGameObject != null)
            {
                AssociatedGameObject.SetActive(true);
            }
        }
    }

    public void FadeOut()
    {
        if (_isFadingIn)
        {
            _isAnimated = true;
            _isFadingIn = false;
            _deltaTime = 0f;
        }
    }

    public void SnapIn()
    {
        _isAnimated = false;
        _isFadingIn = true;
        _deltaTime = _fadeTime;

        if(AssociatedGameObject != null)
        {
            AssociatedGameObject.SetActive(true);
        }

        RefreshColor();
    }

    public void SnapOut()
    {
        _isAnimated = false;
        _isFadingIn = false;
        _deltaTime = _fadeTime;

        if (AssociatedGameObject != null)
        {
            AssociatedGameObject.SetActive(false);
        }

        RefreshColor();
    }

    public float GetRatio()
    {
        return Mathf.Lerp(
            _isFadingIn ? 0.0f : 1.0f,
            _isFadingIn ? 1.0f : 0.0f, 
            Mathf.Clamp01(_deltaTime / _fadeTime));
    }

    private void RefreshColor()
    {
        _renderer.color = Color.Lerp(Color.black, BlackTransparent, GetRatio());
    }

    private void Update()
    {
        if (_isAnimated)
        {
            _deltaTime += Time.deltaTime;
            RefreshColor();
            
            if (_deltaTime >= _fadeTime)
            {
                _isAnimated = false;

                if(!_isFadingIn)
                {
                    if (AssociatedGameObject != null)
                    {
                        AssociatedGameObject.SetActive(false);
                    }
                }
            }
        }
    }
}