using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Linq;

public class Countdown : MonoBehaviour
{
    public float timeRemaining = 30;
    public bool timerIsRunning = false;
    private float initalRemaining;
    private bool reset = false;
    private float blockTimer = 1;
    
    public GameObject timeText;
    public Draw drawScript;
    public RandomWord randomWordScript;

    private PhotonView PV;
    private int playerNr;

    private int index;
    public GameObject wordText;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        timerIsRunning = false;
        initalRemaining = timeRemaining;
    }

    void Update()
    {
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
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
                    //Debug.Log("Time has run out!");
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
        PV.RPC("RPC_Reset", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void RPC_Reset()
    {
        reset = true;
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Line");
        foreach (var gameObj in objects)
        {
            Destroy(gameObj);
        }
        if (playerNr == 1)
        {
            index = Random.Range(0, randomWordScript.words.Length);
            var currentWord = randomWordScript.words[index];

            PV.RPC("RPC_RandomWord", RpcTarget.AllBufferedViaServer, currentWord);
            
        }
    }

    [PunRPC]
    void RPC_RandomWord(string word)
    {
        TextMeshProUGUI textmeshPro = wordText.GetComponent<TextMeshProUGUI>();
        textmeshPro.SetText(word);
    }


}