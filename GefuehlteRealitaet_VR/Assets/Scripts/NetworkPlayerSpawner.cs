using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    public int ColorNr;
    public Vector3[] LinePositions;
    private string path;


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player 2", transform.position, transform.rotation);
            if (SceneManager.GetActiveScene().name == "Remote_Draw")
            {
                Loadplayer2();
            }

        }
        else { 
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);
            if (SceneManager.GetActiveScene().name == "Remote_Draw")
            {
                Loadplayer1();
            }

        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }


    public void Loadplayer1()
    {
        //Load Values
        string path = Application.persistentDataPath + "/Head_1_LineSave.json";
        string jsonString = File.ReadAllText(path);
        JSONObject lines = (JSONObject)JSON.Parse(jsonString);
        int LineCount = lines["LineCount"];
        //Debug.Log("linecount to load: " + LineCount);
        for (int i = 0; i < LineCount; i++)
        {


            var LineJson = lines["lineJson" + i.ToString()];
            //Set Values
            ColorNr = LineJson["ColorNr"];
            var positionCount = LineJson["PositionCount"];
            Debug.Log("PositionCount" + i + ":" + positionCount);
            LinePositions = new Vector3[positionCount];
            for (int j = 0; j < positionCount; j++)
            {
                //Debug.Log("iteration"+j);
                LinePositions[j] = new Vector3(LineJson["Position" + j.ToString()].AsArray[0], LineJson["Position" + j.ToString()].AsArray[1], LineJson["Position" + j.ToString()].AsArray[2]);
                // Debug.Log("Position"+j+"  " +LinePositions[j]);
            }

            GameObject lineGameObject = new GameObject("Line");
            lineGameObject.transform.SetParent(GameObject.Find("Drawing").transform);
            var currentLine = lineGameObject.AddComponent<LineRenderer>();
            currentLine.useWorldSpace = false;
            currentLine.positionCount = positionCount;
            currentLine.SetPositions(LinePositions);
            currentLine.material = Resources.Load<Material>("Materials/" + ColorNr);
            currentLine.startWidth = 0.03f;
        }
    }

    public void Loadplayer2()
    {
        //Load Values
        string path = Application.persistentDataPath + "/Head_2_LineSave.json";
        string jsonString = File.ReadAllText(path);
        JSONObject lines = (JSONObject)JSON.Parse(jsonString);
        int LineCount = lines["LineCount"];
        //Debug.Log("linecount to load: " + LineCount);
        for (int i = 0; i < LineCount; i++)
        {


            var LineJson = lines["lineJson" + i.ToString()];
            //Set Values
            ColorNr = LineJson["ColorNr"];
            var positionCount = LineJson["PositionCount"];
            Debug.Log("PositionCount" + i + ":" + positionCount);
            LinePositions = new Vector3[positionCount];
            for (int j = 0; j < positionCount; j++)
            {
                //Debug.Log("iteration"+j);
                LinePositions[j] = new Vector3(LineJson["Position" + j.ToString()].AsArray[0], LineJson["Position" + j.ToString()].AsArray[1], LineJson["Position" + j.ToString()].AsArray[2]);
                // Debug.Log("Position"+j+"  " +LinePositions[j]);
            }

            GameObject lineGameObject = new GameObject("Line");
            lineGameObject.transform.SetParent(GameObject.Find("Drawing").transform);
            var currentLine = lineGameObject.AddComponent<LineRenderer>();
            currentLine.useWorldSpace = false;
            currentLine.positionCount = positionCount;
            currentLine.SetPositions(LinePositions);
            currentLine.material = Resources.Load<Material>("Materials/" + ColorNr);
            currentLine.startWidth = 0.03f;
        }
    }
}
