using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace SparkleXRTemplates
{
    public enum XRNodeType
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

    public class FeatureGroupDataSource
    {
        List<InputFeatureUsage> inputFeatureUsages;

        public InputDevice inputDevice;
        public DeviceFindState deviceFindState = DeviceFindState.NotFound;

        public FeatureGroupDataSource(List<InputFeatureUsage> inputFeatureUsages, float checkPeriod = -1f)
        {
            this.inputFeatureUsages = inputFeatureUsages;
        }

        float checkPeriod = -1f;
        public IEnumerator GetDevice()
        {
            deviceFindState = DeviceFindState.Finding;
            Debug.Log("Finding");
            while (deviceFindState != DeviceFindState.Found)
            {

                List<InputDevice> inputDevices = new List<InputDevice>();
                List<InputFeatureUsage> currentDeviceInputFeatureUsages;

                InputDevices.GetDevices(inputDevices);

                //Debug.Log("inputDevices with characteristics = " + inputDevices.Count().ToString());
                //Debug.Log("inputDevices = " + InputDevices.GetDevices().;
                
                foreach (InputDevice currentInputDevice in inputDevices)
                {
                    if (!inputDevice.isValid)
                        continue;

                    currentDeviceInputFeatureUsages = new List<InputFeatureUsage>();
                    inputDevice.TryGetFeatureUsages(currentDeviceInputFeatureUsages);

                    Debug.Log("Intersects?");
                    if (((IEnumerable<InputFeatureUsage>)currentDeviceInputFeatureUsages).Intersect(inputFeatureUsages).Count() == inputFeatureUsages.Count())
                    {
                        inputDevice = currentInputDevice;
                        deviceFindState = DeviceFindState.Found;
                        Debug.Log("Found");
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

    // One XRInputProvider may be logically corresponding to several UnityEngine.XR.InputDevice -s
    public abstract class XRInputProvider : MonoBehaviour
    {
        //This is designation of what this InputProvider is for others 
        XRNodeType _xrNodeFeatureGroup;
        public XRNodeType xrNodeFeatureType
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
    }
}

