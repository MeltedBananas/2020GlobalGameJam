﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BrainNode : MonoBehaviour
{
    public Color NormalColor;
    public Color MouseOverColor;

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

    public ParticleSystem PingPS = null;
    public float StopPingAfterSeconds = 2f;

    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySpriteRenderer.color = NormalColor;
        IconRenderer.color = NormalColor;

        SetReadyToSwap(false);
        SetEnabled(BrainNodeEnabled);
    }

    public void Init(Camera mainCamera)
    {
        if (mainCamera != null)
        {
            Billboard.MainCamera = mainCamera;
        }
    }

    public void Reset()
    {
        SetEnabled(true);
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
        if (BrainOwner != null && BrainOwner.CurrentBootLoader != null)
        {
            BrainOwner.CurrentBootLoader.OnModifiedBrainNode(this);
        }
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

        BrainOwner.CurrentBootLoader.OnModifiedBrainNode(this);
        BrainOwner.CurrentBootLoader.OnModifiedBrainNode(otherNode);
    }

    private Coroutine _stopPingCoroutine = null;
    public void Ping()
    {
        if (_stopPingCoroutine != null)
            StopCoroutine(_stopPingCoroutine);
        
        PingPS.Play();
        _stopPingCoroutine = StartCoroutine(DoStopPingAfterSeconds());
    }

    private IEnumerator DoStopPingAfterSeconds()
    {
        yield return new WaitForSeconds(StopPingAfterSeconds);
        PingPS.Stop();
    }

    public void Refresh(ref Word word)
    {
        if (word.Label == data.Word.Label)
        {
            if (BrainNodeEnabled)
            {
                word = data.Word;
            }
            else
            {
                if (word.SilentWhenCancelled)
                {
                    word = new Word("");
                }
                else
                {
                    word = new Word("*hmm*");
                }
                word.Label = data.Word.Label;
            }
            Ping();
        }
    }
}
