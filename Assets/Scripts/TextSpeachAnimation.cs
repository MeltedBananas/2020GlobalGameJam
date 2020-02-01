using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpeachAnimation : MonoBehaviour
{
    public delayBetweenLetters = 0.5;
    public StringToWrite = "Bonjour la maison est rouge";
    private OutputText = "";
    public MaxBubbleTextSize = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delayBetweenLetters -= Time.DeltaTime;
    }
}
