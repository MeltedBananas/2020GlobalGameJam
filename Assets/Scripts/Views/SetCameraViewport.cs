using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SetCameraViewport : MonoBehaviour
{
    [SerializeField] public Rect _viewportRect = new Rect(0.395f, 0.135f, 0.59f, 0.62f);
    
    private void Awake()
    {
        GetComponent<Camera>().rect = _viewportRect;
    }
}
