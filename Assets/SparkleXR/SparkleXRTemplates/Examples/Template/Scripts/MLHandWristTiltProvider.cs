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
        List<Action> mySubscribers = new List<Action>();

        [SerializeField]
        MLHandTracking.HandKeyPose TiltGesture = MLHandTracking.HandKeyPose.Fist;

        [SerializeField]
        float upBentMinAngle = 15f;

        [SerializeField]
        float downBentMinAngle = 15f;


        MLHandTracking.Hand handDevice;

        float stopTiltGestureThreshold = 0.5f; //seconds till gesture become unrecognized since wrong key pose recognition

        float tiltAngle = 0f;
        
        Pose tiltStartPose = Pose.identity;

        HandWristTilt _wristTilt = HandWristTilt.UndefindedTilt;
        public HandWristTilt wristTilt
        {
            set
			{
                //(handDevice.Middle.MCP.Position - handDevice.Wrist.Center.Position)

            }

            get
			{
                return _wristTilt;
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
            if (tiltStartPose == Pose.identity)
            {
                if (handDevice.KeyPose == TiltGesture)
                {
                    //Gesture started
                }
            }
            else
			{
                if (handDevice.KeyPose == TiltGesture)
                {

                }
                else
				{
                    if (handDevice.KeyPose == TiltGesture)
                    {
                        //Gesture longer presented
                    }
                }
            }

		}
        
        void Update()
        {
            if(mySubscribers.Count != 0)
			{
                
			}
        }
    }
}



