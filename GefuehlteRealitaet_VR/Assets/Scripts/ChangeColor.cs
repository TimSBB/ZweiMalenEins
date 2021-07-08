using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChangeColor : MonoBehaviour
{
    public Material newMaterial;
    private GameObject gross;
    private GameObject mittel;
    private GameObject klein;
    public GameObject anzeige;
    private int playerNr;

    private void Start()
    {
        // get Material of this interface item
        newMaterial = this.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.name == "PaintCapsule" && other.gameObject.transform.parent.name == "PaintBrush") {
        if (other.gameObject.name == "PaintCapsule")
        {

            //find brush tips
            gross = GameObject.Find("RightHand Controller/Right Hand Presence/DrawController_Prefab(Clone)/Gross");
            mittel = GameObject.Find("RightHand Controller/Right Hand Presence/DrawController_Prefab(Clone)/Mittel");
            klein = GameObject.Find("RightHand Controller/Right Hand Presence/DrawController_Prefab(Clone)/klein");

            anzeige = GameObject.Find("XR Rig/Camera Offset/RightHand Controller/Right Hand Presence/DrawController_Prefab(Clone)/Farbanzeige");

            //change brush tip
            gross.GetComponent<Renderer>().material = newMaterial;
            mittel.GetComponent<Renderer>().material = newMaterial;
            klein.GetComponent<Renderer>().material = newMaterial;

            anzeige.GetComponent<Renderer>().material.SetColor("Color_4FC6FC3B", newMaterial.GetColor("Color_DCFC887F")); 


            //change line material
            playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
            other.SendMessageUpwards("SetLineMaterial", newMaterial, SendMessageOptions.DontRequireReceiver);
            other.SendMessageUpwards("SetTipMaterial", newMaterial, SendMessageOptions.DontRequireReceiver);
            other.SendMessageUpwards("SetFarbanzeigeMateria", newMaterial, SendMessageOptions.DontRequireReceiver);
        }
    }
    
}
