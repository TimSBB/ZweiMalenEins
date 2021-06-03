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


    public event Action<int, int, Transform> onObjectTriggerEnter;

    public void ObjectTriggerEnter(int labelNumber, int playerNumber, Transform OtherTransform)
    {
        if (onObjectTriggerEnter != null)
        {
            onObjectTriggerEnter(labelNumber, playerNumber, OtherTransform);
        }

    }

    public event Action<int, int> onObjectTriggerExit;
    public void ObjectTriggerExit(int labelNumber, int playerNumber)
    {
        if (onObjectTriggerExit != null)
        {
            onObjectTriggerExit(labelNumber, playerNumber);
        }

    }


}
