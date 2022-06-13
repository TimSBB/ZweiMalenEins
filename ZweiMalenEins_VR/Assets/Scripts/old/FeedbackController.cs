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
    private bool prefabSpawned = false;
    private List<InputDevice> leftHandDevices;
    private List<InputDevice> rightHandDevices;
    private GameObject FeedbackAttachTransform;

    public List<GameObject> prefabs;
    private XRGrabInteractable word;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag.Contains( "label_")){
            LabelNr = int.Parse(gameObject.tag.Replace("label_", ""));
        }
        GameController.current.onObjectRayEnter += OnSnapSpawn;
        GameController.current.onObjectRayExit += OnSnapDestroy;
    }



    private void OnSnapSpawn(int labelNumber, int playerNumber,Transform OtherTransform)
    {
        //leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        // UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        //if (leftHandDevices[0].TryGetFeatureValue(CommonUsages.gripButton, out bool grip)){
        //    grippressed = grip;
        //    print("grippressed" + grippressed);
        //}

        if (playerNumber == this.playerNumber && labelNumber == this.LabelNr && !prefabSpawned)
            {
            // Project Position onto Strahl
            var projectedPlayer = this.transform.position + Vector3.Project(OtherTransform.position - this.transform.position, this.transform.forward);
            //Fix Rotaion below
            var strahlRotation = this.transform.eulerAngles - this.transform.parent.eulerAngles;
            FeedbackPrefab.transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(strahlRotation.x, 0, 0);
            //Instantiate Feedbackprefab and set tag
            FeedbackPrefab.tag = "label_" + LabelNr;
            Instantiate(FeedbackPrefab, projectedPlayer, Quaternion.identity);
            prefabSpawned = true;
        }

    }


    private void OnSnapDestroy(int labelNumber, int playerNumber)
    {
        if (playerNumber == this.playerNumber && labelNumber == this.LabelNr && prefabSpawned)
        {
           prefabs.AddRange(GameObject.FindGameObjectsWithTag("label_" + LabelNr));
            if (prefabs != null) {
                for (int i = 0; i < prefabs.Count; i++)
                {
                    if (prefabs[i] != null) {
                        if (prefabs[i].name == "FeedbackPrefab(Clone)")
                        {
                            if (prefabs[i].GetComponent<Renderer>().enabled)
                            {
                                //prefabs[i].SetActive(false);
                                prefabs[i].GetComponent<Renderer>().enabled = false;
                                //prefabs[i].GetComponent<MyXRSocketInteractor>().allowSelect = false;
                                //prefabs[i].GetComponent<MyXRSocketInteractor>().enabled = false;
                                //Destroy(prefabs[i].gameObject);
                                //prefabs.Remove(prefabs[i]);
                            }
                            prefabSpawned = false;
                        }
                    }
                }
            }
        }
        

    }





    private void OnDestroy()
    {
        GameController.current.onObjectRayEnter -= OnSnapSpawn;
        GameController.current.onObjectRayExit -= OnSnapDestroy;
    }
}
