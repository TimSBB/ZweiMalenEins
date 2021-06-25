using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ChangeScene : MonoBehaviour
{
    private Draw SetNetworkDraw;
    private HeadToTxtWriter WriteHead;


public void changeSceneElems()
    {
        WriteHead = GameObject.Find("Head_TextWriter").GetComponent<HeadToTxtWriter>();
        WriteHead.Save();
        SetNetworkDraw = GameObject.Find("RightHand Controller").GetComponent<Draw>();
        SetNetworkDraw.SetNetworkDrawing();
        GameObject.Find("CharacterEditor_Button_Canvas").SetActive(false);
        GameObject.Find("Text_Character-Editor").SetActive(false);
        GameObject.Find("Button_Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Text_Countdown").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.Find("Text_Wort").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.Find("Text_Wort").GetComponent<RandomWord>().enabled = true;

        var scene = GameObject.Find("CharacterEditor_Scene");
        Destroy(scene);

        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Line");
        foreach (var obj in objects)
        {
            Destroy(obj);
        }

    }
}
