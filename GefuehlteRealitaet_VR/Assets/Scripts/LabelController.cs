using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LabelController : MonoBehaviour
{

    private string LabelTag;
    public int playerNumber;
    private bool ObjectStillLabeled = false;
    private Transform OtherTransform;
    private Collider[] controllerColliders;

    private void Start()
    {
        LabelTag = gameObject.tag;
    }

    private void OnTriggerExit(Collider other)
    {

            OtherTransform = other.transform;
            ObjectStillLabeled = false;
            GameController.current.ObjectTriggerEnter(playerNumber, ObjectStillLabeled, OtherTransform);

    }
    private void OnTriggerEnter(Collider other)
    {
            ObjectStillLabeled = true;
            OtherTransform = other.transform;
            GameController.current.ObjectTriggerEnter(playerNumber, ObjectStillLabeled, OtherTransform);
    }
}
