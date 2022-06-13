using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Linq;


public class Judithy_Line_Loader : MonoBehaviour
{
    private string Word;
    //public RandomWord randomWordScript;
    private int ColorNr;
    private Vector3[] LinePositions;
    private int PositionCount;
    private float widthOfLine;

    void Load()
    {
        //Load Values
        var text = (TextAsset)Resources.Load("baum 1");
        string jsonString = text.text;
        JSONObject lines = (JSONObject)JSON.Parse(jsonString);
        int LineCount = lines["LineCount"];
        //Debug.Log("linecount to load: " + LineCount);
        for (int i = 0; i < LineCount; i++)
        {
            

            var LineJson = lines["lineJson" + i.ToString()];
            //Set Values
           // Word = LineJson["Word"];
            ColorNr = LineJson["ColorNr"];
            var width = LineJson["lineWidth"];
            var positionCount = LineJson["PositionCount"];
            //Debug.Log("PositionCount"+i+":" + positionCount);
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
            currentLine.startWidth = width;

           // Debug.Log(LineJson.ToString());

        }


      
    }

    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) Load();

    }
}
