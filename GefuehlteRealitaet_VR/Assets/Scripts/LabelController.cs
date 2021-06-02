using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LabelController : MonoBehaviour
{

    private int LabelNr = 0;
    public int playerNumber;
    private bool ObjectStillLabeled = false;
    private Transform OtherTransform;
    private Collider[] controllerColliders;

    private void Start()
    {
        if (gameObject.tag.Contains("label_"))
        {
            LabelNr = int.Parse(gameObject.tag.Replace("label_", ""));
        }
    }

    private void OnTriggerExit(Collider other)
    {

            OtherTransform = other.transform;
            ObjectStillLabeled = false;
            GameController.current.ObjectTriggerEnter(LabelNr, playerNumber,ObjectStillLabeled, OtherTransform);

    }
    private void OnTriggerEnter(Collider other)
    {
            ObjectStillLabeled = true;
            OtherTransform = other.transform;
            GameController.current.ObjectTriggerEnter(LabelNr, playerNumber, ObjectStillLabeled, OtherTransform);
    }
}
