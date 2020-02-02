using System;
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
    public Word Word;

    public BrainData(Word word)
    {
        Word = word;
    }

    public override string ToString()
    {
        return Word.ToString();
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
    private BrainNode SwappingBrainNode;

    public Camera MainCamera;
    public SetCameraViewport CameraViewport;

    public List<Texture2D> PossibleNodeIcons;

    public Action OnLoaded = null;

    BrainToolType Tool;

    private bool bOnBrainScreen = false;
    Rect CursorZone;

    bool bShow = false;

    private void Start()
    {
        float zoneHeight = CameraViewport._viewportRect.size.y * Screen.height;
        CursorZone = new Rect(CameraViewport._viewportRect.position.x * Screen.width, Screen.height - CameraViewport._viewportRect.position.y * Screen.height - zoneHeight, CameraViewport._viewportRect.size.x * Screen.width, zoneHeight);

        GatherBrainNodes(transform);
        foreach(BrainNode node in Nodes)
        {
            node.BrainOwner = this;
            node.MainCamera = MainCamera;
            node.Init();
        }

        OnLoaded?.Invoke();
    }

    public void Setup(List<BrainData> dataList)
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

        foreach(BrainNode node in Nodes)
        {
            int rndIconIdx = UnityEngine.Random.Range(0, PossibleNodeIcons.Count);
            Texture2D icon = PossibleNodeIcons[rndIconIdx];
            node.IconRenderer.sprite = Sprite.Create(icon, new Rect(0.0f, 0.0f, icon.width, icon.height), new Vector2(0.5f, 0.5f), 100.0f);

            PossibleNodeIcons.RemoveAt(rndIconIdx);
        }
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
            case BrainToolType.SwapStart:
                SwappingBrainNode = node;
                SwappingBrainNode.SetReadyToSwap(true);
                Tool = BrainToolType.SwapEnd;
                RefreshCursor();
                break;
            case BrainToolType.SwapEnd:
                node.Swap(SwappingBrainNode);
                Tool = BrainToolType.SwapStart;
                Cursor.SetCursor(SwapCursorStart, Vector2.zero, CursorMode.ForceSoftware);
                if (SwappingBrainNode != null)
                {
                    SwappingBrainNode.SetReadyToSwap(false);
                    SwappingBrainNode = null;
                }
                break;
        }
    }

    public void Show(bool bShow)
    {
        this.bShow = bShow;
    }

    private void OnGUI()
    {
        if(!bShow)
        {
            return;
        }

        GUI.DrawTexture(new Rect(BarPosition, BarWidth), BarUI);

        if (Tool != BrainToolType.Inspect)
        {
            if (GUI.Button(new Rect(InspectPosition, ButtonWidth), InspectButtonUI))
            {

                Tool = BrainToolType.Inspect;
                Cursor.SetCursor(InspectCursor, Vector2.zero, CursorMode.ForceSoftware);
                InspectBrainNode = null;
                if (SwappingBrainNode != null)
                {
                    SwappingBrainNode.SetReadyToSwap(false);
                    SwappingBrainNode = null;
                }
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
                if (SwappingBrainNode != null)
                {
                    SwappingBrainNode.SetReadyToSwap(false);
                    SwappingBrainNode = null;
                }
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
                if (SwappingBrainNode != null)
                {
                    SwappingBrainNode.SetReadyToSwap(false);
                    SwappingBrainNode = null;
                }
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
            string nodeInfo = string.Format("{0} - {1}", InspectBrainNode.data.ToString(), InspectBrainNode.BrainNodeEnabled ? "Enabled" : "Disabled");
            GUI.Box(new Rect(InspectBoxPosition, InspectBoxSize), nodeInfo, myStyle);
        }

        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        bool bIsCurrentlyOnScreen = CursorZone.Contains(mousePosition);
        if(bIsCurrentlyOnScreen != bOnBrainScreen)
        {
            bOnBrainScreen = bIsCurrentlyOnScreen;

            if(bOnBrainScreen)
            {
                RefreshCursor();
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    void RefreshCursor()
    {
        switch(Tool)
        {
            case BrainToolType.Cancel:
                Cursor.SetCursor(CancelCursor, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case BrainToolType.Inspect:
                Cursor.SetCursor(InspectCursor, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case BrainToolType.SwapEnd:
                Cursor.SetCursor(SwapCursorEnd, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case BrainToolType.SwapStart:
                Cursor.SetCursor(SwapCursorStart, Vector2.zero, CursorMode.ForceSoftware);
                break;
        }
    }
    public bool ValidateBrain(List<LevelSolution> solution)
    { 
        
        bool result = true;

        foreach(LevelSolution s in solution)
        {   
            bool solutionFound = false;
            foreach(BrainNode node in Nodes)
            {
                if(s.ValidateSolution(node))
                {
                    solutionFound = true;
                    break;
                }
            }
            result &= solutionFound;
        }
        return result;
    }
}
