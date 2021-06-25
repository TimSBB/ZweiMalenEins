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
    private int playerNr;

    private void Start()
    {
        // get Material of this interface item
        newMaterial = this.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PaintCapsule") {

            //find brush tips
            gross = GameObject.Find("Gross");
            mittel = GameObject.Find("Mittel");
            klein = GameObject.Find("klein");
            
            //change brush tip
            gross.GetComponent<Renderer>().material = newMaterial;
            mittel.GetComponent<Renderer>().material = newMaterial;
            klein.GetComponent<Renderer>().material = newMaterial;

            //change line material
            playerNr = PhotonNetwork.LocalPlayer.ActorNumber;
            if (playerNr == 1 || playerNr == 2)
            {
                other.SendMessageUpwards("SetLineMaterial", newMaterial, SendMessageOptions.DontRequireReceiver);
                other.SendMessageUpwards("SetTipMaterial", newMaterial, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    
}
