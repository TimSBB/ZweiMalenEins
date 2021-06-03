﻿using System;
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


    public event Action<int, int, Transform> onObjectRayEnter;

    public void ObjectRayEnter(int labelNumber, int playerNumber, Transform OtherTransform)
    {
        if (onObjectRayEnter != null)
        {
            onObjectRayEnter(labelNumber, playerNumber, OtherTransform);
        }

    }

    public event Action<int, int> onObjectRayExit;
    public void ObjectRayExit(int labelNumber, int playerNumber)
    {
        if (onObjectRayExit != null)
        {
            onObjectRayExit(labelNumber, playerNumber);
        }

    }

    public event Action<int, int> onWordLogIn;
    public void wordLogIn(int labelNumber, int playerNumber)
    {
        if (onWordLogIn != null)
        {
            onWordLogIn(labelNumber, playerNumber);
        }

    }



}
