using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Material newMaterial;
    private GameObject gross;
    private GameObject mittel;
    private GameObject klein;

    private void Start()
    {
        newMaterial = this.GetComponent<Renderer>().material;

        gross = GameObject.Find("Gross");
        mittel = GameObject.Find("Mittel");
        klein = GameObject.Find("klein");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PaintCapsule") { 
        //change brush tip
        other.GetComponent<Renderer>().material = newMaterial;
        gross.GetComponent<Renderer>().material = newMaterial;
        mittel.GetComponent<Renderer>().material = newMaterial;
        klein.GetComponent<Renderer>().material = newMaterial;
            Debug.Log("NewMaterial " + newMaterial);
        //change line material
        other.SendMessageUpwards("SetLineMaterial", newMaterial, SendMessageOptions.DontRequireReceiver);
        }
    }
}
