﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BrainToolType
{
    None,
    Inspect,
    SwapStart,
    SwapEnd,
    Cancel,
}

public class BrainData
{
    string Name;

    public BrainData(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}

public class Brain : MonoBehaviour
{
    List<BrainNode> Nodes = new List<BrainNode>();

    public Texture2D BarUI;
    public Vector2 BarPosition;
    public Vector2 BarWidth;

    public Vector2 ButtonWidth;

    public Texture2D InspectCursor;
    public Texture2D InspectButtonUI;
    public Vector2 InspectPosition;
    public Vector2 InspectBoxPosition;
    public Vector2 InspectBoxSize;
    private BrainNode InspectBrainNode;

    public Texture2D CancelCursor;
    public Texture2D CancelButtonUI;
    public Vector2 CancelPosition;

    public Texture2D SwapCursorStart;
    public Texture2D SwapCursorEnd;
    public Texture2D SwapButtonUI;
    public Vector2 SwapPosition;

    BrainToolType Tool;

    private void Start()
    {
        GatherBrainNodes(transform);
        foreach(BrainNode node in Nodes)
        {
            node.BrainOwner = this;
        }

        Setup(new List<object>() { new BrainData("A"), new BrainData("B"), new BrainData("C"), new BrainData("D"), new BrainData("E"), new BrainData("F"), new BrainData("G"), new BrainData("H") });
    }

    public void Setup(List<object> dataList)
    {
        List<BrainNode> randomizeNodes = new List<BrainNode>();
        for(int i = 0; i < dataList.Count; ++i)
        {
            int randomIdx = UnityEngine.Random.Range(0, Nodes.Count);
            Nodes[randomIdx].data = dataList[i];
            randomizeNodes.Add(Nodes[randomIdx]);
            Nodes.RemoveAt(randomIdx);
        }

        for(int i = Nodes.Count - 1; i >= 0; --i)
        {
            if(Nodes[i].data == null)
            {
                UnityEngine.Object.Destroy(Nodes[i].gameObject);
            }
        }

        Nodes.Clear();
        Nodes.AddRange(randomizeNodes);
    }

    void GatherBrainNodes(Transform itTransform)
    {
        Nodes.AddRange(itTransform.gameObject.GetComponents<BrainNode>());

        for(int i = 0; i < itTransform.childCount; ++i)
        {
            GatherBrainNodes(itTransform.GetChild(i));
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(1))
        {
            foreach(BrainNode node in Nodes)
            {
                node.RightClickReleased();
            }
        }
    }

    public void OnNodeClick(BrainNode node)
    {
        switch(Tool)
        {
            case BrainToolType.Inspect:
                InspectBrainNode = node;
                break;
            case BrainToolType.Cancel:
                node.SetEnabled(!node.BrainNodeEnabled);
                break;
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(BarPosition, BarWidth), BarUI);

        if (Tool != BrainToolType.Inspect)
        {
            if (GUI.Button(new Rect(InspectPosition, ButtonWidth), InspectButtonUI))
            {

                Tool = BrainToolType.Inspect;
                Cursor.SetCursor(InspectCursor, Vector2.zero, CursorMode.ForceSoftware);
                InspectBrainNode = null;
            }
        }
        else
        {
            GUI.DrawTexture(new Rect(InspectPosition, ButtonWidth), InspectButtonUI);
        }

        if (Tool != BrainToolType.Cancel)
        {
            if (GUI.Button(new Rect(CancelPosition, ButtonWidth), CancelButtonUI))
            {

                Tool = BrainToolType.Cancel;
                Cursor.SetCursor(CancelCursor, Vector2.zero, CursorMode.ForceSoftware);
                InspectBrainNode = null;
            }
        }
        else
        {
            GUI.DrawTexture(new Rect(CancelPosition, ButtonWidth), CancelButtonUI);
        }

        if (Tool != BrainToolType.SwapStart && Tool != BrainToolType.SwapEnd)
        {
            if (GUI.Button(new Rect(SwapPosition, ButtonWidth), SwapButtonUI))
            {

                Tool = BrainToolType.SwapStart;
                Cursor.SetCursor(SwapCursorStart, Vector2.zero, CursorMode.ForceSoftware);
                InspectBrainNode = null;
            }
        }
        else
        {
            GUI.DrawTexture(new Rect(SwapPosition, ButtonWidth), SwapButtonUI);
        }
        
        if(Tool == BrainToolType.Inspect && InspectBrainNode != null)
        {
            // Create style for a button
            GUIStyle myStyle = new GUIStyle(GUI.skin.box);
            myStyle.fontSize = 50;
            string nodeInfo = string.Format("{0} - {1}", ((BrainData)InspectBrainNode.data).ToString(), InspectBrainNode.BrainNodeEnabled ? "Enabled" : "Disabled");
            GUI.Box(new Rect(InspectBoxPosition, InspectBoxSize), nodeInfo, myStyle);
        }
    }
}
