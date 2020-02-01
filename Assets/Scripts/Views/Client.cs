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
        Invoke(nameof(Talk), 2f);
    }

    public void Talk()
    {
        if (_eyes != null) _eyes.Play();
        if (_mouth != null) _mouth.Play();
    }

    public void Shutup()
    {
        if (_eyes != null) _eyes.Stop();
        if (_mouth != null) _mouth.Stop();
    }
}
