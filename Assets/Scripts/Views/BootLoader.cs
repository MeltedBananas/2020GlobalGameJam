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
    public Transform _clientsParent = null;
    public TextSpeachAnimation _speachBubble = null;

    private LevelDefinition _currentLevel = null;
    private Client _currentClient = null;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(_brainScene, LoadSceneMode.Additive);

        PickRandomLevel();
        InstantiateLevelObjects();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        // Brain Scene was loaded !!
        var rootGameObjects = scene.GetRootGameObjects();
        // Get from root game objects whichever components you need !!
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
        }
    }
}
