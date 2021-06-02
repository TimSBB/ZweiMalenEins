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


    public event Action<int, bool, Transform> onObjectTriggerEnter;

    public void ObjectTriggerEnter(int playerNumber, bool ObjectStillLabeled, Transform OtherTransform)
    {
        if (onObjectTriggerEnter != null)
        {
            onObjectTriggerEnter(playerNumber, ObjectStillLabeled, OtherTransform);
        }

    }


}
