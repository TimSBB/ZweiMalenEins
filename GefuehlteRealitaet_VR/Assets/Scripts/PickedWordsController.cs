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
    private List<object[]> allLabeled;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        allLabeled = new List<object[]>();
        GameController.current.onWordLogIn += OnLogInFeedback;
        GameController.current.onWordLogOut += OnLogOutFeedback;
    }



    private void OnLogInFeedback(int playerNumber, string word, string label)
    {
        //if (PV.IsMine && word == this.gameObject.name) { 
        if (word == this.gameObject.name){
            PV.RPC("RPC_SetLoggedWord", RpcTarget.AllBufferedViaServer, word, label, playerNumber);
        }
    }

    [PunRPC]
    void RPC_SetLoggedWord(string word, string label, int playerNumber)
    {   
        print("RPC LogInFunction got triggered");

        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == word);
        var gameObj = GameObject.Find(word);

        if (allLabeled.Count == 1)
            {
                var checkObject = allLabeled[0];
                if ((int)checkObject[2] != playerNumber)
                {
                    var labeledObject = new object[]
                    {
                        gameObj,
                        label,
                        playerNumber
                    };
                    allLabeled.Add(labeledObject);
                    print("count of "+ label + "-labelled words " + allLabeled.Count);
                }
                else if((string)checkObject[1] != label)
                {
                    allLabeled[0] = new object[]
                    {
                        gameObj,
                        label,
                        playerNumber
                    };
                    print("count of " + label + "-labelled words " + allLabeled.Count);
                } else print("already labeled by that player!");
            }

            if (allLabeled.Count < 1)
            {
                var labeledObject = new object[]
                {
                    gameObj,
                    label,
                    playerNumber
                };
                allLabeled.Add(labeledObject);
                print("count of " + label + "-labelled words " + allLabeled.Count);
            }
            

       // }

       if (allLabeled.Count ==2)
        {
            print("Both Words were logged in at the same label!!!");
            print("scale now!!!");
            GameObject.Find(word).transform.localScale *= 4f;

        }
    }






    private void OnLogOutFeedback(int playerNumber, string word, string label)
    {
        //if (PV.IsMine && word == this.gameObject.name) { 
        if (word == this.gameObject.name)
        {
            PV.RPC("RPC_SetLoggedWordOut", RpcTarget.AllBufferedViaServer, word, label, playerNumber);
        }
    }

    [PunRPC]
    void RPC_SetLoggedWordOut(string word, string label, int playerNumber)
    {
        print("RPC LogOutFunction got triggered");

        foreach (var item in allLabeled)
        {
            if ((int)item[2] == playerNumber)
            {
                GameObject.Find(word).transform.localScale *= 0.25f;
                allLabeled.Remove(item);
            }

        }
        

    }












    private void OnDestroy()
    {
        GameController.current.onWordLogIn -= OnLogInFeedback;
        GameController.current.onWordLogOut -= OnLogOutFeedback;
    }
}
