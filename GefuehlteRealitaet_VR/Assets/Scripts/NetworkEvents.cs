using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkEvents : MonoBehaviourPun
{
    private const byte WORD_LOGGED_IN_EVENT = 0;
    private List<string> BlockedRays;
    public int raycount;
    // Start is called before the first frame update
    void Start()
    {
        //GameController.current.onSameWordsLogged += OnSameWords;
        PhotonNetwork.NetworkingClient.EventReceived += WordLogIn_EventReceived;
        BlockedRays = new List<string>();
    }

    private void WordLogIn_EventReceived(ExitGames.Client.Photon.EventData obj)
    {
        if (obj.Code == WORD_LOGGED_IN_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            int playerNr = (int)datas[0];
            string word = (string)datas[1];
            string label = (string)datas[2];
            print("Network Event Fired with Player " + playerNr + " and Word " + word + " and label " + label);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 

    //private void OnSameWords(string BlockedRay)
    //{
    //    if (base.photonView.IsMine) { 
    //        BlockedRays.Add(BlockedRay);
    //        var blockedcount = BlockedRays.Count;
    //        print(" BlockedRays.Count:  " +blockedcount);
    //        if (BlockedRays.Count == raycount)
    //        {
    //            print("let the bubble burst!!!");
    //            GameObject.Find("Floor").GetComponent<Renderer>().material.SetColor("Color_", new Color(255f, 255f, 255f));
    //        }
    //       // PhotonNetwork.RaiseEvent(WORD_LOGGED_IN_EVENT, blockedcount, RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendUnreliable);
    //    }
    //}


    private void OnDestroy()
    {
        //GameController.current.onSameWordsLogged -= OnSameWords;
        PhotonNetwork.NetworkingClient.EventReceived -= WordLogIn_EventReceived;
    }
}
