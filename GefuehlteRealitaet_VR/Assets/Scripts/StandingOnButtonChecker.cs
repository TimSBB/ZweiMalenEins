using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingOnButtonChecker : MonoBehaviour
{
    public bool standingOnBottom = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "ReticleParent")
        {
            standingOnBottom = true;
            Debug.Log("standing on button= " + standingOnBottom);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ReticleParent")
        {
            standingOnBottom = false;
            Debug.Log("standing on button= " + standingOnBottom);
        }
    }

}
