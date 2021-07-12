using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace SparkleXRTemplates.Hololens
{
    public class HololensHand : MonoBehaviour
    {
        bool inputDeviceConnected = false;

        private InputDeviceCharacteristics Handedness = InputDeviceCharacteristics.Right;
        private InputDeviceCharacteristics inputCharacteristics = InputDeviceCharacteristics.HandTracking;

        private List<InputDevice> handInpudDevices;

        Hand handFingersData;
        Vector3 handPosition;
        Quaternion handOrientation;

        void GetHandInputDevice()
        {
            InputDevices.GetDevicesWithCharacteristics((InputDeviceCharacteristics)(((int)inputCharacteristics) + ((int)Handedness)), handInpudDevices);

            foreach (InputDevice inputDevice in handInpudDevices)
            {
                if(inputDevice.isValid)
                {
                    if (!inputDevice.TryGetFeatureValue(CommonUsages.handData, out handFingersData)) { }
                    else
                    {
                        inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out handPosition);
                        inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out handOrientation);
                    }
                }
            }
        }

        void Update()
        {
            GetHandInputDevice();
        }

        void SomeMethod()
        {
        }
    }

}


