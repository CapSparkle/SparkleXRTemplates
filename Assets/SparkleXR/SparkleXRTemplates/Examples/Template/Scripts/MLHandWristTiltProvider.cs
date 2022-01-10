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

        List<Action<float>> mySubscribers;
        List<HandWristTilt> tiltGestureState;

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

        
        float tiltAngle = 0f;

        Vector3 startHandDirection = Vector3.zero;
        
        protected Vector3 currentHandDirection
        {
            set
            {
            
            }

            get
			{
                return handDirection;
            }
        }

        void Start()
        {
            base.Start();

            print("CLEAR VARIABLES WRISTCONTROLLER");

            mySubscribers = new List<Action<float>>();
            tiltGestureState = new List<HandWristTilt>();
        }

		[SerializeField]
        LineRenderer lineRend1, lineRend2;

        void RecognizeTiltGesture()
		{
            if (startHandDirection == Vector3.zero)
            {
                if (MLHandDevice.KeyPose == TiltGesture)
                {
                    startHandDirection = currentHandDirection;
                    lineRend1.SetPosition(0, handCenterPosition);
                    lineRend1.SetPosition(1, handCenterPosition + startHandDirection);
                }
            }
            else
			{
                if (MLHandDevice.KeyPose == TiltGesture)
                {
                    Vector3 projectionOnVerticalPlane = Vector3.ProjectOnPlane(currentHandDirection, Vector3.Cross(startHandDirection, Vector3.up)).normalized;

                    lineRend2.SetPosition(0, handCenterPosition);
                    lineRend2.SetPosition(1, handCenterPosition + projectionOnVerticalPlane);

                    tiltAngle = Vector3.Angle(startHandDirection, projectionOnVerticalPlane);
                    tiltAngle *= Mathf.Sign((projectionOnVerticalPlane - startHandDirection).y);
                }
                else
				{
                    startHandDirection = Vector3.zero;
                    tiltAngle = 0f;
                    
                    lineRend1.SetPosition(0, Vector3.zero);
                    lineRend1.SetPosition(1, Vector3.zero);

                    lineRend2.SetPosition(0, Vector3.zero);
                    lineRend2.SetPosition(1, Vector3.zero);
                }
            }

		}

        HandWristTilt currentGesture;

        void Update()
        {
            //lineRend1.SetPosition(0, handCenterPosition);
            //lineRend1.SetPosition(1, handCenterPosition + currentHandDirection);

            base.Update();
            RecognizeTiltGesture();
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



