using UnityEngine;

public class Downscale : MonoBehaviour
{
    private static readonly Vector2 DownScale = new Vector2(0.5f, 0.5f);
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, DownScale, Vector2.zero);
    }
}
