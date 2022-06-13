using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class GalleryLoader : MonoBehaviour
{
    private int loadIndex = 0;
    private string Word;
    private int ColorNr;
    private Vector3[] LinePositions;
    public List<GameObject> BoundingBoxes;
    public string[] files;
    List<string> jsonfiles;
    public bool loadGallery = false;




    // Start is called before the first frame update
    void Start()
    {
         foreach (Transform child in transform)
         {
             if (child.name.Contains("bb_"))
             {
                 BoundingBoxes.Add(child.gameObject);
             }
         }
        files = System.IO.Directory.GetFiles(Application.persistentDataPath);
        jsonfiles = new List<string>();
        foreach (var file in files)
        {
            if (file.Contains(".json") && !file.Contains("Head"))
            {
                jsonfiles.Add(file);
            }
        }
       

    }

    private void Update()
    {
        if (loadGallery)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Contains("bb_"))
                {
                    BoundingBoxes.Add(child.gameObject);
                }
            }
            files = System.IO.Directory.GetFiles(Application.persistentDataPath);
            jsonfiles = new List<string>();
            foreach (var file in files)
            {
                if (file.Contains(".json") && !file.Contains("Head"))
                {
                    jsonfiles.Add(file);
                }
            }
            loadGallery = false;
        }
        if (loadIndex < jsonfiles.Count - 1)
        {
            Load();
            loadIndex++;
        }
    }


    void Load()
    {
        //var transform = GameObject.Find("Drawing").transform;
        //if (transform.childCount > 0)
        //{
        //    foreach (Transform child in transform)
        //    {
        //        GameObject.Destroy(child.gameObject);
        //    }
        //}
        //Load Values

        //string path = Application.persistentDataPath + "/LineSave.json";
        //string jsonString = File.ReadAllText(path);
        string path = files[loadIndex];
        string jsonString = File.ReadAllText(path);
        



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
            
            lineGameObject.transform.localScale *= 0.03f;
            lineGameObject.transform.position = BoundingBoxes[loadIndex].transform.position;
            //lineGameObject.transform.position += new Vector3(-0.2f * 0.03f, -1.5f*0.03f, -0.2f * 0.03f);
            lineGameObject.transform.position += new Vector3(+1f * 0.03f, -0.02f, 1f * 0.03f);
            lineGameObject.transform.SetParent(BoundingBoxes[loadIndex].transform);
            var currentLine = lineGameObject.AddComponent<LineRenderer>();
            currentLine.useWorldSpace = false;
            currentLine.positionCount = positionCount;
            currentLine.SetPositions(LinePositions);
            currentLine.material = Resources.Load<Material>("Materials/" + ColorNr);
            currentLine.startWidth = width * 0.03f;
            // Debug.Log(LineJson.ToString());
        }



    }

   public void LoadItem(int index)
    {
        var transform = GameObject.Find("GallerySpawner").transform;
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        //Load Values

        //string path = Application.persistentDataPath + "/LineSave.json";
        //string jsonString = File.ReadAllText(path);
        string path = files[index];
        string jsonString = File.ReadAllText(path);


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
            lineGameObject.transform.position = GameObject.Find("GallerySpawner").transform.position;
            lineGameObject.transform.SetParent(GameObject.Find("GallerySpawner").transform);
            var currentLine = lineGameObject.AddComponent<LineRenderer>();
            currentLine.useWorldSpace = false;
            currentLine.positionCount = positionCount;
            currentLine.SetPositions(LinePositions);
            currentLine.material = Resources.Load<Material>("Materials/" + ColorNr);
            currentLine.startWidth = width;

            // Debug.Log(LineJson.ToString());
        }



    }

   
}
