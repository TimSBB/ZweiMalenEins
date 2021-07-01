using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class debugGallery : MonoBehaviour
{
    public GameObject gallery;
    public GameObject gallerySchrift;
    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) Gallery();
    }

    private void Gallery()
    {
        PV.RPC("RPC_TimesUp", RpcTarget.AllBufferedViaServer);
        //Instantiate(gallery);
        //Instantiate(gallerySchrift);

        //var transform = GameObject.Find("Drawing").transform;
        //if (transform.childCount > 0)
        //{
        //    foreach (Transform child in transform)
        //    {
        //        Destroy(child.gameObject);
        //    }
        //}

        //var oldAnweisungen = GameObject.Find("AnweisungWortStatusbar");
        //Destroy(oldAnweisungen);

    }


    [PunRPC]
    void RPC_TimesUp()
    {
            //var transform = GameObject.Find("Drawing").transform;
            //if (transform.childCount > 0)
            //{
            //    foreach (Transform child in transform)
            //    {
            //        Destroy(child.gameObject);
            //    }
            //}
            Instantiate(gallery);
            Instantiate(gallerySchrift);

        Destroy(GameObject.Find("AnweisungWortStatusbar(Clone)"));
        Destroy(GameObject.Find("Colorpicker(Clone)"));
    }

}
