using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrainToolType
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

    public void Swap(BrainData other)
    {
        string label = Word.Label;
        string labelOther = other.Word.Label;

        Word swapWord = new Word(other.Word);

        other.Word = Word;
        Word = swapWord;

        Word.Label = label;
        other.Word.Label = labelOther;
    }

    public override string ToString()
    {
        return Word.ToString();
    }
}

public class Brain : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private LevelDefinition _testLevel = null;     
#endif
    
    public List<BrainNode> Nodes = new List<BrainNode>();

    public Color OnColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color OffColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

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

    [SerializeField] private GameObject _infoBox = null;
    [SerializeField] private GameObject _swapInfo = null;
    [SerializeField] private GameObject _cancelInfo = null;
    [SerializeField] private GameObject _rotateInfo = null;

    private bool _swapInfoShown = false;
    private bool _cancelInfoShown = false;
    private bool _rotateInfoShown = false;

    public Action OnLoaded = null;

    private LevelDefinition CurrentLevel;
    public BootLoader CurrentBootLoader;

    BrainToolType Tool;

    private bool bOnBrainScreen = false;
    Rect CursorZone;

    public bool bShow = false;

    private void Awake()
    {
        _infoBox.gameObject.SetActive(false);
    }

    private void Start()
    {
        float zoneHeight = CameraViewport._viewportRect.size.y * Screen.height;
        CursorZone = new Rect(CameraViewport._viewportRect.position.x * Screen.width, Screen.height - CameraViewport._viewportRect.position.y * Screen.height - zoneHeight, CameraViewport._viewportRect.size.x * Screen.width, zoneHeight);

        GatherBrainNodes(transform);
        foreach(BrainNode node in Nodes)
        {
            node.BrainOwner = this;
            node.Init(MainCamera);
        }

        OnLoaded?.Invoke();
    }

    public void Reset()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Tool = BrainToolType.None;
    }

    public void Setup(BootLoader currentBootLoader, LevelDefinition currentLevel)
    {
        CurrentBootLoader = currentBootLoader;
        CurrentLevel = currentLevel;

        Nodes.Clear();
        GatherBrainNodes(transform);

        for (int i = Nodes.Count - 1; i >= 0; --i)
        {
            Nodes[i].Reset();
            Nodes[i].gameObject.SetActive(false);
        }

        List<BrainData> dataList = CurrentLevel.GenerateBrainDataList();

        List<BrainNode> randomizeNodes = new List<BrainNode>();
        for(int i = 0; i < dataList.Count; ++i)
        {
            int randomIdx = UnityEngine.Random.Range(0, Nodes.Count);
            Nodes[randomIdx].data = dataList[i];
            Nodes[randomIdx].gameObject.SetActive(true);
            randomizeNodes.Add(Nodes[randomIdx]);
            Nodes.RemoveAt(randomIdx);
        }

        Nodes.Clear();
        Nodes.AddRange(randomizeNodes);

        List<Texture2D> possibleNodeIcons = new List<Texture2D>(PossibleNodeIcons);
        foreach (BrainNode node in Nodes)
        {
            int rndIconIdx = UnityEngine.Random.Range(0, possibleNodeIcons.Count);
            Texture2D icon = possibleNodeIcons[rndIconIdx];
            node.IconRenderer.sprite = Sprite.Create(icon, new Rect(0.0f, 0.0f, icon.width, icon.height), new Vector2(0.5f, 0.5f), 100.0f);

            possibleNodeIcons.RemoveAt(rndIconIdx);

            node.Init(MainCamera);
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
        if (_infoBox.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            _infoBox.gameObject.SetActive(false);
        }
        
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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            bShow = true;
            Setup(null, _testLevel);
        }
#endif
        
        if(!bShow)
        {
            return;
        }

        GUI.color = Color.Lerp(OffColor, OnColor, CurrentBootLoader._brainFadeInOut.GetRatio());

        GUI.DrawTexture(new Rect(BarPosition, BarWidth), BarUI);

        bool bCanShowInspectTool = CurrentLevel.AvailableTools.Contains(BrainToolType.Inspect);
#if UNITY_EDITOR
        bCanShowInspectTool = true;
#endif
        if (bCanShowInspectTool)
        {
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
        }

        if (CurrentLevel.AvailableTools.Contains(BrainToolType.Cancel))
        {
            if (Tool != BrainToolType.Cancel)
            {
                if (GUI.Button(new Rect(CancelPosition, ButtonWidth), CancelButtonUI))
                {
                    if (!_cancelInfoShown)
                    {
                        _cancelInfoShown = true;
                        // show info on first click!
                        ShowInfoBox(true, false, false);
                    }
                    
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
        }

        if (CurrentLevel.AvailableTools.Contains(BrainToolType.SwapStart) || CurrentLevel.AvailableTools.Contains(BrainToolType.SwapEnd))
        {
            if (Tool != BrainToolType.SwapStart && Tool != BrainToolType.SwapEnd)
            {
                if (GUI.Button(new Rect(SwapPosition, ButtonWidth), SwapButtonUI))
                {
                    if (!_swapInfoShown)
                    {
                        _swapInfoShown = true;
                        // show swap on first click!
                        ShowInfoBox(false, true, false);
                    }
                    
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

    public bool Validate(List<LevelSolution> solution, List<string> labels)
    {
        foreach (LevelSolution s in solution)
        {
            if (labels.Contains(s.Label))
            {
                if (!s.ValidateBrain(Nodes))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool ValidateBrain(List<LevelSolution> solution)
    { 
        foreach(LevelSolution s in solution)
        {
            if(!s.ValidateBrain(Nodes))
            {
                return false;
            }
        }
        return true;
    }

    public BrainNode GetFromLabel(string label)
    {
        foreach(BrainNode node in Nodes)
        {
            if(node.data.Word.Label == label)
            {
                return node;
            }
        }

        return null;
    }

    public void Refresh(ref Word word)
    {
        foreach(BrainNode node in Nodes)
        {
            node.Refresh(ref word);
        }
    }

    private void ShowInfoBox(bool isCancel, bool isSwap, bool isRotate)
    {
        Vector3 localScale = _infoBox.transform.localScale;
        _infoBox.transform.localScale = Vector3.zero;
        _infoBox.gameObject.SetActive(true);
        
        _cancelInfo.SetActive(isCancel);
        _swapInfo.SetActive(isSwap);
        _rotateInfo.SetActive(isRotate);
        
        LeanTween.scale(_infoBox, localScale, 1f).setEase(LeanTweenType.easeOutBack);
    }

    public void ShowRotateInfo()
    {
        if (bShow && !_rotateInfoShown)
        {
            _rotateInfoShown = true;
            ShowInfoBox(false, false, true);
        }
    }
}
