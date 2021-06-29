using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class ChangeSceneOnRaycastHit : MonoBehaviour
{
    private RaycastHit hit;
    private GameObject canvas;
    public Image loadingFeedback;
    private float loadingStatus = 0;
    private bool standingOnBottom = false;
    private GameObject rayOrigin;
    private ChangeScene changescene;

    // Start is called before the first frame update
    void Start()
    {
        loadingFeedback = GameObject.Find("UIRadialImage").GetComponent<Image>();
        canvas = GameObject.Find("CanvasObject");
        changescene = GameObject.Find("ChangeScene").GetComponent<ChangeScene>();
    }

    // Update is called once per frame
    void Update()
    {
        //rayOrigin = GameObject.Find("Bottom_Button");
        //standingOnBottom = rayOrigin.GetComponent<StandingOnButtonChecker>().standingOnBottom;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        if (Physics.Raycast(ray, out hit))
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
                }
                loadingFeedback.fillAmount = loadingStatus;
            }
            else
            {
                canvas.GetComponent<Canvas>().enabled = false;
                loadingStatus = 0;
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
