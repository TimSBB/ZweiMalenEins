using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class resetPos : MonoBehaviour
{
    private int playerNr;
    public bool sceneSet;
    public bool scene2Set;
    public bool killedInstance = false;
    private bool resetOrigin = false;
    public GameObject CharacterEditor_Scene_player1;
    public GameObject CharacterEditor_Scene_player2;
    private GameObject rightHand;
    private GameObject cam;

    private XRController controller;
    public InputHelpers.Button resetButton;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.eulerAngles = new Vector3(0, 180, 0);
        cam = GameObject.Find("Main Camera");
        var pos = cam.transform.position;
        this.transform.position = new Vector3(-pos.x, this.transform.position.y, -pos.z);
        rightHand = GameObject.Find("RightHand Controller");
        controller = rightHand.GetComponent<XRController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(controller.inputDevice, resetButton, out bool isPressed);
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        if (playerNr == 1 && !sceneSet)
        {
            //this.transform.position = new Vector3(-1.7f*0, this.transform.position.y, 0);

            Instantiate(CharacterEditor_Scene_player1);
            sceneSet = true;
            LoadingOverlay overlay = GameObject.Find("LoadingOverlay").gameObject.GetComponent<LoadingOverlay>();
            overlay.FadeOut();
        }
        if (playerNr == 2)
        {
            if (!scene2Set)
           {
                LoadingOverlay overlay = GameObject.Find("LoadingOverlay").gameObject.GetComponent<LoadingOverlay>();
                Destroy(GameObject.Find("CharacterEditor_Scene_player1(Clone)"));
                //this.transform.position = new Vector3(+1.7f*0, this.transform.position.y, 0);

                Instantiate(CharacterEditor_Scene_player2);
                scene2Set = true;
                killedInstance = true;
                overlay.FadeOut();
            }
        }
        if (isPressed)
        {
            if (!resetOrigin) { 
            this.transform.RotateAround(cam.transform.position, Vector3.up, -rightHand.transform.eulerAngles.y);
            this.transform.position = new Vector3(rightHand.transform.position.x, this.transform.position.y, rightHand.transform.position.z);
                resetOrigin = true;
            }

        }
    }
}
