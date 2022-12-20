using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text SecondsText;
    public Text MinutesText;

    public float Seconds;
    public float Minutes;



    void Start()
    {
        
    }

    
    void Update()
    {
        KeepTime();
        KeepText();

    }

    public void KeepText()
    {
        SecondsText.text = Seconds.ToString("00") + "sec";
        MinutesText.text = Minutes.ToString();

    }

    public void KeepTime()
    {
        Seconds += Time.deltaTime;

        if (Seconds >= 60)
        {
            Seconds = 0;
            Minutes += 1;
        }

    }
}
