using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [Header("BrainScene")]
    public string _brainScene = "BrainScene";
    
    [Header("Levels")]
    public List<LevelDefinition> _levelDefinitions = new List<LevelDefinition>();
    
    [Header("References")]
    public Transform _clientsParent = null;
    public TextSpeachAnimation _speachBubble = null;

    [Header("Speech Bubble Appear")]
    public float _appearAfterSeconds = 0.25f;
    public GameObject _speechBubbleImage = null;
    public LeanTweenType _scaleUpEaseType = LeanTweenType.easeInOutBack;
    public float _scaleUpTime = 0.4f;

    private LevelDefinition _currentLevel = null;
    private Client _currentClient = null;
    private Vector3 _speechBubbleInitialScale = Vector3.one;

    private void Awake()
    {
        _speachBubble.enabled = false;
        _speechBubbleImage.SetActive(false);
        _speechBubbleInitialScale = _speechBubbleImage.transform.localScale;
        _speechBubbleImage.transform.localScale = Vector3.zero;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(_brainScene, LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        // Brain Scene was loaded !!
        //var rootGameObjects = scene.GetRootGameObjects();
        // Get from root game objects whichever components you need !!
        
        PickRandomLevel();
        InstantiateLevelObjects();
    }

    private void PickRandomLevel()
    {
        _currentLevel = _levelDefinitions[UnityEngine.Random.Range(0, _levelDefinitions.Count)];
    }
    
    private void InstantiateLevelObjects()
    {
        if (_currentLevel != null)
        {
            _currentClient = Instantiate(_currentLevel.Client.gameObject, _clientsParent).GetComponent<Client>();
            StartCoroutine(ShowBubbleAfterAFewSeconds(_appearAfterSeconds));
        }
    }

    private IEnumerator ShowBubbleAfterAFewSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _speechBubbleImage.SetActive(true);
        LeanTween.scale(_speechBubbleImage, _speechBubbleInitialScale, _scaleUpTime).setEase(_scaleUpEaseType).setOnComplete(() => _speachBubble.enabled = true);
    }
}
