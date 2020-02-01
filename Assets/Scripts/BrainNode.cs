using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BrainNode : MonoBehaviour
{
    public Color NormalColor;
    public Color MouseOverColor;

    public string Label;
    public Vector2 Size = new Vector2(32.0f, 32.0f);

    private Camera mainCamera;
    private SpriteRenderer mySpriteRenderer;

    public object data = null;
    bool bMouseOver = false;

    public Texture2D BaseIcon;
    public Texture2D CancelIcon;
    public Texture2D SwapIcon;

    public Brain BrainOwner;
    public bool BrainNodeEnabled = true;

    private void Start()
    {
        mainCamera = Camera.main;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = NormalColor;
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
    }

    public void SetEnabled(bool bEnabled)
    {
        BrainNodeEnabled = bEnabled;
        if (BrainNodeEnabled)
        {
            mySpriteRenderer.sprite = Sprite.Create(BaseIcon, new Rect(0.0f, 0.0f, BaseIcon.width, BaseIcon.height), new Vector2(0.5f, 0.5f), 250.0f);
        }
        else
        {
            mySpriteRenderer.sprite = Sprite.Create(CancelIcon, new Rect(0.0f, 0.0f, CancelIcon.width, CancelIcon.height), new Vector2(0.5f, 0.5f), 250.0f);
        }
    }

    public void SetReadyToSwap(bool bReadyToSwap)
    {
        if (bReadyToSwap)
        {
            mySpriteRenderer.sprite = Sprite.Create(SwapIcon, new Rect(0.0f, 0.0f, SwapIcon.width, SwapIcon.height), new Vector2(0.5f, 0.5f), 250.0f);
        }
        else
        {
            SetEnabled(BrainNodeEnabled);
        }
    }
}
