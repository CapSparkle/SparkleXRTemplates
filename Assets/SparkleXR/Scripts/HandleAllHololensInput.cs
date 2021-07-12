using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;


//#if WINDOWS_UWP

public class HandleAllHololensInput : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    private void Start()
    {
        StartCoroutine("outputUpdater");
    }

    IEnumerator outputUpdater()
    {
        while(true)
        {
            OutputAllInputDeviceData();
            yield return new WaitForSeconds(5f);
        }
    }

    void OutputDeviceInputFeatures(InputDevice device)
    {
        if (!device.isValid)
            return;

        Debug.Log(string.Format("Device: \"'{0}'\"; role \"'{1}'\"",
                        device.name, device.characteristics.ToString()));
        text.text += string.Format("Device: \"'{0}'\"; role \"'{1}'\" | \n\n",
                    device.name, device.characteristics.ToString());
        List<InputFeatureUsage> featureUsages = new List<InputFeatureUsage>();
        device.TryGetFeatureUsages(featureUsages);
        foreach (InputFeatureUsage IFUsage in featureUsages)
        {
            print(device.name + "__Feature(name: \"" + IFUsage.name + "\", type: \"" + IFUsage.type + "\"");
            text.text += (device.name + "__Feature(name: \"" + IFUsage.name + "\", type: \"" + IFUsage.type + "\"\n");
        }
    }

    void OutputAllInputDeviceData()
    {
        text.text = "";
        //All hard devices data
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        print(inputDevices.Count);
        text.text += "inputDevices.Count = " + inputDevices.Count.ToString() + ":" + "\n";


        //OutputDeviceInputFeatures(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand));
        //OutputDeviceInputFeatures(InputDevices.GetDeviceAtXRNode(XRNode.RightHand));

        foreach (var device in inputDevices)
        {
            if (device.characteristics.ToString().Contains("HandTracking") 
                && (device == InputDevices.GetDeviceAtXRNode(XRNode.RightHand)))
            {
                OutputDeviceInputFeatures(device);
            }
        }
    }
}
//#endif
