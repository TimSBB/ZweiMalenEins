using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;


public class ToTxtWriter : MonoBehaviour
{
    public InputHelpers.Button saveInput;
    public InputHelpers.Button loadInput;
    private string Word;
    public RandomWord randomWordScript;
    private int ColorNr;
    private Vector3[] LinePositions;
    private int PositionCount;
    private float widthOfLine;
    private bool debug;
    private XRController controller;
    private int loadIndex = 0;
    private bool saved = false;
    private bool loaded = false;

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


    void Save()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Line");
        JSONObject lines = new JSONObject();
        //Debug.Log("LineCount: " + objects.Count());
        lines.Add("LineCount",objects.Count());
        int j = 0;
        foreach (var gameobj in objects)
        {
            if (gameobj.transform.parent.name == "Drawing")
            {
                var count = gameobj.GetComponent<LineRenderer>().positionCount;
                LinePositions = new Vector3[count];
                gameobj.GetComponent<LineRenderer>().GetPositions(LinePositions);
                //Debug.Log("Positions: " + LinePositions);

                JSONObject lineJson = new JSONObject();

                //Word = randomWordScript.currentWord;
                Word = "GalleryTest";
                lineJson.Add("Word", Word);
                var colorname = gameobj.GetComponent<LineRenderer>().material.name;
                if (colorname.Contains(" (Instance)"))
                {
                    ColorNr = int.Parse(colorname.Replace(" (Instance)", ""));
                }
                else
                {
                    ColorNr = int.Parse(colorname);
                }

                lineJson.Add("ColorNr", ColorNr);
                widthOfLine = gameobj.GetComponent<LineRenderer>().startWidth;
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
                lines.Add("lineJson" + j.ToString(), lineJson);
                j++;
            } 
        }
        //Save Json to Computer
        string path = Application.persistentDataPath + "/" + System.DateTime.Now.ToString("yyyyMMdd_hh_mm") + "_" + Word + ".json";
        //Debug.Log(lines.ToString());
        File.WriteAllText(path, lines.ToString());
        Debug.Log("saved json to file!!");
        Debug.Log("path: " + path);

    }

    void Load()
    {
        var transform = GameObject.Find("Drawing").transform;
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        //Load Values
        string[] files = System.IO.Directory.GetFiles(Application.persistentDataPath);
        List<string> jsonfiles = new List<string>();
        foreach (var file in files)
        {
            if (file.Contains(".json") && !file.Contains("Head"))
            {
                jsonfiles.Add(file);
            }
        }
        //string path = Application.persistentDataPath + "/LineSave.json";
        //string jsonString = File.ReadAllText(path);
        string path = files[loadIndex];
        string jsonString = File.ReadAllText(path);
        if (loadIndex < jsonfiles.Count - 1)
        {
            loadIndex++;
        }
        else loadIndex = 0;



        JSONObject lines = (JSONObject)JSON.Parse(jsonString);
        int LineCount = lines["LineCount"];
        for (int i = 0; i < LineCount; i++)
        {
            var LineJson = lines["lineJson" + i.ToString()];
            //Set Values
            Word = LineJson["Word"];
            ColorNr = LineJson["ColorNr"];
            var width = LineJson["lineWidth"];
            var positionCount = LineJson["PositionCount"];
            LinePositions = new Vector3[positionCount];
            for (int j = 0; j < positionCount; j++)
            {
                LinePositions[j] = new Vector3(LineJson["Position" + j.ToString()].AsArray[0], LineJson["Position" + j.ToString()].AsArray[1], LineJson["Position" + j.ToString()].AsArray[2]);
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
        // CreateText();
        string path = Application.persistentDataPath + "/LineSave.json";
        Debug.Log("path: " + path);
        controller = GameObject.Find("RightHand Controller").GetComponent<XRController>();
        //Debug.Log("dateTime: " + System.DateTime.Now.ToString("yyyyMMdd_hh_mm"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();
        if (Input.GetKeyDown(KeyCode.D))
        {
            debug = true;    
        }
        if (debug)
        {
            
            InputHelpers.IsPressed(controller.inputDevice, saveInput, out bool SaveisPressed, 0.9f);
            if (SaveisPressed && !saved)
            {
                saved = true;
                Debug.Log("Save is pressed!");
              
            }
            InputHelpers.IsPressed(controller.inputDevice, loadInput, out bool LoadisPressed, 0.9f);
            if (LoadisPressed) Debug.Log("Load is pressed!");
            if (LoadisPressed && !loaded)
            {
                loaded = true;
                //Load();

                Debug.Log("Load is pressed!");

   
            }
            //Debug.Log(controller);
        }


    }


}
