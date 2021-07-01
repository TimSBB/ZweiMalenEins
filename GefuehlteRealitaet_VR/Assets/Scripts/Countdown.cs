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
    
    private Draw drawScript;
    private HeadToTxtWriter headwriter;
    public RandomWord randomWordScript;

    private PhotonView PV;
    private int playerNr;

    private int index;
    public GameObject wordText;

    private Image statusBar;
    private float hue;
    private float H, S, V;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        timerIsRunning = false;
        initalRemaining = timeRemaining;
        statusBar = GameObject.Find("UIStatusBar").GetComponent<Image>();
        var color = statusBar.color;

        Color.RGBToHSV(statusBar.color, out H, out S, out V);
        hue = H;
        
        drawScript = GameObject.Find("RightHand Controller").GetComponent<Draw>();
        headwriter = GameObject.Find("Head_TextWriter").GetComponent<HeadToTxtWriter>();

    }

    void Update()
    {
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        if (reset)
        {
            timeRemaining = initalRemaining;
            //DisplayTime(timeRemaining);
            reset = false;
            timerIsRunning = false;
            blockTimer = 1;
        }

        if (blockTimer > 0 && headwriter.wroteOtherHead && headwriter.wroteOwnHead)
        {
            blockTimer -= 1 * Time.deltaTime;
        }
            if (blockTimer <= 0 )
            {
                if ((drawScript.isDrawing && drawScript.allowDraw) || (drawScript.OtherisDrawing && drawScript.OtherisDrawingAllowedToDraw))
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
                    statusBar.fillAmount = timeRemaining.Remap(0, initalRemaining, 0, 1);
                    H = hue.Remap(0, initalRemaining, 0.3464253f, 0.005555556f);
                    statusBar.color = Color.HSVToRGB(H, S, V);    
                    //DisplayTime(timeRemaining);
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

    //void DisplayTime(float timeToDisplay)
    //{
    //    TextMeshProUGUI textmeshPro = timeText.GetComponent<TextMeshProUGUI>();
    //    textmeshPro.SetText(string.Format("{0:N2}", timeToDisplay));
    //}

    public void ResetTimer()
    {
        PV.RPC("RPC_Reset", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void RPC_Reset()
    {
        reset = true;
        var transform = GameObject.Find("Drawing").transform;
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
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