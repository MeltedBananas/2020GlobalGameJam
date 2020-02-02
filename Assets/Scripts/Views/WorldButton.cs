using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private bool _disableOnClick = true;
    [SerializeField] private UnityEvent _action = null;

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
}
