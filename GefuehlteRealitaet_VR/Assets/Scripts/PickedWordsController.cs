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
                PV.RPC("RPC_SetLoggedWord", RpcTarget.Others, word, label);
            }
    }

    [PunRPC]
    void RPC_SetLoggedWord(string word, string label)
    {   
        print("RPC Function got triggered");
        var wordObject = GameObject.Find(word);

        if (wordObject.tag == label)
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
