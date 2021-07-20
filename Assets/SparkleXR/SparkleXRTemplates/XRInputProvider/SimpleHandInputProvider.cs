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

    //TODO: consistent naming!
	



	public class SimpleHandInputProvider : XRInputProvider
    {
        [SerializeField]
        protected Handedness handedness = Handedness.None;


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
            }
        }
        #endregion


        #region -Featrues representing simple hand data-

        FeatureGroupDataSource handSimpleFeaturesData;
        
        Quaternion _handOrientation;
        public Quaternion handOrientation
        {
            get
            {
                if (handSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
				{
                    
                    if(!handSimpleFeaturesData.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion _handOrientation))
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
                    if (!handSimpleFeaturesData.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 _handCenterPosition))
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



        void Awake()
        {
            xrNodeFeatureType = XRNodeType.Hand;

            handFingerData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { (InputFeatureUsage)CommonUsages.handData });
            handSimpleFeaturesData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { (InputFeatureUsage)CommonUsages.devicePosition, (InputFeatureUsage)CommonUsages.deviceRotation });
            
            StartCoroutine(handFingerData.GetDevice());
            StartCoroutine(handSimpleFeaturesData.GetDevice());
        }
    }
}