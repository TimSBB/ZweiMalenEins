using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWidth : MonoBehaviour
{
    public float width;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PaintCapsule") { 
        //change brush tip
        //change line material
        other.SendMessageUpwards("SetLineWidth", width, SendMessageOptions.DontRequireReceiver);
        }
    }
}
