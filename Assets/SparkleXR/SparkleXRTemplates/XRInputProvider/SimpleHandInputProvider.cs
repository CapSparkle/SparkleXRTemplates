using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.InteractionSubsystems;


namespace SparkleXRTemplates
{
    public enum Handedness
    {
        None,
        Left = 256,
        Right = 512
    }

	public class SimpleHandInputProvider : XRInputProvider
    {
        [SerializeField]
        protected Handedness handedness = Handedness.None;


        #region -Feature representing hand finger points-
        InputDevice handFingerDataDevice;
		List<InputFeatureUsage> handFingerFeatures = new List<InputFeatureUsage>()
		{
            (InputFeatureUsage) CommonUsages.handData,
        };

		Hand _handData;
        public Hand handData
        {
            get
            {
                if (DeviceFindStates[handFingerDataDevice] == DeviceFindState.Found)
				{
                    if (!handFingerDataDevice.TryGetFeatureValue(CommonUsages.handData, out _handData))
					{
                        DeviceFindStates[handFingerDataDevice] = DeviceFindState.NotFound;
                        StartCoroutine(GetDeviceWithFeatures(handFingerFeatures, handFingerDataDevice));
                    }
                }
                 else if (DeviceFindStates[handFingerDataDevice] == DeviceFindState.NotFound)
                    StartCoroutine(GetDeviceWithFeatures(handFingerFeatures, handFingerDataDevice));
                 
                return _handData;
            }
        }
        #endregion

        #region -Featrues representing simple hand data-
        InputDevice handSimpleFeaturesDevice;
        List<InputFeatureUsage> handSimpleFeatures = new List<InputFeatureUsage>()
        {
            (InputFeatureUsage) CommonUsages.devicePosition,
            (InputFeatureUsage) CommonUsages.deviceRotation
        };
        
        Quaternion _handOrientation;
        public Quaternion handOrientation
        {
            get
            {
                if (DeviceFindStates[handSimpleFeaturesDevice] == DeviceFindState.Found)
				{
                    
                    if(!handSimpleFeaturesDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion _handOrientation))
					{
                        DeviceFindStates[handSimpleFeaturesDevice] = DeviceFindState.NotFound;
                        StartCoroutine(GetDeviceWithFeatures(handSimpleFeatures, handSimpleFeaturesDevice));
                    }
                }
                else if (DeviceFindStates[handSimpleFeaturesDevice] == DeviceFindState.NotFound)
                    StartCoroutine(GetDeviceWithFeatures(handSimpleFeatures, handSimpleFeaturesDevice));

                return _handOrientation;
            }
        }

        Vector3 _handCenterPosition;
        public Vector3 handCenterPosition
        {
            get
            {
                if (DeviceFindStates[handSimpleFeaturesDevice] == DeviceFindState.Found)
				{
                    if (!handSimpleFeaturesDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _handCenterPosition))
                    {
                        DeviceFindStates[handSimpleFeaturesDevice] = DeviceFindState.NotFound;
                        StartCoroutine(GetDeviceWithFeatures(handSimpleFeatures, handSimpleFeaturesDevice));
                    }
                }
                else if (DeviceFindStates[handSimpleFeaturesDevice] == DeviceFindState.NotFound)
                    StartCoroutine(GetDeviceWithFeatures(handSimpleFeatures, handSimpleFeaturesDevice));

                return _handCenterPosition;
            }
        }
        #endregion

        void Start()
        {
            xrNodeFeatureGroup = XRNodeFeatureGroup.Hand;

            inputDeviceCharacteristics = (InputDeviceCharacteristics)((int)inputDeviceCharacteristics + (int)handedness);

            DeviceFindStates.Add(handFingerDataDevice, DeviceFindState.NotFound);
            DeviceFindStates.Add(handSimpleFeaturesDevice, DeviceFindState.NotFound);

            StartCoroutine(GetDeviceWithFeatures(handFingerFeatures, handFingerDataDevice));
            StartCoroutine(GetDeviceWithFeatures(handSimpleFeatures, handSimpleFeaturesDevice));
        }

        /*public IEnumerator GetHandDataDevice()
		{
            handFingerDataDeviceFindState = DeviceFindState.Finding;

            while (handFingerDataDeviceFindState != DeviceFindState.Found)
			{
                List<InputDevice> inputDevices = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics((InputDeviceCharacteristics)(((int)inputDeviceCharacteristics) + ((int)handedness)), inputDevices);
                foreach (InputDevice inputDevice in inputDevices)
                {
                    if (!inputDevice.isValid)
                        continue;

                    if (inputDevice.TryGetFeatureValue(CommonUsages.handData, out _handData))
					{
                        handFingerDataDevice = inputDevice;
                        handFingerDataDeviceFindState = DeviceFindState.Found;
                        yield break;
					}      
				}

                if (checkPeriod > 0)
                    yield return new WaitForSeconds(checkPeriod);
                else
                    yield return new WaitForEndOfFrame();
            }
		}

        public IEnumerator GetHandSimpleFeaturesDevice()
        {
            handSimpleFeaturesDeviceFindState = DeviceFindState.Finding;

            while (handSimpleFeaturesDeviceFindState != DeviceFindState.Found)
            {
                List<InputDevice> inputDevices = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics((InputDeviceCharacteristics)(((int)inputDeviceCharacteristics) + ((int)handedness)), inputDevices);
                foreach (InputDevice inputDevice in inputDevices)
                {
                    if (!inputDevice.isValid)
                        continue;

                    if (inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out _handCenterPosition) &&
                        inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out _handOrientation))
                    {
                        handSimpleFeaturesDevice = inputDevice;
                        handSimpleFeaturesDeviceFindState = DeviceFindState.Found;
                        yield break;
                    }
                }

                if (checkPeriod > 0)
                    yield return new WaitForSeconds(checkPeriod);
                else
                    yield return new WaitForEndOfFrame();
            }
        }*/
    }
}