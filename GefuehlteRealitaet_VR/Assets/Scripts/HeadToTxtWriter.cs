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
    private bool wroteOwnHead;
    private bool wroteOtherHead;

    private bool gotHeadPos1;
    private bool gotHeadPos2;
    private bool scene2Set;
    public resetPos resetPosScript; 
    private Vector3 headRohlingPos1;
    private Vector3 headRohlingPos2;


    private List<GameObject> myLines;
    private PhotonView PV;
    private Draw SetNetworkDraw;

    public void Save()
    {
        myLines = new List<GameObject>();
       // var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Line");
        var transform = GameObject.Find("Drawing").transform;
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                myLines.Add(child.gameObject);
            }
        }
        JSONObject lines = new JSONObject();
        //Debug.Log("LineCount: " + objects.Count());
        lines.Add("LineCount", transform.childCount);
        int j = 0;
        foreach (var gameobj in myLines)
        {
            var count = gameobj.GetComponent<LineRenderer>().positionCount;
            LinePositions = new Vector3[count];
            gameobj.GetComponent<LineRenderer>().GetPositions(LinePositions);
            //Debug.Log("Positions: " + LinePositions);

            JSONObject lineJson = new JSONObject();

            ColorNr = int.Parse(gameobj.GetComponent<LineRenderer>().material.name.Replace(" (Instance)", ""));
            lineJson.Add("ColorNr", ColorNr);
            var widthOfLine = gameobj.GetComponent<LineRenderer>().startWidth;
            lineJson.Add("lineWidth", widthOfLine);
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
        string jsonString = lines.ToString();
        File.WriteAllText(path, jsonString);
        var bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
        PV.RPC("RPC_SendOwnHead", RpcTarget.AllBufferedViaServer, playerNr, bytes);
    }

    public void Load()
    {
        //Load Values
        if (playerNr == 1)
        {
            path = Application.persistentDataPath + "/Head_2_LineSave.json";
        }
        if (playerNr == 2)
        {
            path = Application.persistentDataPath + "/Head_1_LineSave.json";
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
           // Debug.Log("PositionCount"+i+":" + positionCount);
            LinePositions = new Vector3[positionCount];
            for (int j = 0; j < positionCount; j++)
            {
                //Debug.Log("iteration"+j);
                LinePositions[j] = new Vector3(LineJson["Position" + j.ToString()].AsArray[0], LineJson["Position" + j.ToString()].AsArray[1], LineJson["Position" + j.ToString()].AsArray[2]);
               // Debug.Log("Position"+j+"  " +LinePositions[j]);
            }

            GameObject lineGameObject = new GameObject("Line");
            if (playerNr == 1)
            {
                lineGameObject.transform.position = lineGameObject.transform.position - headRohlingPos1 + new Vector3(-1.7f, 0, 0);
                lineGameObject.transform.SetParent(GameObject.Find("Network Player 2(Clone)").transform.Find("Head").transform.Find("Head").transform.Find("drawingHolder"));
                
            }
            if (playerNr == 2)
            {
                lineGameObject.transform.position = lineGameObject.transform.position - headRohlingPos2 + new Vector3(1.7f, 0, 0);
                lineGameObject.transform.SetParent(GameObject.Find("Network Player(Clone)").transform.Find("Head").transform.Find("Head").transform.Find("drawingHolder"));

            }
           
            var currentLine = lineGameObject.AddComponent<LineRenderer>();
            currentLine.useWorldSpace = false;
            currentLine.positionCount = positionCount;
            currentLine.SetPositions(LinePositions);
            currentLine.material = Resources.Load<Material>("Materials/" + ColorNr);
            currentLine.startWidth = LineJson["lineWidth"] * 0.7f;

           // Debug.Log(LineJson.ToString());

        }
        GameObject.Find("Network Player 2(Clone)").transform.Find("Head").transform.Find("Head").transform.Find("drawingHolder").transform.Rotate(0, 0, 225, Space.Self);
        GameObject.Find("Network Player(Clone)").transform.Find("Head").transform.Find("Head").transform.Find("drawingHolder").transform.Rotate(0, 0, 135, Space.Self);
        GameObject.Find("Network Player 2(Clone)").transform.Find("Head").transform.Find("Head").transform.localScale *= 0.7f;
        GameObject.Find("Network Player(Clone)").transform.Find("Head").transform.Find("Head").transform.localScale *= 0.7f;
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = this.GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        scene2Set = resetPosScript.scene2Set;
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        if (playerNr == 1 && !gotHeadPos1)
        {
            headRohlingPos1 = GameObject.Find("CharacterEditor_Scene_player1(Clone)").transform.Find("Head").transform.position;
            gotHeadPos1 = true;
        }
        if (playerNr == 2 && !gotHeadPos2 && scene2Set)
        {
            headRohlingPos2 = GameObject.Find("CharacterEditor_Scene_player2(Clone)").transform.Find("Head").transform.position;
            gotHeadPos2 = true;
        }
    }


    //void RPC_SendOwnHead(int playerNumber, byte[] headJson)
    [PunRPC]
    void RPC_SendOwnHead(int playerNumber, byte[] headJson)
    {
        Debug.Log("triggered the fucking rpc!");
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
            File.WriteAllText(path, System.Text.Encoding.UTF8.GetString(headJson));
            wroteOtherHead = true;

        }
        else
        {
            if (playerNr == 1)
            {
                path = Application.persistentDataPath + "/Head_1_LineSave.json";
            }
            if (playerNr == 2)
            {
                path = Application.persistentDataPath + "/Head_2_LineSave.json";
            }
            File.WriteAllText(path, System.Text.Encoding.UTF8.GetString(headJson));
            wroteOwnHead = true;
        }
        if (wroteOwnHead && wroteOtherHead)
        {
            NetworkPlayer[] networkplayers = (NetworkPlayer[])GameObject.FindObjectsOfType(typeof(NetworkPlayer));
            foreach (NetworkPlayer networkplayerScript in networkplayers)
            {
                networkplayerScript.GetComponent<NetworkPlayer>().showOtherPlayer = true;
            }
            // load headDrawings to Head of Networkplayer Prefab
            Load();

            //delete what has been drawing while waiting for the other player
            var transform = GameObject.Find("Drawing").transform;
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            }
            //allow to draw the lines of the other network player
            SetNetworkDraw = GameObject.Find("RightHand Controller").GetComponent<Draw>();
            SetNetworkDraw.SetNetworkDrawing();
        }

    }
}
