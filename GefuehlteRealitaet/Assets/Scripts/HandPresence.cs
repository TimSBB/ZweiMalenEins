using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class HandPresence : MonoBehaviour
{

    private InputDevice targetDevice;
    private bool deviceRecognized = false;
    private int prevDeviceCount = 0;
    List<InputDevice> devices = new List<InputDevice>();
    // Start is called before the first frame update
    void Start()
    {
        //List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(devices.Count > prevDeviceCount && !deviceRecognized)
        {
            InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

            foreach (var item in devices)
            {
                Debug.Log(item.name + item.characteristics);
            }
            if (devices.Count > 0)
            {
                targetDevice = devices[0];
            }
            deviceRecognized = true;
        }
        prevDeviceCount = devices.Count;

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue)
        {
            Debug.Log("Pressing Primary Button");
        }
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f)
        {
            Debug.Log("Pressing Primary Button");
        }
    }
}
