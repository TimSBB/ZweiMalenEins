using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MyXRSocketInteractor : XRSocketInteractor
{
    private XRInteractionManager SceneInteractionManager;
    // Start is called before the first frame update
    private string targetTag;

    protected override void Awake()
    {
        base.Awake();
        SceneInteractionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        interactionManager = SceneInteractionManager;
        targetTag = this.gameObject.tag;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void disableActive()
    {
        if (this.GetComponent<MyXRSocketInteractor>().selectTarget != null)
        {
            this.GetComponent<MyXRSocketInteractor>().allowSelect = false;
        }

    }


    //public override bool CanSelect(XRBaseInteractable interactable)
    //{
    //    return base.CanSelect(interactable) && (interactable.CompareTag(targetTag) || interactable.CompareTag("word"));
    //}

}
