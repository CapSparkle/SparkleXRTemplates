
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace SparkleXRTemplates
{
    public enum Handedness
    {
        None = 0,
        Left = 256,
        Right = 512
	}

    //TODO: consistent naming!
	



	public class SimpleHandInputProvider : XRInputProvider
    {
        [SerializeField]
        protected Handedness handedness = Handedness.None;

        public Action onHandednessChange;

        #region -Feature representing hand finger points-

        FeatureGroupDataSource handFingerData;

		Hand _handData;
        public Hand handData
        {
            get
            {
                if (handFingerData.deviceFindState == DeviceFindState.Found)
				{
                    if (!handFingerData.inputDevice.TryGetFeatureValue(CommonUsages.handData, out _handData))
					{
                        handFingerData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(handFingerData.GetDevice());
                    }
                }
                 else if (handFingerData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handFingerData.GetDevice());
                 
                return _handData;

                //return MLHandTracking.Right.Center
            }
        }
        #endregion


        #region -Featrues representing simple hand data-

        FeatureGroupDataSource handSimpleFeaturesData;
        
        Quaternion _handOrientation = Quaternion.identity;
        public Quaternion handOrientation
        {
            get
            {
                if (handSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
				{
                    
                    if(!handSimpleFeaturesData.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out _handOrientation))
					{
                        handSimpleFeaturesData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(handSimpleFeaturesData.GetDevice());
                    }
                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.GetDevice());

                return _handOrientation;
            }
        }

        Vector3 _handCenterPosition;
        public Vector3 handCenterPosition
        {
            get
            {
                if (handSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
                {
                    if (!handSimpleFeaturesData.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out _handCenterPosition))
                    {
                        handSimpleFeaturesData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(handSimpleFeaturesData.GetDevice());
                    }
                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.GetDevice());

                return _handCenterPosition;
            }
        }
        #endregion



        protected void Start()
        {
            xrNodeType = XRNodeType.Hand;

            try
			{
                inputDeviceCharacteristics = (InputDeviceCharacteristics)((int)inputDeviceCharacteristics + (int)handedness + (int)InputDeviceCharacteristics.HandTracking);


                handFingerData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { (InputFeatureUsage)CommonUsages.handData },
                    inputDeviceCharacteristics);
                handSimpleFeaturesData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { (InputFeatureUsage)CommonUsages.devicePosition, (InputFeatureUsage)CommonUsages.deviceRotation },
                    inputDeviceCharacteristics);
            }
            catch (Exception exc)
			{
                print(exc.Message);
			}

            
            StartCoroutine(handFingerData.GetDevice());
            StartCoroutine(handSimpleFeaturesData.GetDevice());
        }
    }
}