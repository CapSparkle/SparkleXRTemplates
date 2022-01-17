using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
#endif

#if PLATFORM_UWP

#endif

namespace SparkleXRTemplates
{
    public class HeadInputProvider : XRInputProvider
    {

        #region -Feature representing eye gaze data-

        //Vector3 _eyeFixationPoint;


        #if PLATFORM_LUMIN
        public Vector3 eyeFixationPoint
        {
            get
            {
                if (MLEyes.CalibrationStatus == MLEyes.Calibration.Good)
                {
                    return MLEyes.FixationPoint;
                }

                return Vector3.up;
            }
            set
            {

            }
        }

        #endif



        #endregion

        #region -Featrues representing simple head data-

        FeatureGroupDataSource headSimpleFeaturesData;

        Quaternion _headOrientation;
        public Quaternion headOrientation
        {
            get
            {
                if (headSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
                {

                    if (!headSimpleFeaturesData.inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out _headOrientation))
                    {
                        headSimpleFeaturesData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(headSimpleFeaturesData.FindDevice());
                    }
                }
                else if (headSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(headSimpleFeaturesData.FindDevice());

                return _headOrientation;
            }
        }

        Vector3 _headPosition;
        public Vector3 headPosition
        {
            get
            {
                if (headSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
                {
                    if (!headSimpleFeaturesData.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out _headPosition))
                    {
                        headSimpleFeaturesData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(headSimpleFeaturesData.FindDevice());
                    }
                }
                else if (headSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(headSimpleFeaturesData.FindDevice());

                return _headPosition;
            }
        }
    #endregion





        protected void Start()
        {
            xrNodeType = XRNodeType.HMD;

            inputDeviceCharacteristics = (InputDeviceCharacteristics)((int)inputDeviceCharacteristics + (int)InputDeviceCharacteristics.HeadMounted);

            headSimpleFeaturesData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { (InputFeatureUsage)CommonUsages.devicePosition, (InputFeatureUsage)CommonUsages.deviceRotation },
                inputDeviceCharacteristics);

            StartCoroutine(headSimpleFeaturesData.FindDevice());
        }
    }
}