using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BrainNode : MonoBehaviour
{
    public Color NormalColor;
    public Color MouseOverColor;

    public string Label;
    public Vector2 Size = new Vector2(32.0f, 32.0f);

    public Camera MainCamera;
    private SpriteRenderer mySpriteRenderer;

    public BrainData data = null;
    bool bMouseOver = false;

    public Brain BrainOwner;
    public bool BrainNodeEnabled = true;

    public Billboard Billboard;
    public SpriteRenderer IconRenderer;
    public SpriteRenderer SwapIconRenderer;
    public SpriteRenderer CancelIconRenderer;

    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = NormalColor;
        IconRenderer.color = NormalColor;

        SetReadyToSwap(false);
        SetEnabled(BrainNodeEnabled);
    }

    public void Init()
    {
        Billboard.MainCamera = MainCamera;
    }

    void OnMouseDown()
    {
        BrainOwner.OnNodeClick(this);
    }

    void OnMouseEnter()
    {
        bMouseOver = true;
        if (!Input.GetMouseButton(1))
        {
            mySpriteRenderer.color = MouseOverColor;
            IconRenderer.color = MouseOverColor;
        }
    }

    public void RightClickReleased()
    {
        if(bMouseOver)
        {
            OnMouseEnter();
        }
    }

    void OnMouseExit()
    {
        bMouseOver = false;
        mySpriteRenderer.color = NormalColor;
        IconRenderer.color = NormalColor;
    }

    public void SetEnabled(bool bEnabled)
    {
        BrainNodeEnabled = bEnabled;

        CancelIconRenderer.enabled = !BrainNodeEnabled;
    }

    public void SetReadyToSwap(bool bReadyToSwap)
    {
        SwapIconRenderer.enabled = bReadyToSwap;
    }

    public void Swap(BrainNode otherNode)
    {
        data.Swap(otherNode.data);

        Sprite swapSprite = IconRenderer.sprite;
        IconRenderer.sprite = otherNode.IconRenderer.sprite;
        otherNode.IconRenderer.sprite = swapSprite;

        bool swapEnabled = BrainNodeEnabled;
        SetEnabled(otherNode.BrainNodeEnabled);
        otherNode.SetEnabled(swapEnabled);
    }
}
