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

    //will be called when ready button is pressed >> see onClick Event on button Game Object
    public void changeSceneElems()
    {
        LoadingOverlay overlay = GameObject.Find("LoadingOverlay").gameObject.GetComponent<LoadingOverlay>();
       // overlay.FadeOut();
        //Write the drawing to your local file and in the end trigger RPC event to send via Network
        //!!!! this is also where the players ar made visible !!!!!
        WriteHead = GameObject.Find("Head_TextWriter").GetComponent<HeadToTxtWriter>();
        WriteHead.Save();

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
        //PhotonNetwork.Instantiate("AnweisungWortStatusbar", new Vector3(0, 2.217f, 1.5f), Quaternion.identity);


        //var scene = GameObject.Find("CharacterEditor_Scene");
        //Destroy(scene);

        var transform = GameObject.Find("Drawing").transform;
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        //var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Line");
        //foreach (var obj in objects)
        //{
        //    Destroy(obj);
        //}

       // overlay.FadeIn();
    }


}
