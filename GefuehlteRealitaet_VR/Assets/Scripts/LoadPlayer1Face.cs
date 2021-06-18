using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class LoadPlayer1Face : MonoBehaviour
{
    public int ColorNr;
    public Vector3[] LinePositions;
    // Start is called before the first frame update
    void Start()
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

            // Debug.Log(LineJson.ToString());
        }

    }

}