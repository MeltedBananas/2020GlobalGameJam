using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    bool isActive = false;
    public GameObject Item;
    public float _disappearSeconds = 0.85f;
    public LeanTweenType _disappearEaseType = LeanTweenType.linear;
    public float _appearSeconds = 0.25f;
    public void Start()
    {
        LeanTween.moveY(gameObject, -Screen.height, _disappearSeconds).setEase(_disappearEaseType);
    }
    public void UI_ShowInventory()
    {
        if (!isActive)
        {
            isActive = true;
            Item.SetActive(true);
            LeanTween.moveY(gameObject, 0f, _appearSeconds).setEase(_disappearEaseType);
        }
        else
        {
            isActive = false;
            LeanTween.moveY(gameObject, -Screen.height, _disappearSeconds).setEase(_disappearEaseType);
        }
        
    }
}
