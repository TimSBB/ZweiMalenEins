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


    // Start is called before the first frame update
    void Start()
    {
        LabelTag = gameObject.tag;
        GameController.current.onObjectTriggerEnter += OnRightLabel;

        leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);


    }



    private void OnRightLabel(string LabelTag, int playerNumber, bool ObjectStillLabeled)
    {
        //if (leftHandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool grip))
        //{
        //    grippressed = grip;
        //    print("grippressed" + grippressed);
        //}
            //if (LabelTag == this.LabelTag  && playerNumber == this.playerNumber && ObjectStillLabeled && !grippressed) {
            if (LabelTag == this.LabelTag && playerNumber == this.playerNumber && ObjectStillLabeled)
            {
                var pos = gameObject.transform.position + new Vector3(0, 1, 0);
            Instantiate(FeedbackPrefab, pos, Quaternion.identity);
        }

    }

    private void OnDestroy()
    {
        GameController.current.onObjectTriggerEnter -= OnRightLabel;
    }
}
