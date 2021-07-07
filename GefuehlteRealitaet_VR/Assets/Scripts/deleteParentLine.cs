using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteParentLine : MonoBehaviour
{
    private GameObject radierer;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PaintCapsule")
        {
            if (other.transform.parent.Find("DrawController_Prefab(Clone)/Radierer") != null)
            {
                var enabled = other.transform.parent.Find("DrawController_Prefab(Clone)/Radierer").GetComponent<MeshRenderer>().enabled;
                if (enabled)
                {
                    Destroy(this.transform.parent.gameObject.transform.parent.gameObject);
                }
            } else
            {
                radierer = GameObject.Find("RightHand Controller/Right Hand Presence/DrawController_Prefab(Clone)/Radierer");

                if (radierer.GetComponent<MeshRenderer>().enabled == true)
                {
                    Destroy(this.transform.parent.gameObject.transform.parent.gameObject);
                }
            }

        }
    }
}
