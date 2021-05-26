using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LabelController : MonoBehaviour
{

    private string LabelTag;
    public int playerNumber;
    private bool controllersLeftTrigger = false;
    private Collider[] controllerColliders;

    private void Start()
    {
        LabelTag = gameObject.tag;
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == "xrRig")
        {
            controllersLeftTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (gameObject.tag == other.tag) {
            GameController.current.ObjectTriggerEnter(LabelTag, playerNumber);
        }

        //if (gameObject.tag == other.tag && controllersLeftTrigger)
        //{
        //    GameController.current.ObjectTriggerEnter(LabelTag, playerNumber);
        //}

    }
}
