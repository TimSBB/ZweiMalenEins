using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    public Transform[] SpawnPositions;
    private int spawnIndex;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (spawnIndex >= SpawnPositions.Length) spawnIndex = 0;
        Vector3 position = SpawnPositions[spawnIndex].transform.position;
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", position, transform.rotation);
        spawnIndex++;

    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
        if (spawnIndex != 0) spawnIndex--;
    }
}
