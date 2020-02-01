using UnityEngine;

public class Upscale : MonoBehaviour
{
    private static readonly Vector2 UpScale = new Vector2(2f, 2f);
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, UpScale, Vector2.zero);
    }
}
