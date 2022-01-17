
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

        protected FeatureGroupDataSource handFingerPointsData;

        protected Hand _handData;
        public Hand handData
        {
            get
            {
                if (handFingerPointsData.deviceFindState == DeviceFindState.Found)
				{
                    if (!handFingerPointsData.inputDevice.TryGetFeatureValue(CommonUsages.handData, out _handData))
					{
                        handFingerPointsData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(handFingerPointsData.FindDevice());
                    }
                }
                 else if (handFingerPointsData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handFingerPointsData.FindDevice());
                 
                return _handData;

                //return MLHandTracking.Right.Center
            }
        }
        #endregion


        #region -Featrues representing simple hand data-

        protected FeatureGroupDataSource handSimpleFeaturesData;
        
        protected Quaternion _handOrientation = Quaternion.identity;
        public virtual Quaternion handOrientation
        {
            get
            {
                if (handSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
				{
                    
                    if(!handSimpleFeaturesData.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out _handOrientation))
					{
                        handSimpleFeaturesData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(handSimpleFeaturesData.FindDevice());
                    }
                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.FindDevice());

                return _handOrientation;
            }
        }

        protected Vector3 _handCenterPosition;
        public Vector3 handCenterPosition
        {
            get
            {
                if (handSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
                {
                    if (!handSimpleFeaturesData.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out _handCenterPosition) &&
                        _handCenterPosition != Vector3.zero)
                    {
                        handSimpleFeaturesData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(handSimpleFeaturesData.FindDevice());
                    }
                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.FindDevice());

                return _handCenterPosition;
            }
        }
        #endregion



        protected void Start()
        {
            xrNodeType = XRNodeType.Hand;
            FormFeatureGroupDataSource();

        }

        protected virtual void FormFeatureGroupDataSource()
		{
            try
            {
                inputDeviceCharacteristics = (InputDeviceCharacteristics)((int)inputDeviceCharacteristics + (int)handedness + (int)InputDeviceCharacteristics.HandTracking);


                handFingerPointsData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { (InputFeatureUsage)CommonUsages.handData },
                    inputDeviceCharacteristics);
                handSimpleFeaturesData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { (InputFeatureUsage)CommonUsages.devicePosition, (InputFeatureUsage)CommonUsages.deviceRotation },
                    inputDeviceCharacteristics);
            }
            catch (Exception exc)
            {
                print(exc.Message);
            }

            //StartCoroutine(handFingerData.GetDevice());
            //StartCoroutine(handSimpleFeaturesData.GetDevice());
        }
    }
}