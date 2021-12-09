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
        InputDeviceCharacteristics inputDeviceCharacteristics;

        public InputDevice inputDevice;
        public DeviceFindState deviceFindState = DeviceFindState.NotFound;

        public FeatureGroupDataSource(List<InputFeatureUsage> inputFeatureUsages, InputDeviceCharacteristics inputDeviceCharacteristics, float checkPeriod = -1f)
        {
            this.inputFeatureUsages = inputFeatureUsages;
            this.inputDeviceCharacteristics = inputDeviceCharacteristics;
        }

        float checkPeriod = -1f;
        public IEnumerator GetDevice()
        {
            deviceFindState = DeviceFindState.Finding;

            while (deviceFindState != DeviceFindState.Found)
            {

                List<InputDevice> inputDevices = new List<InputDevice>();
                List<InputFeatureUsage> currentDeviceInputFeatureUsages;

                InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics ,inputDevices);

                foreach (InputDevice currentInputDevice in inputDevices)
                {

                    if (!currentInputDevice.isValid)
                        continue;

                    //Debug.Log((uint)currentInputDevice.characteristics);
                    //Debug.Log((uint)inputDeviceCharacteristics);
                    //Debug.Log((uint)currentInputDevice.characteristics & (uint)inputDeviceCharacteristics);

                    //if ((InputDeviceCharacteristics)((uint)currentInputDevice.characteristics & (uint)inputDeviceCharacteristics) != inputDeviceCharacteristics)
                    //    continue;

                    currentDeviceInputFeatureUsages = new List<InputFeatureUsage>();
                    currentInputDevice.TryGetFeatureUsages(currentDeviceInputFeatureUsages);
                    
                    if (((IEnumerable<InputFeatureUsage>)currentDeviceInputFeatureUsages).Intersect(inputFeatureUsages).Count() == inputFeatureUsages.Count())
                    {
                        inputDevice = currentInputDevice;
                        deviceFindState = DeviceFindState.Found;
                        Debug.Log("Device '" + inputDevice.name +  "' found with features: " + inputFeatureUsages[0].name + ", " + (inputFeatureUsages.Count > 1 ? inputFeatureUsages[1].name : " ") + (inputFeatureUsages.Count > 2 ? ", ..." : " "));
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
        public XRNodeType xrNodeType
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
        protected InputDeviceCharacteristics inputDeviceCharacteristics = InputDeviceCharacteristics.None;
    }
}

