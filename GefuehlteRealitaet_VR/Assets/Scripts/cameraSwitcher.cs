using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class cameraSwitcher : MonoBehaviour
{
    private XRController controller;
    public InputHelpers.Button camerswitchButton;
    private bool toggle;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("RightHand Controller").GetComponent<XRController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 1)
        {
            timer += 0.1f;
        }
        InputHelpers.IsPressed(controller.inputDevice, camerswitchButton, out bool isPressed);
        if (isPressed && timer >= 1)
        {
            timer = 0;
        }
        if (timer == 0)
        {
            toggle = !toggle;
        }
        if (toggle)
        {
            this.GetComponent<Camera>().depth = 1;
            GameObject.Find("ActiveCameraDebug").GetComponent<Canvas>().enabled = true;
        }
        if (!toggle)
        {
            this.GetComponent<Camera>().depth = -1;
            GameObject.Find("ActiveCameraDebug").GetComponent<Canvas>().enabled = false;
        }
    }
}
