using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera MainCamera;

    private void Start()
    {
        if (MainCamera != null)
        {
            MainCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (MainCamera != null)
        {
            transform.rotation = MainCamera.transform.rotation;
        }
    }
}
