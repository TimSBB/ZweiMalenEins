using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class cameraSwitcher : MonoBehaviour
{
    private XRController controller;
    public InputHelpers.Button camerswitchButton;
    private bool toggle;
    private float timer = 0;

    private int playerNr;
    private Draw drawScript;
    private bool nextScene;
    private bool posSet;
    private bool galleryEnabled;


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("RightHand Controller").GetComponent<XRController>();
        drawScript = GameObject.Find("RightHand Controller").GetComponent<Draw>();
    }

    // Update is called once per frame
    void Update()
    {
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        nextScene = drawScript.DoNetworkDraw;

        if (timer <= 1)
        {
            timer += 0.1f;
        }
        InputHelpers.IsPressed(controller.inputDevice, camerswitchButton, out bool isPressed);
        if (isPressed && timer >= 1)
        {
            timer = 0;
        }
        if (timer == 0)
        {
            toggle = !toggle;
            if (GameObject.Find("GalleryDome_Prefab(Clone)") != null)
            {
                galleryEnabled = true;
            }
            if (playerNr == 1 )
            {
                // this.transform.position += new Vector3(3.4f, 0, 0);
                //this.transform.Rotate(0, -90, 0, Space.Self);
               
                if (nextScene)
                {

                    this.transform.position = GameObject.Find("camera1_scene2").transform.position;
                    this.transform.rotation = GameObject.Find("camera1_scene2").transform.rotation;
                }
                if (galleryEnabled)
                {
                    this.transform.position = GameObject.Find("camera1_scene_gallery").transform.position;
                    this.transform.rotation = GameObject.Find("camera1_scene_gallery").transform.rotation;
                }
            }
            if (playerNr == 2)
            {
                if (!posSet)
                {
                    this.transform.position = GameObject.Find("camera2").transform.position;
                    this.transform.rotation = GameObject.Find("camera2").transform.rotation;
                    posSet = true;
                }
                if (nextScene)
                {
                    this.transform.position = GameObject.Find("camera2_scene2").transform.position;
                    this.transform.rotation = GameObject.Find("camera2_scene2").transform.rotation;
                }
                if (galleryEnabled)
                {
                    this.transform.position = GameObject.Find("camera2_scene_gallery").transform.position;
                    this.transform.rotation = GameObject.Find("camera2_scene_gallery").transform.rotation;
                }
            }

           


        }

        




        if (toggle)
        {
            if (playerNr == 1 && nextScene)
            {
                if (GameObject.Find("Anweisung.001") != null && GameObject.Find("AnweisungWortStatusbar(Clone)/Anweisungen/Anweisungen") != null)
                {
                    GameObject.Find("Anweisung.001").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("AnweisungWortStatusbar(Clone)/Anweisungen/Anweisungen").GetComponent<MeshRenderer>().enabled = false;
                }
                if (galleryEnabled)
                {
                    var transform = GameObject.Find("Drawing").transform;
                    if (transform.childCount > 0)
                    {
                        foreach (Transform child in transform)
                        {
                            child.GetComponent<LineRenderer>().enabled = false; 
                        }
                    }
                }
            }



            this.GetComponent<Camera>().depth = 1;
            GameObject.Find("ActiveCameraDebug").GetComponent<Canvas>().enabled = true;
            
        }
        if (!toggle)
        {
            if (playerNr == 1 && nextScene)
            {

                if (GameObject.Find("Anweisung.001") != null && GameObject.Find("AnweisungWortStatusbar(Clone)/Anweisungen/Anweisungen") != null)
                {
                    GameObject.Find("Anweisung.001").GetComponent<MeshRenderer>().enabled = true;
                    GameObject.Find("AnweisungWortStatusbar(Clone)/Anweisungen/Anweisungen").GetComponent<MeshRenderer>().enabled = true;
                }
                if (galleryEnabled)
                {
                    var transform = GameObject.Find("Drawing").transform;
                    if (transform.childCount > 0)
                    {
                        foreach (Transform child in transform)
                        {
                            child.GetComponent<LineRenderer>().enabled = true;
                        }
                    }
                }
            }
            this.GetComponent<Camera>().depth = -1;
            GameObject.Find("ActiveCameraDebug").GetComponent<Canvas>().enabled = false;
        }
    }
}
