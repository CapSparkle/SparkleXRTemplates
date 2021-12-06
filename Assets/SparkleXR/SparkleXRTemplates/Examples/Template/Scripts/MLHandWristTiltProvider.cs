using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace SparkleXRTemplates.MagicLeap
{
	public enum HandWristTilt
	{ 
        WristBentUp,
        WristBentDown,
        UndefindedTilt
    }


	public class MLHandWristTiltProvider : MLHandInputProvider
    {
        List<Action<float>> mySubscribers = new List<Action<float>>();
        List<HandWristTilt> tiltGestureState = new List<HandWristTilt>();

        public void AddGestureListener(Action<float> newSubscriber, HandWristTilt handWristTilt)
		{
            mySubscribers.Add(newSubscriber);
            tiltGestureState.Add(handWristTilt);
        }

        public void RemoveGestureListener(Action<float> goingOutSubscriber)
        {
            int indexOfGoingOutSubscriber = mySubscribers.IndexOf(goingOutSubscriber);
            mySubscribers.RemoveAt(indexOfGoingOutSubscriber);
            tiltGestureState.RemoveAt(indexOfGoingOutSubscriber);
        }

        [SerializeField]
        MLHandTracking.HandKeyPose TiltGesture = MLHandTracking.HandKeyPose.Fist;

        [SerializeField]
        float upBentMinAngle = 15f;

        [SerializeField]
        float downBentMinAngle = -15f;


        MLHandTracking.Hand _handDevice;
        MLHandTracking.Hand handDevice
		{
			get
			{
                return _handDevice;
            }
            set
			{
                _handDevice = value;
			}
		}

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
                return (handDevice.Middle.MCP.Position - handDevice.Wrist.Center.Position).normalized;
            }
        }

        void Start()
        {
            base.Start();
            GetHandDevice();
        }

        void GetHandDevice()
		{
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

        void RecognizeTiltAngle()
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
                    Vector3 projectionOnVerticalPlane = Vector3.ProjectOnPlane(currentHandDirection, Vector3.Cross(startHandDirection, Vector3.up));
                    tiltAngle = Vector3.Angle(startHandDirection, projectionOnVerticalPlane);
                    tiltAngle *= Mathf.Sign((projectionOnVerticalPlane - startHandDirection).y);
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
            RecognizeTiltAngle();
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



