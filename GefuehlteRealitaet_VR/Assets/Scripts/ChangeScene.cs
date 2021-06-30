using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ChangeScene : MonoBehaviour
{
    private HeadToTxtWriter WriteHead;


    //will be called when ready button is pressed >> see onClick Event on button Game Object
public void changeSceneElems()
    {
        //Write the drawing to your local file and in the end trigger RPC event to send via Network
        //!!!! this is also where the players ar made visible !!!!!
        WriteHead = GameObject.Find("Head_TextWriter").GetComponent<HeadToTxtWriter>();
        WriteHead.Save();

        GameObject.Find("RightHand Controller").GetComponent<Draw>().nextScene = true;
        GameObject.Find("RightHand Controller").GetComponent<Draw>().allowDraw = false;

        GameObject.Find("CharacterEditor_Button_Canvas").SetActive(false);
        GameObject.Find("Text_Character-Editor").SetActive(false);
        GameObject.Find("Button_Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Text_Countdown").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.Find("Text_Wort").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.Find("Text_Wort").GetComponent<RandomWord>().enabled = true;

        var scene = GameObject.Find("CharacterEditor_Scene");
        Destroy(scene);

        var transform = GameObject.Find("Drawing").transform;
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        //var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Line");
        //foreach (var obj in objects)
        //{
        //    Destroy(obj);
        //}
    }


}
