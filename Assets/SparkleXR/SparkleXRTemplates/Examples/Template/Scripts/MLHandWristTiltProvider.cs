using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace SparkleXRTemplates.MagicLeap
{
	public enum TiltGesture
	{ 
        BentUp,
        BentDown,
        UndefindedTilt
    }


	public class MLHandWristTiltProvider : MLHandInputProvider
    {

        List<Action<float>> mySubscribers;
        List<TiltGesture> tiltGestureState;

        public void AddGestureListener(Action<float> newSubscriber, TiltGesture handWristTilt)
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

            mySubscribers = new List<Action<float>>();
            tiltGestureState = new List<TiltGesture>();
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

        TiltGesture currentGesture;

        void Update()
        {
            //lineRend1.SetPosition(0, handCenterPosition);
            //lineRend1.SetPosition(1, handCenterPosition + currentHandDirection);

            base.Update();
            RecognizeTiltGesture();
            if (tiltAngle > upBentMinAngle)
				currentGesture = MagicLeap.TiltGesture.BentUp;
            else if (tiltAngle < downBentMinAngle)
				currentGesture = MagicLeap.TiltGesture.BentDown;
            else
			{
				currentGesture = MagicLeap.TiltGesture.UndefindedTilt;
                return;
            }

            if(mySubscribers.Count != 0)
			{
                for(int i = 0; i < tiltGestureState.Count; i ++)
				{
                    if (tiltGestureState[i] == currentGesture)
					{
                        float degreesExceedingMinTiltThreshold = Mathf.Abs(tiltAngle - (currentGesture == MagicLeap.TiltGesture.BentUp ? upBentMinAngle : downBentMinAngle));
                        mySubscribers[i](degreesExceedingMinTiltThreshold);
					}
				}
			}
        }
    }
}



