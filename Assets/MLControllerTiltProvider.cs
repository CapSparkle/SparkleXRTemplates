using System;
using System.Collections.Generic;
using UnityEngine;


#if PLATFORM_LUMIN
namespace SparkleXRTemplates.MagicLeap
{
	public enum ControllerPressAction
	{ 
        Bumper,
        Trigger,
        PadTap
    }


	public class MLControllerTiltProvider : MLControllerInputProvider
    {

        List<Action<float>> mySubscribers;
        List<TiltGesture> tiltGestureState;

        public void AddGestureListener(Action<float> newSubscriber, TiltGesture tiltGesture)
        {
            mySubscribers.Add(newSubscriber);
            tiltGestureState.Add(tiltGesture);
        }

        public void RemoveGestureListener(Action<float> goingOutSubscriber)
        {
            int indexOfGoingOutSubscriber = mySubscribers.IndexOf(goingOutSubscriber);
            mySubscribers.RemoveAt(indexOfGoingOutSubscriber);
            tiltGestureState.RemoveAt(indexOfGoingOutSubscriber);
        }

        [SerializeField]
        ControllerPressAction pressActionOfGesture = ControllerPressAction.Bumper;

        [SerializeField]
        float upBentMinAngle = 15f;

        [SerializeField]
        float downBentMinAngle = -15f;


        float tiltAngle = 0f;

        Vector3 startControlerDirection = Vector3.zero;

        protected Vector3 currentControllerDirection
        {
            get
            {
                return controllerOrientation * Vector3.forward;
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

        bool isPressActionHappened()
		{
            switch(pressActionOfGesture)
			{
                case (ControllerPressAction.Bumper):
                    if (bumperPressed)
                        return true;
                    else
                        return false;

                case (ControllerPressAction.Trigger):
                    if (triggerValue > 0.95)
                        return true;
                    else
                        return false;

                case (ControllerPressAction.PadTap):
                    if (touchPadTouch)
                        return true;
                    else
                        return false;
            }

            return false;
		}

        void RecognizeTiltGesture()
        {
            if (startControlerDirection == Vector3.zero)
            {
                if (isPressActionHappened())
                {
                    startControlerDirection = currentControllerDirection;
                    lineRend1.SetPosition(0, controllerPosition);
                    lineRend1.SetPosition(1, controllerPosition + startControlerDirection);
                }
            }
            else
            {
                if (isPressActionHappened())
                {
                    Vector3 projectionOnVerticalPlane = Vector3.ProjectOnPlane(currentControllerDirection, Vector3.Cross(startControlerDirection, Vector3.up)).normalized;

                    lineRend2.SetPosition(0, controllerPosition);
                    lineRend2.SetPosition(1, controllerPosition + projectionOnVerticalPlane);

                    tiltAngle = Vector3.Angle(startControlerDirection, projectionOnVerticalPlane);
                    tiltAngle *= Mathf.Sign((projectionOnVerticalPlane - startControlerDirection).y);
                }
                else
                {
                    startControlerDirection = Vector3.zero;
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

            base.Update();
            RecognizeTiltGesture();
            if (tiltAngle > upBentMinAngle)
                currentGesture = TiltGesture.BentUp;
            else if (tiltAngle < downBentMinAngle)
                currentGesture = TiltGesture.BentDown;
            else
            {
                currentGesture = TiltGesture.UndefindedTilt;
                return;
            }

            if (mySubscribers.Count != 0)
            {
                for (int i = 0; i < tiltGestureState.Count; i++)
                {
                    if (tiltGestureState[i] == currentGesture)
                    {
                        float degreesExceedingMinTiltThreshold = Mathf.Abs(tiltAngle - (currentGesture == TiltGesture.BentUp ? upBentMinAngle : downBentMinAngle));
                        mySubscribers[i](degreesExceedingMinTiltThreshold);
                    }
                }
            }
        }
    }
}
#endif
