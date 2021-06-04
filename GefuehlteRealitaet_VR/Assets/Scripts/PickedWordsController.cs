using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PickedWordsController : MonoBehaviour
{

    private PhotonView PV;
    private int playerNr;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        GameController.current.onWordLogIn += OnLogInFeedback;
    }



    private void OnLogInFeedback(int playerNumber, string word, string label)
    {
        if (PV.IsMine) { 
            PV.RPC("RPC_SetLoggedWord", RpcTarget.AllBufferedViaServer, word, label);
        }
    }

    [PunRPC]
    void RPC_SetLoggedWord(string word, string label)
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == word);
        var wordsLabeled = new List<GameObject>();
        foreach (var gameObj in objects)
        {
            if (gameObj.tag == label)
            {
                wordsLabeled.Add(gameObj);
            }
        }
        if (wordsLabeled.Count == 2)
        {
            print("Both Words were logged in at the same label!!!");
            foreach (var gameObj in wordsLabeled)
            {
                if(gameObj.GetComponent<BubbleID>().BubbleId == playerNr)
                {
                    print("scale now!!!");
                    gameObj.transform.localScale *= 4f;
                }

            }
        }
    }

    private void OnDestroy()
    {
        GameController.current.onWordLogIn -= OnLogInFeedback;
    }
}
