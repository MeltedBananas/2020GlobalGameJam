﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool IsQuestion = false;
    bool isActive = false;
    bool isMoving = false;
    public GameObject Item;
    public float _disappearSeconds = 0.85f;
    public LeanTweenType _disappearEaseType = LeanTweenType.linear;
    public float _appearSeconds = 0.25f;

    public OnClickOutside _onClickOutside = null;

    public void Start()
    {
        isActive = false;
        Item.SetActive(false);
        if (!IsQuestion)
        {
            isMoving = true;
            LeanTween.moveY(gameObject, -Screen.height, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
            {
                isMoving = false;
            });
        }
        else
        {
            LeanTween.moveX(gameObject, -Screen.width, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
            {
                Item.SetActive(false);
            });
        }
    }
    public void UI_ShowInventory()
    {
        if (isMoving)
        {
            return;
        }
        if (!isActive)
        {
            if (_onClickOutside != null)
                _onClickOutside.gameObject.SetActive(true);
            
            isActive = true;
            Item.SetActive(true);
            isMoving = true;
            LeanTween.moveY(gameObject, 0f, _appearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
            {
                isMoving = false;
            });
        }
        else
        {
            UI_HideInventory();
        }
    }
    
    public void UI_ShowQuestions()
    {
        if (isMoving)
        {
            return;
        }
        
        if (!isActive)
        {
            if (_onClickOutside != null)
                _onClickOutside.gameObject.SetActive(true);
            
            isActive = true;
            isMoving = true;
            Item.SetActive(true);
            LeanTween.moveX(gameObject, 0f, _appearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
            {
                isMoving = false;
            });
        }
        else 
        {
            UI_HideQuestions();
        }
    }

    public void UI_HideQuestions()
    {
        if (_onClickOutside != null)
            _onClickOutside.gameObject.SetActive(false);
        
        isActive = false;
        isMoving = true;
        LeanTween.moveX(gameObject, -Screen.width, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
        {
            Item.SetActive(false);
            isMoving = false;
        });
    }

    public void UI_HideInventory()
    {
        if (_onClickOutside != null)
            _onClickOutside.gameObject.SetActive(false);
        
        isMoving = true;
        isActive = false;
        LeanTween.moveY(gameObject, -Screen.height, _disappearSeconds).setEase(_disappearEaseType).setOnComplete(() =>
        {
            isMoving = false;
            Item.SetActive(false);
        });
    }
}
