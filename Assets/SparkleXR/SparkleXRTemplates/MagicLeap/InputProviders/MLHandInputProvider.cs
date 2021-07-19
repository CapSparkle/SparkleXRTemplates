using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SparkleXRTemplates;
using UnityEngine.XR;
using UnityEngine.XR.MagicLeap;
using UnityEngine.XR.InteractionSubsystems;


namespace SparkleXRLib.MagicLeap
{
    

    //MagicLeapKeyPose
    //MagicLeapKeyPoseGestureEvent

    public class MLHandInputProvider : SimpleHandInputProvider
    {
        MLHandTracking.Hand handGesturesDevice;
        
        List<Action> subscribers = new List<Action>();
        /*public List<Action> subscribers
        {
            get
            {
                return _subscribers;

            }
            protected set
            {
                _subscribers = value;

                if (_subscribers == null || _subscribers.Count == 0)
                {
                    if(notificationCoroutine != null)
					{

					}
				}
                else
				{
                    if (notificationCoroutine != null)
                    {

                    }
                }
            }
        }*/

        List<GestureState> gestureStates = new List<GestureState>();
        List<MLGestureMask> mlGestureMasks = new List<MLGestureMask>();

        private void Start()
        {
            #region -enable key poses-
            //TODO: bring that code block to the separate class

            List<MLHandTracking.HandKeyPose> defaultEnabledKeyPoses = new List<MLHandTracking.HandKeyPose> {
            MLHandTracking.HandKeyPose.Fist,
            MLHandTracking.HandKeyPose.C,
            MLHandTracking.HandKeyPose.Ok,
            MLHandTracking.HandKeyPose.OpenHand,
            MLHandTracking.HandKeyPose.Pinch,
            MLHandTracking.HandKeyPose.Finger,
            MLHandTracking.HandKeyPose.L,

            MLHandTracking.HandKeyPose.NoPose,
            MLHandTracking.HandKeyPose.NoHand
        };

            MLHandTracking.KeyPoseManager.EnableKeyPoses(defaultEnabledKeyPoses.ToArray(), true);

            #endregion


            if (handedness == Handedness.Right)
                handGesturesDevice = MLHandTracking.Right;
            else if (handedness == Handedness.Left)
                handGesturesDevice = MLHandTracking.Left;

            handGesturesDevice.OnHandKeyPoseBegin += NotifyBeginGestures;
            handGesturesDevice.OnHandKeyPoseEnd += NotifyEndGestures;
        }
		private void Update()
		{
			if (subscribers != null && subscribers.Count != 0)
			{
                NotifyUpdatedGestures();
            }
		}

        public void NotifyUpdatedGestures()
        {
            for(int i = 0; i < gestureStates.Count; i ++)
			{
                if (gestureStates[i] == GestureState.Updated)
                {
                   /* if (mlGestureMasks[i].HasFlag((MLGestureMask)Math.Pow(2.0, (int)myHand.KeyPose)))
                    {
                        subscribers[i].Invoke();
                    }*/
                }
            }
        }
		public void NotifyBeginGestures(MLHandTracking.HandKeyPose keyPose)
        {
            for(int i = 0; i < gestureStates.Count; i ++)
			{
                if(gestureStates[i] == GestureState.Started)
				{
                    if(mlGestureMasks[i].HasFlag((MLGestureMask)Math.Pow(2.0, (int)keyPose)))
					{
                        subscribers[i].Invoke();
                    }
				}
			}
        }
        public void NotifyEndGestures(MLHandTracking.HandKeyPose keyPose)
        {
            for (int i = 0; i < gestureStates.Count; i++)
            {
                if ((gestureStates[i] == GestureState.Completed) || gestureStates[i] == GestureState.Canceled)
                {
                    if (mlGestureMasks[i].HasFlag((MLGestureMask)Math.Pow(2.0, (int)keyPose)))
                    {
                        subscribers[i].Invoke();
                    }
                }
            }
        }

        public void AddGestureListener(Action methodListener, MLGestureMask mLGestureMask, GestureState gestureState)
        {
            if(subscribers.Contains(methodListener))
			{
                int indexToExtendSubscription = subscribers.FindIndex(0, methodListener.Equals);

                if(gestureStates[indexToExtendSubscription] == gestureState)
				{
                    //Extend mask
                    mlGestureMasks[indexToExtendSubscription] = (MLGestureMask)((int)mlGestureMasks[indexToExtendSubscription] | (int)mLGestureMask);
                    return;
                }
            }

            subscribers.Add(methodListener);
            mlGestureMasks.Add(mLGestureMask);
            gestureStates.Add(gestureState);
        }
        public void RemoveGestureListener(Action methodListener)
		{
            int indexToRemove = subscribers.FindIndex(0, methodListener.Equals);
            
            if(indexToRemove != -1)
			{
                subscribers.RemoveAt(indexToRemove);
                mlGestureMasks.RemoveAt(indexToRemove);
                gestureStates.RemoveAt(indexToRemove);
            }
        }
    }
}
