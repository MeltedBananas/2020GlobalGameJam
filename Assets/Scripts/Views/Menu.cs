using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _postItsParent = null;
    [SerializeField] private GameObject _levelDescParent = null;
    [SerializeField] private GameObject _creditParent = null;
    [SerializeField] private string _introText = "Explain the game here";
    public AudioManager _audioManager = null;

    [Header("Menu Disappear")]
    public float _disappearSeconds = 0.85f;
    public LeanTweenType _disappearEaseType = LeanTweenType.linear;
    public float _appearSeconds = 0.85f;
    public BootLoader Bootloader;
    public TMP_Text _levelDescription = null;
    public string EndOfGameMessage = "You have done a great job! All client were sucessfully fixed!";

    public void UI_MainMenu()
    {
        _postItsParent.SetActive(true);
        _levelDescParent.SetActive(false);
        _creditParent.SetActive(false);
        LeanTween.moveY(gameObject, 10f, _appearSeconds).setEase(_disappearEaseType);
    }

    public void UI_StartGame()
    {
        _audioManager.PlayMusic();
        
        LeanTween.moveY(gameObject, -Screen.height -100, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
        {
            _postItsParent.SetActive(false);
            _levelDescParent.SetActive(true);
            Bootloader.StartGame(BootLoader.EGameFlow.GameIntro);
            _levelDescription.SetText(_introText);
        });
    }

    public void UI_EndofGame()
    {
        _levelDescription.SetText(EndOfGameMessage);
        LeanTween.moveY(gameObject, 10f, _appearSeconds).setEase(_disappearEaseType);
    }

    public void UI_Credits()
    {   
        LeanTween.moveY(gameObject, -Screen.height - 100, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
        {
            _postItsParent.SetActive(false);
            _levelDescription.SetText("");
            _levelDescParent.SetActive(true);
            Bootloader._startGameButton.enabled = true;
            Bootloader.bInCredits = true;
            _creditParent.SetActive(true);
            LeanTween.moveY(gameObject, 0, _appearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
            {
            });
        });
    }

    public void UI_Controls()
    {
        // lol nothing
    }
}
