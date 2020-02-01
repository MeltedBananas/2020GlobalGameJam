using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            transform.rotation = SceneView.lastActiveSceneView.rotation;
        }
    }
}
