using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{

    public static GameController current;


    private void Awake()
    {
        current = this;
    }


    public event Action<string, int> onObjectTriggerEnter;

    public void ObjectTriggerEnter(string LabelTag, int playerNumber)
    {
        if (onObjectTriggerEnter != null)
        {
            onObjectTriggerEnter(LabelTag, playerNumber);
        }

    }


}
