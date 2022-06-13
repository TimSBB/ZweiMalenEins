using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Linq;

public class ColliderTrail : MonoBehaviour
{
    public GameObject trailObject;
    public float newObjectDistance = 1.0f;
    public float sizeRatio = 1.0f;

    private Vector3 currentObjectStartPosition;
    private GameObject currentObject;
    private int ColorNr;
    private Vector3[] LinePositions;
    private int index = 0;

    private void Start()
    {
        Load();
    }
    // Update is called once per frame
    void Update()
    {
        if (index < LinePositions.Length - 1)
        {
            //Debug.Log("LinePositions.Length " + LinePositions.Length);
            if (currentObject == null)
            {
                currentObject = Instantiate(trailObject, LinePositions[index], Quaternion.identity) as GameObject;
                currentObjectStartPosition = LinePositions[index];
            }
            else
            {
               
                Vector3 toTransformVector = LinePositions[index] - currentObjectStartPosition;

                currentObject.transform.rotation = Quaternion.LookRotation(toTransformVector);

                Vector3 newScale = currentObject.transform.localScale;
                float distance = toTransformVector.magnitude;
                newScale.z = distance * sizeRatio;
                currentObject.transform.localScale = newScale;

                if (distance >= newObjectDistance)
                {
                    Debug.Log("index " + index);
                    currentObject = Instantiate(trailObject, LinePositions[index], Quaternion.identity) as GameObject;
                    currentObjectStartPosition = LinePositions[index];
                }
            }
            index++;

        }


    }

    void Load()
    {

        string path = Application.persistentDataPath + "/20210624_01_08_GalleryTest.json";
        string jsonString = File.ReadAllText(path);




        JSONObject lines = (JSONObject)JSON.Parse(jsonString);
        int LineCount = lines["LineCount"];
        for (int i = 0; i < LineCount; i++)
        {
            var LineJson = lines["lineJson" + i.ToString()];
            //Set Values
            ColorNr = LineJson["ColorNr"];
            var width = LineJson["lineWidth"];
            var positionCount = LineJson["PositionCount"];
            LinePositions = new Vector3[positionCount];
            for (int j = 0; j < positionCount; j++)
            {
                LinePositions[j] = new Vector3(LineJson["Position" + j.ToString()].AsArray[0], LineJson["Position" + j.ToString()].AsArray[1], LineJson["Position" + j.ToString()].AsArray[2]);
            }

            GameObject lineGameObject = new GameObject("Line");

            lineGameObject.transform.localScale *= 0.05f;
            lineGameObject.transform.position = GameObject.Find("Drawing").transform.position;
            lineGameObject.transform.SetParent(GameObject.Find("Drawing").transform);
            var currentLine = lineGameObject.AddComponent<LineRenderer>();
            currentLine.useWorldSpace = false;
            currentLine.positionCount = positionCount;
            currentLine.SetPositions(LinePositions);
            currentLine.material = Resources.Load<Material>("Materials/" + ColorNr);
            currentLine.startWidth = width * 0.05f;
            // Debug.Log(LineJson.ToString());
        }



    }
}
