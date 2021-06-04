using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private object[] loggedWordInfo;
    private object[] datas;

// Start is called before the first frame update
void Start()
    {
        ConnectToServer();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            print("PlayerCount:  " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("PlayerID:  " + player.ActorNumber);
        }
        base.OnJoinedRoom();
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("PlayerNumber " + PhotonNetwork.PlayerList);
    }

}
