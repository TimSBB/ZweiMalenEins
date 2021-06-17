using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Material newMaterial;

    private void Start()
    {
        newMaterial = this.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PaintCapsule") { 
        //change brush tip
        other.GetComponent<Renderer>().material = newMaterial;
        Debug.Log("NewMaterial " + newMaterial);
        //change line material
        other.SendMessageUpwards("SetLineMaterial", newMaterial, SendMessageOptions.DontRequireReceiver);
        }
    }
}
