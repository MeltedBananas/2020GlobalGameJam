using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _postItsParent = null;
    [SerializeField] private GameObject _levelDescParent = null;
    
    [Header("Menu Disappear")]
    public float _disappearSeconds = 0.85f;
    public LeanTweenType _disappearEaseType = LeanTweenType.linear;
    public float _appearSeconds = 0.25f;
    
    public void UI_StartGame()
    {
        LeanTween.moveY(gameObject, -Screen.height, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
        {
            _postItsParent.SetActive(false);
            _levelDescParent.SetActive(true);
            LeanTween.moveY(gameObject, 0f, _appearSeconds).setEase(_disappearEaseType);
        });
    }

    public void UI_Credits()
    {
        // lol nothing
    }

    public void UI_Controls()
    {
        // lol nothing
    }
}
