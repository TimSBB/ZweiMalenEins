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

    public event Action<int, string, string> onWordLogIn;
    public void wordLogIn(int playerNumber, string word, string label)
    {
        if (onWordLogIn != null)
        {
            onWordLogIn(playerNumber, word, label);
        }

    }

    public event Action<int, string, string> onWordLogOut;
    public void wordLogOut(int playerNumber, string word, string label)
    {
        if (onWordLogOut != null)
        {
            onWordLogOut(playerNumber, word, label);
        }

    }



}
