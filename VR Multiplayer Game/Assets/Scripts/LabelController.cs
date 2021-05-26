using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LabelController : MonoBehaviour
{

    private string LabelTag;
    public int playerNumber;
    private bool ObjectStillLabeled = false;
    private Collider[] controllerColliders;

    private void Start()
    {
        LabelTag = gameObject.tag;
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag == other.tag)
        {
            ObjectStillLabeled = false;
            GameController.current.ObjectTriggerEnter(LabelTag, playerNumber, ObjectStillLabeled);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == other.tag) {
            ObjectStillLabeled = true;
            GameController.current.ObjectTriggerEnter(LabelTag, playerNumber, ObjectStillLabeled);
        }
    }
}
