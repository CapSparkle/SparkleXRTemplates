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
    enum Handedness
    {
        None,
        Left = 256,
        Right = 512
    }

    //MagicLeapKeyPose
    //MagicLeapKeyPoseGestureEvent

    public class MLHandInputProvider : XRInputProvider
    {
        [SerializeField]
        Handedness handedness = Handedness.None;

        List<Action> methods;
        List<GestureState> gestureStates;
        List<MLGestureMask> mlGestureMasks;


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

            xrNodeFeatureGroup = XRNodeFeatureGroup.Hand;


            if (handedness == Handedness.Right)
            {
                MLHandTracking.Right.OnHandKeyPoseBegin += HandleBeginGestures;
                MLHandTracking.Right.OnHandKeyPoseEnd += HandleEndGestures;
            }
            else if (handedness == Handedness.Left)
            {
                MLHandTracking.Left.OnHandKeyPoseBegin += HandleBeginGestures;
                MLHandTracking.Left.OnHandKeyPoseEnd += HandleEndGestures;
            }
        }

        public void HandleBeginGestures(MLHandTracking.HandKeyPose keyPose)
        {
            for(int i = 0; i < gestureStates.Count; i ++)
			{
                if(gestureStates[i] == GestureState.Started)
				{
                    if(mlGestureMasks[i].HasFlag((MLGestureMask)Math.Pow(2.0, (int)keyPose)))
					{
                        methods[i].Invoke();
                    }
				}
			}
        }
        public void HandleEndGestures(MLHandTracking.HandKeyPose keyPose)
        {
            for (int i = 0; i < gestureStates.Count; i++)
            {
                if ((gestureStates[i] == GestureState.Completed) || gestureStates[i] == GestureState.Canceled)
                {
                    if (mlGestureMasks[i].HasFlag((MLGestureMask)Math.Pow(2.0, (int)keyPose)))
                    {
                        methods[i].Invoke();
                    }
                }
            }
        }

        public void AddGestureListener(Action methodListener, MLGestureMask mLGestureMask, GestureState gestureState)
        {
        }

        public void RemoveGestureListener(Action methodListener)
		{
		}
    }
}
