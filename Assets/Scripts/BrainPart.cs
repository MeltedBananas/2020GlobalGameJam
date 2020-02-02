using System;
using UnityEngine;

public class BrainPart : MonoBehaviour
{
    private Brain _brain = null;

    private void Awake()
    {
        _brain = GetComponentInParent<Brain>();
    }

    private void OnMouseOver()
    {
        _brain.ShowRotateInfo();
    }
}
