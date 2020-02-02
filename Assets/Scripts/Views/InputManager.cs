using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera _uiCamera = null;
    private RaycastHit[] _hits = { };
    private int _uiLayerMask;

    private readonly List<WorldButton> _buttonHits = new List<WorldButton>();

    private void Awake()
    {
        _uiLayerMask = LayerMask.NameToLayer("UI");
        Array.Resize(ref _hits, 50);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // On Click - ray cast through all UI elements
            int count = Physics.RaycastNonAlloc(_uiCamera.ScreenPointToRay(Input.mousePosition), _hits,
                _uiCamera.farClipPlane, 1 << _uiLayerMask, QueryTriggerInteraction.Collide);

            if (count > 0)
            {
                _buttonHits.Clear();
            }

            for (int i = 0; i < count; ++i)
            {
                _buttonHits.Add(_hits[i].collider.GetComponent<WorldButton>());
            }

            for (int i = 0; i < _buttonHits.Count; ++i)
            {
                // if button consumes the event, break and ignore the rest
                if (_buttonHits[i].Click())
                    break;
            }
        }
    }
}