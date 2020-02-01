using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    public string _brainScene = "BrainScene";
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(_brainScene, LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        // Brain Scene was loaded !!
        var rootGameObjects = scene.GetRootGameObjects();
        // Get from root game objects whichever components you need !!
    }
}
