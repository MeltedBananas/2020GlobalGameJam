using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private bool _disableOnClick = true;
    [SerializeField] private UnityEvent _action = null;

    [SerializeField] private bool _enableHover = false;
    [SerializeField] private Color _hoverColor = Color.gray;
    [SerializeField] private TMP_Text _buttonText = null;
    [SerializeField] private InputManager _inputManager = null;

    private Color _textInitialColor = Color.black;
    private int _uiLayerMask;

    public bool HoverEnabled => _enableHover;
    
    private void Awake()
    {
        _uiLayerMask = LayerMask.NameToLayer("UI");
        
        if (_enableHover)
        {
            _textInitialColor = _buttonText.color;
        }
    }

    private void OnEnable()
    {
        if (_enableHover)
            _inputManager.RegisterHovering();
    }

    private void OnDisable()
    {
        if (_enableHover)
            _inputManager.UnregisterHovering();
    }

    /// <summary>
    /// Clicks the button
    /// </summary>
    /// <returns>if the Action was consumed</returns>
    public bool Click()
    {
        if (enabled && _action != null)
        {
            if (_disableOnClick)
                enabled = false;
            
            _action.Invoke();
            
            return true;
        }

        return false;
    }

    public void IsHovering(bool isHovering)
    {
        if (_enableHover)
        {
            _buttonText.color = isHovering ? _hoverColor : _textInitialColor;
        }
    }
}
