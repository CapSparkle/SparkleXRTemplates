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

    public enum DeviceFindState
	{
        NotFound,
        Finding,
        Found
	}

    public class SimpleHandInputProvider : XRInputProvider
    {
        [SerializeField]
        protected Handedness handedness = Handedness.None;
        InputDeviceCharacteristics inputDeviceCharacteristics = InputDeviceCharacteristics.HandTracking;

        InputDevice handDataDevice;
        DeviceFindState handDataDeviceFindState = DeviceFindState.NotFound;

        InputDevice handSimpleFeaturesDevice;
        DeviceFindState handSimpleFeaturesDeviceFindState = DeviceFindState.NotFound;

        void Start()
        {
            xrNodeFeatureGroup = XRNodeFeatureGroup.Hand;
            StartCoroutine("getHandDataDevice");
        }

        float checkPeriod = -1f;
        public IEnumerator getHandDataDevice()
		{
            handDataDeviceFindState = DeviceFindState.Finding;

            while (handDataDeviceFindState != DeviceFindState.Found)
			{
                List<InputDevice> inputDevices = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics((InputDeviceCharacteristics)(((int)inputDeviceCharacteristics) + ((int)handedness)), inputDevices);
                foreach (InputDevice inputDevice in inputDevices)
                {
                    if (!inputDevice.isValid)
                        continue;

                    if (inputDevice.TryGetFeatureValue(CommonUsages.handData, out _handData))
					{
                        handDataDevice = inputDevice;
                        handDataDeviceFindState = DeviceFindState.Found;
                        yield break;
					}      
				}

                if (checkPeriod > 0)
                    yield return new WaitForSeconds(checkPeriod);
                else
                    yield return new WaitForEndOfFrame();
            }
		}

        public IEnumerator getHandSimpleFeaturesDevice()
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
        }

        Hand _handData;
        public Hand handData
		{
			get
			{
                if(handDataDeviceFindState == DeviceFindState.Found)
                    handDataDevice.TryGetFeatureValue(CommonUsages.handData, out _handData);
                
                else if(handDataDeviceFindState == DeviceFindState.NotFound)
                    StartCoroutine("getHandDataDevice");
                
                return _handData;

            }
            protected set
			{
                _handData = value;

            }
		}


        Quaternion _handOrientation;
        public Quaternion handOrientation
        {
            get
            {
                if (handSimpleFeaturesDeviceFindState == DeviceFindState.Found)
                    handSimpleFeaturesDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion _handOrientation);

                else if (handSimpleFeaturesDeviceFindState == DeviceFindState.NotFound)
                    StartCoroutine("getHandSimpleFeaturesDevice");

                return _handOrientation;

            }
            protected set
            {
                _handOrientation = value;
            }
        }


        Vector3 _handCenterPosition;
        public Vector3 HandCenterPosition
        {
            get
            {
                if (handSimpleFeaturesDeviceFindState == DeviceFindState.Found)
                    handSimpleFeaturesDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _handCenterPosition);

                else if (handSimpleFeaturesDeviceFindState == DeviceFindState.NotFound)
                    StartCoroutine("getHandSimpleFeaturesDevice");

                return _handCenterPosition;

            }
            protected set
            {
                _handCenterPosition = value;
            }
        }
    }
}