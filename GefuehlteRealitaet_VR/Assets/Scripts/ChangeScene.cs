using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Photon.Pun;

public class ChangeScene : MonoBehaviour
{
    private HeadToTxtWriter WriteHead;
    private int playerNr;
    public GameObject ColorPicker;
    public GameObject AnweisungWortStatusbar;
    private bool triggeredFade;
    private LoadingOverlay overlay;
    private HeadToTxtWriter headWriter;

    //will be called when ready button is pressed >> see onClick Event on button Game Object
    public void changeSceneElems()
    {
        overlay = GameObject.Find("LoadingOverlay").gameObject.GetComponent<LoadingOverlay>();
        overlay.FadeIn();
        //Write the drawing to your local file and in the end trigger RPC event to send via Network
        //!!!! this is also where the players ar made visible !!!!!
        WriteHead = GameObject.Find("Head_TextWriter").GetComponent<HeadToTxtWriter>();
        WriteHead.Save();
        headWriter.loadOwnHead();
       
        triggeredFade = true;
    }

    public void Start()
    {
        overlay = GameObject.Find("LoadingOverlay").gameObject.GetComponent<LoadingOverlay>();
        headWriter = GameObject.Find("Head_TextWriter").GetComponent<HeadToTxtWriter>();
    }

    public void Update()
    {
        if (!overlay.fading && triggeredFade)
        {
            Debug.Log("should fade in now");
            triggeredFade = false;
            GameObject.Find("RightHand Controller").GetComponent<Draw>().nextScene = true;
            GameObject.Find("RightHand Controller").GetComponent<Draw>().allowDraw = false;

            playerNr = PhotonNetwork.LocalPlayer.ActorNumber;

            if (playerNr == 1)
            {
                GameObject.Find("CharacterEditor_Scene_player1(Clone)").SetActive(false);
            }
            if (playerNr == 2)
            {
                GameObject.Find("CharacterEditor_Scene_player2(Clone)").SetActive(false);
            }

            Instantiate(ColorPicker);
            Instantiate(AnweisungWortStatusbar);


            var transform = GameObject.Find("Drawing").transform;
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            }
            overlay.FadeOut();
        }
    }
}
