using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LabelController : MonoBehaviour
{

    private int LabelNr = 0;
    public int playerNumber;
    private Transform OtherTransform;
    private Collider[] controllerColliders;
    private XRGrabInteractable word;
    private void Start()
    {
        if (gameObject.tag.Contains("label_"))
        {
            LabelNr = int.Parse(gameObject.tag.Replace("label_", ""));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("xrRigController"))
        {
            //var controller = other.GetComponent<XRDirectInteractor>();
            //if (!controller.isSelectActive)
            //{
               // word = other.GetComponent<XRGrabNetworkInteractable>();
                GameController.current.ObjectTriggerExit(LabelNr, playerNumber);
            //}
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("xrRigController"))
        {
            print("should trigger snap spawn");
            var controller = other.GetComponent<XRDirectInteractor>();
            //word = other.GetComponent<XRGrabNetworkInteractable>();
            if (controller.isSelectActive)
            {
                OtherTransform = other.transform;
                GameController.current.ObjectTriggerEnter(LabelNr, playerNumber, OtherTransform);
            }
        }
    }
}
