using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MyXRSocketInteractor : XRSocketInteractor
{
    private XRInteractionManager SceneInteractionManager;
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        SceneInteractionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        interactionManager = SceneInteractionManager;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
