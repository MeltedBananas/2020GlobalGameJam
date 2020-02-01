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

    private void Start()
    {
        mainCamera = Camera.main;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = NormalColor;
    }

    void OnMouseEnter()
    {
        mySpriteRenderer.color = MouseOverColor;
    }

    void OnMouseExit()
    {
        mySpriteRenderer.color = NormalColor;
    }
}
