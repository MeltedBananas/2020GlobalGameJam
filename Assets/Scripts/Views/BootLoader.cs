using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BootLoader : MonoBehaviour
{
    [Header("BrainScene")]
    public string _brainScene = "BrainScene";
    
    [Header("Levels")]
    public List<LevelDefinition> _levelDefinitions = new List<LevelDefinition>();

    [Header("References")]
    public GameObject _menu = null;
    public Menu _clipboard = null;
    public Transform _clientsParent = null;
    public Transform _inventoryParent = null;
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
    public Inventory ActionsMenu;
    public QuestionButton InventoryMenuButton;

    [Header("Menu Disappear")]
    public float _disappearSeconds = 0.85f;
    public LeanTweenType _disappearEaseType = LeanTweenType.linear;
    public float _appearSeconds = 0.85f;

    [Header("Audio")]
    [SerializeField] private AudioManager _audioManager = null;
    public AudioSource LevelValidatedAudio;

    private LevelDefinition _currentLevel = null;
    private Client _currentClient = null;
    private Vector3 _speechBubbleInitialScale = Vector3.one;
    private readonly List<GameObject> _brainSceneRootGameObjects = new List<GameObject>();
    private bool _firstTimeShown = false;
    public Brain _brain = null;
    private Camera _brainCamera = null;
    private Rect _brainCameraViewport = Rect.zero;
    private bool _isBrainCameraOpened = true;
    public FadeInOut _brainFadeInOut = null;
    private int CurrentLevelIndex = -1;
    private bool bWaitingForSpawn = false;
    private GameObject item;
    private bool bStartNextLevelAfterSpeech = false;
    public bool bInCredits = false;

    int QuestionAskedIndex = -1;

    private void Start()
    {
        _speachBubble.enabled = false;
        _speachBubble.OnTextComplete += OnTextComplete;
        _speechBubbleImage.SetActive(false);
        _speechBubbleInitialScale = _speechBubbleImage.transform.localScale;
        _speechBubbleImage.transform.localScale = Vector3.zero;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(_brainScene, LoadSceneMode.Additive);
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
            _brainFadeInOut = _brainCamera.GetComponentInChildren<FadeInOut>(true);
            _brainFadeInOut.AssociatedGameObject = _brainCamera.gameObject;
            _brainFadeInOut.SnapOut();
            _isBrainCameraOpened = false;

            foreach (var root in _brainSceneRootGameObjects)
            {
                _brain = root.GetComponentInChildren<Brain>(true);
                if (_brain != null)
                    break;
            }

            if (_brain != null)
            {
                _brain.Reset();
            }
        }
    }

    private void NextLevel(bool bShowLevel)
    {
        _speachBubble.ClearLine();
        QuestionAskedIndex = -1;

        if (CurrentLevelIndex < _levelDefinitions.Count - 1)
        {
            _currentLevel = _levelDefinitions[++CurrentLevelIndex];
            _levelDescription.SetText(_currentLevel.SetupDescription);
            _firstTimeShown = false;

            if (_brain != null)
            {
                _brain.bShow = false;
                _brain.Setup(this, _currentLevel);
                _currentLevel.FuckUp(_brain);
            }

            if (bShowLevel)
            {
                UI_ShowMenu();
                bWaitingForSpawn = true;
            }
            else
            {
                bWaitingForSpawn = false;
                InstantiateLevelObjects();
            }
        }
        else
        {
            EndOfGame();
        }
        
    }
    
    private void EndOfGame()
    {
        CurrentLevelIndex = _levelDefinitions.Count;

        _brain.Reset();
        _brain.Show(false);
        UI_TurnOffScreen();
        _clipboard.UI_EndofGame();
        _startGameButton.enabled = true;
    }

    private void InstantiateLevelObjects()
    {
        if (_currentLevel != null)
        {
            if(_currentClient != null)
            {
                Destroy(_currentClient.gameObject);
                _currentClient = null;
            }

             if(item != null)
            {
                Destroy(item.gameObject);
                item = null;
            } 
            _currentClient = Instantiate(_currentLevel.Client.gameObject, _clientsParent).GetComponent<Client>();
            _currentClient.Init(_currentLevel, _speachBubble);
            if (_currentLevel.ItemPrefab != null)
            {
                
                item = Instantiate(_currentLevel.ItemPrefab.gameObject, _inventoryParent);
            }

           
            foreach(QuestionButton questionButton in QuestionButtons)
            {
                questionButton.Initialized(_currentLevel);
            }
        }
    }

    public void UI_StartGame()
    {
        if (bInCredits)
        {
            CurrentLevelIndex = -1;
            bInCredits = false;

            LeanTween.moveY(_menu, -Screen.height - 100, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
            {
                _clipboard.UI_MainMenu();
            });
            
        }
        else
        {
            if (CurrentLevelIndex < _levelDefinitions.Count)
            {
                LeanTween.moveY(_menu, -Screen.height - 100, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
                {
                    UI_TurnOnScreen();
                    _brain.Show(true);
                    ActionsMenu.UI_ShowQuestions();

                    StartCoroutine(ShowBubbleAfterAFewSeconds(_appearAfterSeconds));
                });
            }
            else
            {
                if (_currentClient != null)
                {
                    Destroy(_currentClient.gameObject);
                    _currentClient = null;
                }

                if (item != null)
                {
                    Destroy(item.gameObject);
                    item = null;
                }

                _clipboard.UI_Credits();
            }
        }
    }
    
    private IEnumerator ShowBubbleAfterAFewSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ShowSpeech();
    }

    private IEnumerator NextLevelAfterAFewSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HideSpeech();
        NextLevel(true);
    }

    public void ShowSpeech()
    {
        _speechBubbleImage.SetActive(true);
        LeanTween.scale(_speechBubbleImage, _speechBubbleInitialScale, _scaleUpTime).setEase(_scaleUpEaseType).setOnComplete(() =>
        {
            _speachBubble.enabled = true;
            _currentClient.Talk();
            _audioManager.PlaySound(AudioManager.SoundsBank.TalkSpeech);
            _speachBubble.SetupLine(_currentLevel.ClientDescription);
            UI_TurnOnScreen();
        });
    }

    public void HideSpeech()
    {
        LeanTween.scale(_speechBubbleImage, Vector3.zero, _scaleUpTime).setEase(_scaleUpEaseType).setOnComplete(() =>
        {
            _speachBubble.enabled = false;
            _currentClient.Shutup();
            _speachBubble.ClearLine();

            QuestionButtons.ForEach(x => x.gameObject.SetActive(false));
            if (_currentLevel.ItemPrefab != null && _currentLevel.ItemSprite != null)
            {
                InventoryMenuButton.gameObject.SetActive(false);
            }
        });
    }
    
    private void OnTextComplete()
    {
        if (_currentClient != null)
        {
            _currentClient.Shutup();

            if(QuestionAskedIndex >= 0)
            {
                bool bValidated = _brain.Validate(_currentLevel.Solutions, QuestionButtons[QuestionAskedIndex].AssociatedLabels);
                QuestionButtons[QuestionAskedIndex].SetValidationState(bValidated ? QuestionButton.ValidationState.Valid : QuestionButton.ValidationState.Invalid);
            }
        }
        _audioManager.StopSound();

        if (!_firstTimeShown)
        {
            QuestionButtons.ForEach(x => x.ScaleUp());
            if (_currentLevel.ItemPrefab != null && _currentLevel.ItemSprite != null)
            {
                InventoryMenuButton.gameObject.GetComponent<Image>().sprite = _currentLevel.ItemSprite;

                float widthRatio = _currentLevel.ItemSprite.texture.width / _currentLevel.ItemSprite.texture.height;
                Vector2 currentRect = ((RectTransform)InventoryMenuButton.gameObject.transform).sizeDelta;
                ((RectTransform)InventoryMenuButton.gameObject.transform).sizeDelta = new Vector2(currentRect.y * widthRatio, currentRect.y);
                InventoryMenuButton.gameObject.SetActive(true);
                InventoryMenuButton.ScaleUp();
            }
            _firstTimeShown = true;
        }

        if (bStartNextLevelAfterSpeech)
        {
            StartCoroutine(NextLevelAfterAFewSeconds(1.75f));
            bStartNextLevelAfterSpeech = false;
        }
        else
        {
            bool bAllQuestionValidated = true;
            for (int i = 0; i < _currentLevel.Questions.Count; ++i)
            {
                QuestionButton questionButton = QuestionButtons[i];
                if (questionButton.ButtonState != QuestionButton.ValidationState.Valid)
                {
                    bAllQuestionValidated = false;
                    break;
                }
            }

            if (bAllQuestionValidated)
            {
                StartCoroutine(TestSolution(1.5f));
            }
        }
    }

    public void UI_ShowMenu()
    {
        _brain.Reset();
        UI_TurnOffScreen();
        QuestionButtons.ForEach(x => x.gameObject.SetActive(false));
        ActionsMenu.UI_HideQuestions();
        InventoryMenuButton.gameObject.SetActive(false);
        LeanTween.moveY(_menu, 10f, _appearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
        {
            _startGameButton.enabled = true;
            if (bWaitingForSpawn)
            {
                InstantiateLevelObjects();
                bWaitingForSpawn = false;
            }
        });
    }

    public void UI_AskQuestion(int index)
    {
        QuestionAskedIndex = index;
        if(QuestionButtons[QuestionAskedIndex].ButtonState == QuestionButton.ValidationState.Invalid)
        {
            QuestionButtons[QuestionAskedIndex].SetValidationState(QuestionButton.ValidationState.Unknown);
        }

        _currentClient.AskQuestion(index);
        _currentClient.Talk();
        _audioManager.PlaySound(AudioManager.SoundsBank.TalkSpeech);
    }

    private IEnumerator TestSolution(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (_brain.ValidateBrain(_currentLevel.Solutions))
        {
            _currentClient.Talk();
            _audioManager.PlaySound(AudioManager.SoundsBank.TalkSpeech);
            _speachBubble.SetupLine(_currentLevel.SuccessSpeech);
            UI_TurnOffScreen();

            QuestionAskedIndex = -1;
            bStartNextLevelAfterSpeech = true;
            LevelValidatedAudio.Play();
        }
    }

    public void UI_ToggleScreen()
    {
        if (_isBrainCameraOpened)
            UI_TurnOffScreen();
        else
            UI_TurnOnScreen();
    }

    public void UI_TurnOnScreen()
    {
        _isBrainCameraOpened = true;
        _brainFadeInOut.FadeIn();

    }

    public void UI_TurnOffScreen()
    {
        _isBrainCameraOpened = false;
        _brainFadeInOut.FadeOut();
    }

    public void Update()
    {
        if (Input.GetButtonDown("CheatNextLevel"))
        {
            NextLevel(true);
        }
        else if (Input.GetButtonDown("CheatPreviousLevel"))
        {
            CurrentLevelIndex = Mathf.Max(CurrentLevelIndex - 1, -1);
            NextLevel(true);
        }
    }

    public void StartGame(bool bMenuAnimDone)
    {
        if (bMenuAnimDone)
        {
            UI_ShowMenu();
        }
        else
        {
            NextLevel(false);
        }
    }

    public void OnModifiedBrainNode(BrainNode node)
    {
        if (node != null && node.data != null && node.data.Word != null)
        {
            foreach (QuestionButton button in QuestionButtons)
            {
                if (button.AssociatedLabels.Contains(node.data.Word.Label))
                {
                    button.SetValidationState(QuestionButton.ValidationState.Unknown);
                }
            }
        }
    }
}
