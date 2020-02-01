using UnityEngine;

public class UpscaleDownscale : MonoBehaviour
{
    private static readonly Vector2 DownScale = new Vector2(0.5f, 0.5f);
    private static readonly Vector2 UpScale = new Vector2(2f, 2f);
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //Graphics.Blit(src, dest, DownScale, Vector2.zero);
        //Graphics.Blit(dest, dest, Vector2.one, Vector2.zero);
        Graphics.Blit(src, dest, DownScale, Vector2.zero);
    }
}
