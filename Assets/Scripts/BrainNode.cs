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

    private void Start()
    {
        mainCamera = Camera.main;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = NormalColor;
    }

    void OnMouseDown()
    {
        Debug.Log(data.ToString());
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
}
