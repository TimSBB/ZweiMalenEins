using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGallery : MonoBehaviour
{
    public GameObject gallery;
    public GameObject gallerySchrift;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) Gallery();
    }

    private void Gallery()
    {
        Instantiate(gallery);
        Instantiate(gallerySchrift);

        var transform = GameObject.Find("Drawing").transform;
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        var oldAnweisungen = GameObject.Find("AnweisungWortStatusbar");
        Destroy(oldAnweisungen);
    }
}
