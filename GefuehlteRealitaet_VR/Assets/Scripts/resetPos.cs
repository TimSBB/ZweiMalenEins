using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class resetPos : MonoBehaviour
{
    private int playerNr;
    private bool sceneSet;
    public GameObject CharacterEditor_Scene_player1;
    public GameObject CharacterEditor_Scene_player2;
    // Start is called before the first frame update
    void Start()
    {
        var pos = GameObject.Find("Main Camera").transform.position;
        this.transform.position = new Vector3(-pos.x, this.transform.position.y, -pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
        if (playerNr == 1 && !sceneSet)
        {
            // this.transform.position = new Vector3(-1.7f, this.transform.position.y, 0);
            this.transform.position = new Vector3(-1.7f, this.transform.position.y, 0);
            Instantiate(CharacterEditor_Scene_player1);
            sceneSet = true;
            LoadingOverlay overlay = GameObject.Find("LoadingOverlay").gameObject.GetComponent<LoadingOverlay>();
            overlay.FadeOut();
        }
        if (playerNr == 2 && !sceneSet)
        {
            // this.transform.position = new Vector3(-1.7f, this.transform.position.y, 0);
            this.transform.position = new Vector3(-1.7f, this.transform.position.y, 0);
            Instantiate(CharacterEditor_Scene_player2);
            sceneSet = true;
            LoadingOverlay overlay = GameObject.Find("LoadingOverlay").gameObject.GetComponent<LoadingOverlay>();
            overlay.FadeOut();
        }
    }
}
