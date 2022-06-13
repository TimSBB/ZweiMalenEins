using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using Photon.Pun;

public class ChangeSceneOnRaycastHit : MonoBehaviour
{
    private RaycastHit hit;
    private GameObject canvas;
    public Image loadingFeedback;
    private float loadingStatus = 0;
    private bool standingOnBottom = false;
    private GameObject rayOrigin;
    private ChangeScene changescene;

    private int playerNr;
    private bool scene2Set;
    private resetPos resetPosScript;
    private bool nextScentriggered;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("CanvasObject");
        changescene = GameObject.Find("ChangeScene").GetComponent<ChangeScene>();
        resetPosScript = GameObject.Find("Camera Offset").GetComponent<resetPos>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            changescene.changeSceneElems();
            nextScentriggered = true;
            loadingStatus = 0;
        }
        scene2Set = resetPosScript.scene2Set;
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;


        if (playerNr == 2  && scene2Set && !nextScentriggered)
        {
            loadingFeedback = GameObject.Find("UIRadialImage").GetComponent<Image>();
        }
        if (playerNr == 1  && !resetPosScript.killedInstance && resetPosScript.sceneSet && !nextScentriggered)
        {
            loadingFeedback = GameObject.Find("UIRadialImage").GetComponent<Image>();
        }
        //rayOrigin = GameObject.Find("Bottom_Button");
        //standingOnBottom = rayOrigin.GetComponent<StandingOnButtonChecker>().standingOnBottom;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        if (Physics.Raycast(ray, out hit) && !nextScentriggered && loadingFeedback != null)
        {

            if (hit.collider.isTrigger && hit.transform.name == "lookAtButton" && standingOnBottom)
            {
                //print("I'm looking at " + hit.transform.name);
                //Do the thing
                canvas.transform.position = hit.transform.position;
                canvas.GetComponent<Canvas>().enabled = true;
                loadingStatus = loadingStatus + 0.006f;
                if (loadingStatus > 1)
                {
                    loadingStatus = 0;
                    changescene.changeSceneElems();
                    nextScentriggered = true;
                }
                loadingFeedback.fillAmount = loadingStatus;
            }
            else
            {
                canvas.GetComponent<Canvas>().enabled = false;
                loadingStatus = 0;
                loadingFeedback.fillAmount = loadingStatus;
            }
        }
        else
        {
            canvas.GetComponent<Canvas>().enabled = false;
            loadingStatus = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bottom_Button")
        {
            standingOnBottom = true;
            //Debug.Log("standing on button= " + standingOnBottom);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Bottom_Button")
        {
            standingOnBottom = false;
            //Debug.Log("standing on button= " + standingOnBottom);
        }
    }





}
