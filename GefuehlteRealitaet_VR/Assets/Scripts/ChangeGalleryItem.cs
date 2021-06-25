using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeGalleryItem : MonoBehaviour
{

    private int loadIndex = 0;
    private int ColorNr;
    private Vector3[] LinePositions;
    private string Word;
    private string[] files;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PaintCapsule")
        {
            if (this.gameObject.name.Contains("bb_0"))
            {
                loadIndex = int.Parse(this.gameObject.name.Replace("bb_0", "")) - 1;
            }
            else
            {
                loadIndex = int.Parse(this.gameObject.name.Replace("bb_", "")) - 1;
            }
             files = System.IO.Directory.GetFiles(Application.persistentDataPath);
            List<string> jsonfiles = new List<string>();
            foreach (var file in files)
            {
                if (file.Contains(".json") && !file.Contains("Head"))
                {
                    jsonfiles.Add(file);
                }
            }
            if(jsonfiles.Count > loadIndex)
            {
                Load();
            }

            other.SendMessageUpwards("SetGalleryItem", loadIndex, SendMessageOptions.DontRequireReceiver);

        }
    }


    void Load()
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
