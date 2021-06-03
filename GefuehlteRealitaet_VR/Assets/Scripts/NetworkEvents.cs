using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkEvents : MonoBehaviourPun
{
    private const byte WORD_LOGGED_IN_EVENT = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameController.current.onWordLogIn += OnLogInFeedback;
        PhotonNetwork.NetworkingClient.EventReceived += WordLogIn_EventReceived;
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

    void SendLoggedObject(int playerNumber, string word, string label)
    {
        object[] datas = new object[]
        {
            playerNumber,
            word,
            label
        };

        PhotonNetwork.RaiseEvent(WORD_LOGGED_IN_EVENT, datas,RaiseEventOptions.Default,ExitGames.Client.Photon.SendOptions.SendUnreliable);
    }

    private void OnLogInFeedback(int playerNumber, string word, string label)
    {
        if (base.photonView.IsMine) { 
        SendLoggedObject(playerNumber, word, label);
        }
    }


    private void OnDestroy()
    {
        GameController.current.onWordLogIn -= OnLogInFeedback;
        PhotonNetwork.NetworkingClient.EventReceived -= WordLogIn_EventReceived;
    }
}
