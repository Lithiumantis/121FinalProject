using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private int hundredths = 0;
    private int seconds = 0;
    private int minutes = 0;

    [System.NonSerialized]
    public bool paused;
    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HundredthTick());
    }

    // Update is called once per frame
    void Update()
    {
        if(hundredths >= 99)
        {
            seconds++;
            hundredths = 0;
        }
        if (seconds >= 60)
        {
            minutes++;
            seconds = 0;
        }

        string minuteString = (minutes > 9) ? minutes.ToString() : "0" + minutes.ToString();
        string secondString = (seconds > 9) ? seconds.ToString() : "0" + seconds.ToString();
        string hundredthsString = (hundredths > 9) ? hundredths.ToString() : "0" + hundredths.ToString();

        timerText.text = minuteString + ":" + secondString + ":" + hundredthsString;
    }

    private IEnumerator HundredthTick()
    {
        yield return new WaitForSeconds(0.01f);
        if(!paused)
            hundredths++;
        StartCoroutine(HundredthTick());
    }
}
