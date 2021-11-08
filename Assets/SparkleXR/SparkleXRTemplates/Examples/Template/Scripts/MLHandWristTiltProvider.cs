using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace SparkleXRTemplates
{
	public enum HandWristTilt
	{ 
        WristBentUp,
        WristBentDown,
        UndefindedTilt
    }


	public class MLHandWristTiltProvider : SimpleHandInputProvider
    {
        List<Action<float>> mySubscribers = new List<Action<float>>();
        List<HandWristTilt> tiltGestureState = new List<HandWristTilt>();

        [SerializeField]
        MLHandTracking.HandKeyPose TiltGesture = MLHandTracking.HandKeyPose.Fist;

        [SerializeField]
        float upBentMinAngle = 15f;

        [SerializeField]
        float downBentMinAngle = -15f;


        MLHandTracking.Hand handDevice;

        //float stopTiltGestureThreshold = 0.5f; //seconds till gesture become unrecognized since wrong key pose recognition has been occured
        
        float tiltAngle = 0f;

        Vector3 startHandDirection = Vector3.zero;
        
        protected Vector3 currentHandDirection
        {
            set
            {
            
            }

            get
			{
                return handDevice.Middle.MCP.Position - handDevice.Wrist.Center.Position;
            }
        }

        void Start()
        {
            base.Start();

            try
			{
                if (handedness == Handedness.Right)
                    handDevice = MLHandTracking.Right;
                else if (handedness == Handedness.Left)
                    handDevice = MLHandTracking.Left;
            }
            catch (Exception exc)
			{
                print(exc.Message);
			}
        }

        void RecognizeGesture()
		{
            if (startHandDirection == Vector3.zero)
            {
                if (handDevice.KeyPose == TiltGesture)
                {
                    startHandDirection = currentHandDirection;
                }
            }
            else
			{
                if (handDevice.KeyPose == TiltGesture)
                {
                    tiltAngle = Vector3.Angle(startHandDirection, Vector3.ProjectOnPlane(currentHandDirection, Vector3.Cross(startHandDirection, Vector3.up)));
                }
                else
				{
                    startHandDirection = Vector3.zero;
                    tiltAngle = 0f;
                }
            }

		}

        HandWristTilt currentGesture;

        void Update()
        {
            RecognizeGesture();
            if (tiltAngle > upBentMinAngle)
                currentGesture = HandWristTilt.WristBentUp;
            else if (tiltAngle < downBentMinAngle)
                currentGesture = HandWristTilt.WristBentDown;
            else
			{
                currentGesture = HandWristTilt.UndefindedTilt;
                return;
            }

            if(mySubscribers.Count != 0)
			{
                for(int i = 0; i < tiltGestureState.Count; i ++)
				{
                    if (tiltGestureState[i] == currentGesture)
					{
                        float degreesExceedingMinTiltThreshold = Mathf.Abs(tiltAngle - (currentGesture == HandWristTilt.WristBentUp ? upBentMinAngle : downBentMinAngle));
                        mySubscribers[i](degreesExceedingMinTiltThreshold);
					}
				}
			}
        }
    }
}



