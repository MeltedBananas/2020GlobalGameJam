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

    [Header("Menu Disappear")]
    public float _disappearSeconds = 0.85f;
    public LeanTweenType _disappearEaseType = LeanTweenType.linear;
    public float _appearSeconds = 0.25f;

    private LevelDefinition _currentLevel = null;
    private Client _currentClient = null;
    private Vector3 _speechBubbleInitialScale = Vector3.one;
    private readonly List<GameObject> _brainSceneRootGameObjects = new List<GameObject>();

    private void Awake()
    {
        PickRandomLevel();

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
            _brainSceneRootGameObjects.ForEach(x => x.SetActive(false));
        
            InstantiateLevelObjects();
        }
    }

    private void PickRandomLevel()
    {
        _currentLevel = _levelDefinitions[UnityEngine.Random.Range(0, _levelDefinitions.Count)];
        _levelDescription.SetText(_currentLevel.SetupDescription);
    }
    
    private void InstantiateLevelObjects()
    {
        if (_currentLevel != null)
        {
            _currentClient = Instantiate(_currentLevel.Client.gameObject, _clientsParent).GetComponent<Client>();
        }
    }

    public void UI_StartGame()
    {
        LeanTween.moveY(_menu, -Screen.height, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
        {
            _brainSceneRootGameObjects.ForEach(x => x.SetActive(true));
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
    }

    public void UI_ShowMenu()
    {
        LeanTween.moveY(_menu, 0f, _appearSeconds).setEase(_disappearEaseType).setOnComplete(() => _startGameButton.enabled = true);
    }
}
