using System;
using UnityEngine;
using UnityEngine.Events;

public class OnClickOutside : MonoBehaviour
{
    public UnityEvent OnOutsideHit = null;
    
    private RaycastHit[] _hits = { };
    private int _uiLayerMask;
    private Camera _uiCamera = null;

    private void Awake()
    {
        _uiLayerMask = LayerMask.NameToLayer("UI");
        Array.Resize(ref _hits, 50);
    }

    private void Update()
    {
        if (OnOutsideHit == null)
            return;
        
        if (_uiCamera == null)
        {
            _uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        }

        if (_uiCamera == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            int count = Physics.RaycastNonAlloc(_uiCamera.ScreenPointToRay(Input.mousePosition), _hits,
                _uiCamera.farClipPlane, 1 << _uiLayerMask, QueryTriggerInteraction.Collide);

            bool hasHit = false;
            for (int i = 0; i < count && !hasHit; ++i)
            {
                hasHit = _hits[i].collider.GetComponent<OnClickOutside>() == this;
            }

            if (!hasHit)
            {
                OnOutsideHit?.Invoke();
            }
        }
    }
}
