using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PickedWordsController : MonoBehaviour
{
    public int raycount = 6;
    private PhotonView PV;
    private int playerNr;
    private bool wordsLogged;
    private List<object[]> allLabeled;
    private List<string> BlockedRays;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        allLabeled = new List<object[]>();
        BlockedRays = new List<string>();
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
            if ((string)checkObject[1] == label) { 
                if ((int)checkObject[2] != playerNumber)
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
                else print("already labeled by that player!");
            }
            else
            {
                allLabeled[0] = new object[]
                   {
                            gameObj,
                            label,
                            playerNumber
                   };
                print("count of " + label + "-labelled words " + allLabeled.Count);
            }
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

       if (allLabeled.Count ==2 && !wordsLogged)
        {
            print("Both Words were logged in at the same label!!!");
            print("scale now!!!");
            GameObject.Find(word).transform.localScale *= 4f;
            var labeledObject = allLabeled[0];
            var BlockedRay = (string)labeledObject[1];
            BlockedRays.Add(BlockedRay);
            if (BlockedRays.Count == raycount)
            {
                print("let the bubble burst!!!");
                GameObject.Find("Floor").GetComponent<Renderer>().material.SetColor("Color_", new Color(255f, 255f, 255f));
            }
            wordsLogged = true;
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

        for (int i = 0; i< allLabeled.Count;  i++) 
        {
            var item = allLabeled[i];
            if ((int)item[2] == playerNumber)
            {
                if (allLabeled.Count == 2)
                {
                GameObject.Find(word).transform.localScale *= 0.25f;
                    wordsLogged = false;
                }
                allLabeled.Remove(item);
                BlockedRays.Remove((string)item[1]);
            }
        }
        

    }












    private void OnDestroy()
    {
        GameController.current.onWordLogIn -= OnLogInFeedback;
        GameController.current.onWordLogOut -= OnLogOutFeedback;
    }
}
