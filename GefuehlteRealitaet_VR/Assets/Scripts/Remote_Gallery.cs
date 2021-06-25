using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;


public class Remote_Gallery : MonoBehaviour
{
    private bool galleryEnabled = false;
    private PhotonView PV;
    private int playerNr;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    public void enableGallery()
    {
        var transform = GameObject.Find("GalleryDome").transform;

        if (!galleryEnabled)
        {
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.GetComponent<Renderer>().enabled = true;
                    child.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }
            GameObject.Find("GalleryDome").GetComponent<GalleryLoader>().enabled = true;
            GameObject.Find("GalleryDome").GetComponent<GalleryLoader>().loadGallery = true;
            galleryEnabled = true;
            PV.RPC("RPC_GalleryEnable", RpcTarget.AllBufferedViaServer, playerNr);
        }


    }
    [PunRPC]
    void RPC_GalleryEnable(int playerNumber)
    {
        if (playerNumber != playerNr)
        {
            var transform = GameObject.Find("GalleryDome").transform;

            if (!galleryEnabled)
            {
                if (transform.childCount > 0)
                {
                    foreach (Transform child in transform)
                    {
                        child.gameObject.GetComponent<Renderer>().enabled = true;
                        child.gameObject.GetComponent<BoxCollider>().enabled = true;
                    }
                }
                GameObject.Find("GalleryDome").GetComponent<GalleryLoader>().enabled = true;
                GameObject.Find("GalleryDome").GetComponent<GalleryLoader>().loadGallery = true;
                galleryEnabled = true;
            }
        }
    }
}
