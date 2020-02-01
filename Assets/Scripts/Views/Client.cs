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

        Invoke(nameof(Talk), 2f);
    }

    public void Talk()
    {
        if (_mouth != null) _mouth.Play();
        if (_eyes != null) _eyes.SpeedUp(2f);
        StartCoroutine(StopTalking(3f));
    }

    private IEnumerator StopTalking(float time)
    {
        yield return new WaitForSeconds(3f);
        Shutup();
    }

    public void Shutup()
    {
        if (_mouth != null) _mouth.Stop();
        if (_eyes != null) _eyes.SpeedUp(1f);
    }
}
