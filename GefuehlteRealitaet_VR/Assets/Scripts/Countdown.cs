using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Countdown : MonoBehaviour
{
    public float timeRemaining = 30;
    public bool timerIsRunning = false;
    private float initalRemaining;
    private bool reset = false;
    private float blockTimer = 1;
    
    public GameObject timeText;
    public Draw drawScript;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = false;
        initalRemaining = timeRemaining;
    }

    void Update()
    {
        if (reset)
        {
            timeRemaining = initalRemaining;
            DisplayTime(timeRemaining);
            reset = false;
            timerIsRunning = false;
            blockTimer = 1;
        }

        if (blockTimer > 0)
        {
            blockTimer -= 1 * Time.deltaTime;
        }
        if (blockTimer <= 0) { 
            if(drawScript.isDrawing || drawScript.OtherisDrawing)
            {
                timerIsRunning = true;
            }
            if (!drawScript.isDrawing && !drawScript.OtherisDrawing)
            {
                timerIsRunning = false;
            }
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= 1 * Time.deltaTime;
                    //print(timeRemaining);
                    DisplayTime(timeRemaining);
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    timerIsRunning = false;
                }
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        TextMeshProUGUI textmeshPro = timeText.GetComponent<TextMeshProUGUI>();
        textmeshPro.SetText(string.Format("{0:N2}", timeToDisplay));
    }

    public void ResetTimer()
    {
        reset = true;
    }
}