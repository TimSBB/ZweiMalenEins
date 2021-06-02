using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FeedbackController : MonoBehaviour
{
    public GameObject FeedbackPrefab;
    private int LabelNr = 0;
    public int playerNumber;

    private bool grippressed;
    private List<InputDevice> leftHandDevices;
    private List<InputDevice> rightHandDevices;
    private GameObject FeedbackAttachTransform;


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag.Contains( "label_")){
            LabelNr = int.Parse(gameObject.tag.Replace("label_", ""));
        }
        GameController.current.onObjectTriggerEnter += OnRightLabel;

        leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);


    }



    private void OnRightLabel(int labelNumber, int playerNumber, bool ObjectStillLabeled, Transform OtherTransform)
    {
        //if (leftHandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool grip))
        //{
        //    grippressed = grip;
        //    print("grippressed" + grippressed);
        //}
            if (playerNumber == this.playerNumber && ObjectStillLabeled && labelNumber == this.LabelNr)
            {
            // Project Position onto Strahl
            var projectedPlayer = this.transform.position + Vector3.Project(OtherTransform.position - this.transform.position, this.transform.forward);


            var strahlRotation = this.transform.eulerAngles - this.transform.parent.eulerAngles;
            FeedbackPrefab.transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(strahlRotation.x, 0, 0); 
            
            Instantiate(FeedbackPrefab, projectedPlayer, Quaternion.identity);

        }

    }

    private void OnDestroy()
    {
        GameController.current.onObjectTriggerEnter -= OnRightLabel;
    }
}
