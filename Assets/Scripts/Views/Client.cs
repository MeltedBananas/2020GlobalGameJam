using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private Animate _eyes = null;
    [SerializeField] private Animate _mouth = null;

    private void Start()
    {
        if (_eyes != null) _eyes.Play();
    }

    public void Talk()
    {
        if (_mouth != null) _mouth.Play();
    }

    public void Shutup()
    {
        if (_mouth != null) _mouth.Stop();
    }
}
