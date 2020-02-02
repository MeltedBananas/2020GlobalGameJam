using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera _uiCamera = null;
    public KeyCode _quitKeyCode = KeyCode.Escape;
    
    private RaycastHit[] _hits = { };
    private int _uiLayerMask;

    private readonly List<WorldButton> _buttonHits = new List<WorldButton>();
    private int _registerHovering = 0;
    private WorldButton _lastButtonHover = null;

    private void Awake()
    {
        _uiLayerMask = LayerMask.NameToLayer("UI");
        Array.Resize(ref _hits, 50);
    }

    public void RegisterHovering() => RegisterOrUnregisterForHovering(true);
    public void UnregisterHovering() => RegisterOrUnregisterForHovering(false);

    private void RegisterOrUnregisterForHovering(bool addedOrRemove)
    {
        _registerHovering += addedOrRemove ? 1 : -1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_quitKeyCode))
        {
            Application.Quit();
            return;
        }
        
        // Hovering
        if (_registerHovering > 0)
        {
            RaycastHit hit;
            bool hovering = Physics.Raycast(_uiCamera.ScreenPointToRay(Input.mousePosition), out hit, _uiCamera.farClipPlane,
                1 << _uiLayerMask, QueryTriggerInteraction.Collide);

            if (hovering)
            {
                var btn = hit.collider.GetComponent<WorldButton>();
                if (btn != null && btn.HoverEnabled)
                {
                    if (_lastButtonHover != null) _lastButtonHover.IsHovering(false);
                    _lastButtonHover = btn;
                    _lastButtonHover.IsHovering(true);
                }
            }
            else if (_lastButtonHover != null)
            {
                _lastButtonHover.IsHovering(false);
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _buttonHits.Clear();
            
            // On Click - ray cast through all UI elements
            int count = Physics.RaycastNonAlloc(_uiCamera.ScreenPointToRay(Input.mousePosition), _hits,
                _uiCamera.farClipPlane, 1 << _uiLayerMask, QueryTriggerInteraction.Collide);

            for (int i = 0; i < count; ++i)
            {
                _buttonHits.Add(_hits[i].collider.GetComponent<WorldButton>());
            }

            for (int i = 0; i < _buttonHits.Count; ++i)
            {
                // if button consumes the event, break and ignore the rest
                if (_buttonHits[i] != null && _buttonHits[i].Click())
                    break;
            }
        }
    }
}