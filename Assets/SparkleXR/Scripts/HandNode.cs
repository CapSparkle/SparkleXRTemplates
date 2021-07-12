using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandNode : MonoBehaviour
{

    void UpdatePosition(InputDevice inputDevice)
    {
        Vector3 HandPosition;
        inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out HandPosition);
        transform.position = HandPosition;
    }

    const InputDeviceCharacteristics myCharacterictic = InputDeviceCharacteristics.HandTracking;
    
    [SerializeField]
    InputDeviceRole myRole = InputDeviceRole.LeftHanded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand));
    }
}
