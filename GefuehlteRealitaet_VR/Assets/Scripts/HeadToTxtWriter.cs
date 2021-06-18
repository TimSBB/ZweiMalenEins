using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Linq;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class HeadToTxtWriter : MonoBehaviour
{
    public int ColorNr;
    public Vector3[] LinePositions;
    private int PositionCount;
    private int playerNr;
    private string path;

    private PhotonView PV;

    //void CreateText()
    //{
    //    //Path to The File
    //    string path = Application.dataPath + "/Log.txt";
    //    //Create File if it doesn't exist
    //    if (!File.Exists(path))
    //    {
    //        File.WriteAllText(path, "Login log \n\n");
    //    }
    //    //Content
    //    string content = "Login date: " + System.DateTime.Now + "\n";
    //    //Add some text to it
    //    File.AppendAllText(path, content);
    //}


    public void Save()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Line");
        JSONObject lines = new JSONObject();
        //Debug.Log("LineCount: " + objects.Count());
        lines.Add("LineCount",objects.Count());
        int j = 0;
        foreach (var gameobj in objects)
        {
            
            var count = gameobj.GetComponent<LineRenderer>().positionCount;
            LinePositions = new Vector3[count];
            gameobj.GetComponent<LineRenderer>().GetPositions(LinePositions);
            //Debug.Log("Positions: " + LinePositions);

            JSONObject lineJson = new JSONObject();

            ColorNr = int.Parse(gameobj.GetComponent<LineRenderer>().material.name.Replace(" (Instance)", ""));
            lineJson.Add("ColorNr", ColorNr);

            lineJson.Add("PositionCount", LinePositions.Length);

            for (int i = 0; i < LinePositions.Length; i++)
            {
                JSONArray position = new JSONArray();
                position.Add(LinePositions[i].x);
                position.Add(LinePositions[i].y);
                position.Add(LinePositions[i].z);
                lineJson.Add("Position" + i.ToString(), position);
            }
          //  Debug.Log(lineJson.ToString());
            lines.Add("lineJson"+j.ToString(), lineJson);
            j++;
        }
        //Save Json to Computer
        if (playerNr == 1) {
            path = Application.persistentDataPath + "/Head_1_LineSave.json";
        }
        if (playerNr == 2)
        {
            path = Application.persistentDataPath + "/Head_2_LineSave.json";
        }
        //Debug.Log(lines.ToString());
        File.WriteAllText(path, lines.ToString());
        //Debug.Log("path: " + path);
        if (playerNr == 1)
        {
            path = Application.persistentDataPath + "/Head_1_LineSave.json";
        }
        if (playerNr == 2)
        {
            path = Application.persistentDataPath + "/Head_2_LineSave.json";
        }
        string jsonString = File.ReadAllText(path);
        PV.RPC("RPC_SendOwnHead", RpcTarget.AllBufferedViaServer, playerNr, jsonString);
    }

    public void Load()
    {
        //Load Values
        if (playerNr == 1)
        {
            path = Application.persistentDataPath + "/Head_1_LineSave.json";
        }
        if (playerNr == 2)
        {
            path = Application.persistentDataPath + "/Head_2_LineSave.json";
        }
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
            Debug.Log("PositionCount"+i+":" + positionCount);
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

           // Debug.Log(LineJson.ToString());

        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        // CreateText();
    }

    // Update is called once per frame
    void Update()
    {
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();

    }



    [PunRPC]
    void RPC_SendOwnHead(int playerNumber, string headJson)
    {
        if (playerNumber != playerNr)
        {
            if (playerNr == 1)
            {
                path = Application.persistentDataPath + "/Head_2_LineSave.json";
            }
            if (playerNr == 2)
            {
                path = Application.persistentDataPath + "/Head_1_LineSave.json";
            }
            File.WriteAllText(path, headJson.ToString());
        }
        SceneManager.LoadScene("Remote_Draw");
    }
}
