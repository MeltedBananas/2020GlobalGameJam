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
        var asyncOp = SceneManager.LoadSceneAsync(_brainScene, LoadSceneMode.Additive);
    }
}
