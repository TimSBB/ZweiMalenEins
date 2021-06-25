using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class ChangeWidth : MonoBehaviour
{
    public float width;
    private GameObject gross;
    private GameObject mittel;
    private GameObject klein;
    private GameObject radier;
    private int playerNr;


    private void OnTriggerEnter(Collider other)
    {
        gross = GameObject.Find("Gross");
        mittel = GameObject.Find("Mittel");
        klein = GameObject.Find("klein");
        radier = GameObject.Find("Radierer");


        if (other.gameObject.name == "PaintCapsule") {
            //change brush tip
            if (this.gameObject.name == "Gross_Auswahl") 
            {
                gross.GetComponent<MeshRenderer>().enabled = true;
                mittel.GetComponent<MeshRenderer>().enabled = false;
                klein.GetComponent<MeshRenderer>().enabled = false;
                radier.GetComponent<MeshRenderer>().enabled = false;
            }
            if (this.gameObject.name == "Mittel_Auswahl")
            {
                gross.GetComponent<MeshRenderer>().enabled = false;
                mittel.GetComponent<MeshRenderer>().enabled = true;
                klein.GetComponent<MeshRenderer>().enabled = false;
                radier.GetComponent<MeshRenderer>().enabled = false;
            }
            if (this.gameObject.name == "klein_Auswahl") 
            {
                gross.GetComponent<MeshRenderer>().enabled = false;
                mittel.GetComponent<MeshRenderer>().enabled = false;
                klein.GetComponent<MeshRenderer>().enabled = true;
                radier.GetComponent<MeshRenderer>().enabled = false;
            }
            if (this.gameObject.name == "Radierer_Auswahl")
            {
                gross.GetComponent<MeshRenderer>().enabled = false;
                mittel.GetComponent<MeshRenderer>().enabled = false;
                klein.GetComponent<MeshRenderer>().enabled = false;
                radier.GetComponent<MeshRenderer>().enabled = true;
            }
            //change line material
            other.SendMessageUpwards("SetLineWidth", width, SendMessageOptions.DontRequireReceiver);
            playerNr = PhotonNetwork.LocalPlayer.ActorNumber;

            ////change tip of networkplayer
                other.SendMessageUpwards("SetTipWidth", this.gameObject.name, SendMessageOptions.DontRequireReceiver);

        }
    }
}
