using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FeedbackController : MonoBehaviour
{
    public GameObject FeedbackPrefab;
    private string LabelTag;
    public int playerNumber;

    private bool grippressed;
    private List<InputDevice> leftHandDevices;
    private List<InputDevice> rightHandDevices;
    private GameObject FeedbackAttachTransform;


    // Start is called before the first frame update
    void Start()
    {
        LabelTag = gameObject.tag;
        GameController.current.onObjectTriggerEnter += OnRightLabel;

        leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);


    }



    private void OnRightLabel(int playerNumber, bool ObjectStillLabeled, Transform OtherTransform)
    {
        //if (leftHandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool grip))
        //{
        //    grippressed = grip;
        //    print("grippressed" + grippressed);
        //}
            if (playerNumber == this.playerNumber && ObjectStillLabeled)
            {
            //var pos = gameObject.transform.position + new Vector3(0, 1, 0);
            var pos = OtherTransform.position;
            var StrahlxRotation = this.transform.rotation.x;
            FeedbackPrefab.transform.GetChild(0).gameObject.transform.Rotate(StrahlxRotation, 0.0f, 0.0f, Space.Self);
            Instantiate(FeedbackPrefab, pos, Quaternion.identity);

        }

    }

    private void OnDestroy()
    {
        GameController.current.onObjectTriggerEnter -= OnRightLabel;
    }
}
