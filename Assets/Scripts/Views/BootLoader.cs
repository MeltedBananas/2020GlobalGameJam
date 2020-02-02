using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [Header("BrainScene")]
    public string _brainScene = "BrainScene";
    
    [Header("Levels")]
    public List<LevelDefinition> _levelDefinitions = new List<LevelDefinition>();

    [Header("References")]
    public GameObject _menu = null;
    public Transform _clientsParent = null;
    public TextSpeachAnimation _speachBubble = null;
    public TMP_Text _levelDescription = null;
    public WorldButton _startGameButton = null;

    [Header("Speech Bubble Appear")]
    public float _appearAfterSeconds = 0.25f;
    public GameObject _speechBubbleImage = null;
    public LeanTweenType _scaleUpEaseType = LeanTweenType.easeInOutBack;
    public float _scaleUpTime = 0.4f;

    [Header("Question Buttons")]
    public List<QuestionButton> QuestionButtons;

    [Header("Menu Disappear")]
    public float _disappearSeconds = 0.85f;
    public LeanTweenType _disappearEaseType = LeanTweenType.linear;
    public float _appearSeconds = 0.25f;

    private LevelDefinition _currentLevel = null;
    private Client _currentClient = null;
    private Vector3 _speechBubbleInitialScale = Vector3.one;
    private readonly List<GameObject> _brainSceneRootGameObjects = new List<GameObject>();
    private bool _firstTimeShown = false;
    public Brain _brain = null;
    private Camera _brainCamera = null;
    private Rect _brainCameraViewport = Rect.zero;
    private bool _isBrainCameraOpened = true;

    private int CurrentLevelIndex = -1;

    private void Awake()
    {
        NextLevel();

        _speachBubble.enabled = false;
        _speachBubble.OnTextComplete += OnTextComplete;
        _speechBubbleImage.SetActive(false);
        _speechBubbleInitialScale = _speechBubbleImage.transform.localScale;
        _speechBubbleImage.transform.localScale = Vector3.zero;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(_brainScene, LoadSceneMode.Additive);
    }

    private void Start()
    {
        //NextLevel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == _brainScene)
        {
            _brainSceneRootGameObjects.Clear();
            _brainSceneRootGameObjects.AddRange(scene.GetRootGameObjects());
            
            foreach (var root in _brainSceneRootGameObjects)
            {
                _brainCamera = root.GetComponentInChildren<Camera>(true);
                if (_brainCamera != null)
                    break;
            }

            _brainCameraViewport = _brainCamera.rect;
            _brainCamera.gameObject.SetActive(false);

            foreach (var root in _brainSceneRootGameObjects)
            {
                _brain = root.GetComponentInChildren<Brain>(true);
                if (_brain != null)
                    break;
            }

            _brain.OnLoaded += () =>
            {
#if UNITY_EDITOR
                _currentLevel.AvailableTools.Add(BrainToolType.Inspect);
#endif
                _brain.Setup(_currentLevel);
                _currentLevel.FuckUp(_brain);
            };


            InstantiateLevelObjects();
        }
    }

    private void NextLevel()
    {
        _currentLevel = _levelDefinitions[++CurrentLevelIndex];
        _levelDescription.SetText(_currentLevel.SetupDescription);
        _firstTimeShown = false;
    }
    
    private void InstantiateLevelObjects()
    {
        if (_currentLevel != null)
        {
            _currentClient = Instantiate(_currentLevel.Client.gameObject, _clientsParent).GetComponent<Client>();


            _currentClient.Init(_currentLevel, _speachBubble);
            foreach(QuestionButton questionButton in QuestionButtons)
            {
                questionButton.Initialized(_currentLevel);
            }
        }
    }

    public void UI_StartGame()
    {
        LeanTween.moveY(_menu, -Screen.height, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
        {
            _brainCamera.gameObject.SetActive(true);
            _brain.Show(true);
            StartCoroutine(ShowBubbleAfterAFewSeconds(_appearAfterSeconds));
        });
    }
    
    private IEnumerator ShowBubbleAfterAFewSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _speechBubbleImage.SetActive(true);
        LeanTween.scale(_speechBubbleImage, _speechBubbleInitialScale, _scaleUpTime).setEase(_scaleUpEaseType).setOnComplete(() =>
        {
            _speachBubble.enabled = true;
            _currentClient.Talk();
            _speachBubble.SetupLine(_currentLevel.ClientDescription);
        });
    }
    
    private void OnTextComplete()
    {
        _currentClient.Shutup();

        if (!_firstTimeShown)
        {
            QuestionButtons.ForEach(x => x.ScaleUp());
            _firstTimeShown = true;
        }
    }

    public void UI_ShowMenu()
    {
        LeanTween.moveY(_menu, 0f, _appearSeconds).setEase(_disappearEaseType).setOnComplete(() => _startGameButton.enabled = true);
    }

    public void UI_AskQuestion(int index)
    {
        _currentClient.AskQuestion(index);
        if (_brain.ValidateBrain(_currentLevel.Solutions))
        {
            Debug.Log("WIN!!!!");
        }
        
    }

    public void UI_CloseScreen()
    {
        // TODO: only enable after it has opened
        _isBrainCameraOpened = !_isBrainCameraOpened;
        
        
    }

    public void Update()
    {
        if (Input.GetButtonDown("CheatNextLevel"))
        {
            NextLevel();
        }
        else if (Input.GetButtonDown("CheatPreviousLevel"))
        {
            CurrentLevelIndex = Mathf.Max(CurrentLevelIndex - 1, -1);
            NextLevel();
        }
    }
}
