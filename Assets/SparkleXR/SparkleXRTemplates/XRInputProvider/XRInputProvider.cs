using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace SparkleXRTemplates
{
    public enum XRNodeFeatureGroup
    {
        Controller,
        Hand,
        HMD
    }

    public enum DeviceFindState
    {
        NotFound,
        Finding,
        Found
    }


    // One XRInputProvider may be logically corresponding to several UnityEngine.XR.InputDevice -s
    public abstract class XRInputProvider : MonoBehaviour
    {
        //This is designation of what this InputProvider is for others 
        XRNodeFeatureGroup _xrNodeFeatureGroup;
        public XRNodeFeatureGroup xrNodeFeatureGroup
        {
            get
            {
                return _xrNodeFeatureGroup;
            }
            protected set
            {
                _xrNodeFeatureGroup = value;
            }
        }


        //This variable is using for choose subset of devices to find features in them
        protected InputDeviceCharacteristics inputDeviceCharacteristics = InputDeviceCharacteristics.HandTracking;


        protected Dictionary<InputDevice, DeviceFindState> DeviceFindStates = new Dictionary<InputDevice, DeviceFindState>();


        float checkPeriod = -1f;
        protected IEnumerator GetDeviceWithFeatures(List<InputFeatureUsage> targetInputFeatureUsages, InputDevice inputDeviceToFind)
        {
            DeviceFindStates[inputDeviceToFind] = DeviceFindState.Finding;

            while (DeviceFindStates[inputDeviceToFind] != DeviceFindState.Found)
            {

                List<InputDevice> inputDevices = new List<InputDevice>();
                List<InputFeatureUsage> inputFeatureUsages;

                InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);
                foreach (InputDevice inputDevice in inputDevices)
                {
                    if (!inputDevice.isValid)
                        continue;

                    inputFeatureUsages = new List<InputFeatureUsage>();
                    inputDevice.TryGetFeatureUsages(inputFeatureUsages);

                    if (((IEnumerable<InputFeatureUsage>)inputFeatureUsages).Intersect(targetInputFeatureUsages).Count() == targetInputFeatureUsages.Count())
                    {
                        inputDeviceToFind = inputDevice;
                        DeviceFindStates[inputDeviceToFind] = DeviceFindState.Found;
                        yield break;
                    }
                }

                if (checkPeriod > 0)
                    yield return new WaitForSeconds(checkPeriod);
                else
                    yield return new WaitForEndOfFrame();
            }
        }

    }
}

