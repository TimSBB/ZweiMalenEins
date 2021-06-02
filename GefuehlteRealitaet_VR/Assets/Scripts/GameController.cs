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


    public event Action<int, int, bool, Transform> onObjectTriggerEnter;

    public void ObjectTriggerEnter(int labelNumber, int playerNumber, bool ObjectStillLabeled, Transform OtherTransform)
    {
        if (onObjectTriggerEnter != null)
        {
            onObjectTriggerEnter(labelNumber, playerNumber, ObjectStillLabeled, OtherTransform);
        }

    }


}
