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
        //change brush tip
        other.GetComponent<Renderer>().material = newMaterial;
        //change line material
        other.SendMessageUpwards("SetLineMaterial", newMaterial, SendMessageOptions.DontRequireReceiver);
    }
}
