using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PickedWordsController : MonoBehaviour
{

    private PhotonView PV;
    private int playerNr;
    private bool rpcTriggered;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        GameController.current.onWordLogIn += OnLogInFeedback;
    }



    private void OnLogInFeedback(int playerNumber, string word, string label)
    {
            if (PV.IsMine && word == this.gameObject.name) { 
                PV.RPC("RPC_SetLoggedWord", RpcTarget.AllBufferedViaServer, word, label, playerNumber);
            }
    }

    [PunRPC]
    void RPC_SetLoggedWord(string word, string label, int playerNumber)
    {   
        print("RPC Function got triggered");
        var wordObject = GameObject.Find(word);
        var wordsLabeled = new List<GameObject>();
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == word);
        foreach (var gameObj in objects)
        {
            if (gameObj.tag == label)
            {
                wordsLabeled.Add(gameObj);
                print("count of labelled words " + wordsLabeled.Count);
            }
        }




        if (wordObject.tag == label  && wordsLabeled.Count ==2)
        {
            print("Both Words were logged in at the same label!!!");
            print("scale now!!!");
            wordObject.transform.localScale *= 4f;
        }
    }

    private void OnDestroy()
    {
        GameController.current.onWordLogIn -= OnLogInFeedback;
    }
}
