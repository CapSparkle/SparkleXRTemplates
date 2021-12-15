using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SparkleXRTemplates;
using UnityEngine.XR;
using UnityEngine.XR.MagicLeap;
using UnityEngine.XR.InteractionSubsystems;


namespace SparkleXRTemplates.MagicLeap
{
    

    //MagicLeapKeyPose
    //MagicLeapKeyPoseGestureEvent

    public class MLHandInputProvider : SimpleHandInputProvider
    {
        protected MLHandTracking.Hand MLHandDevice;
        
        List<Action> mySubscribers = new List<Action>();
     
        List<GestureState> gestureStates = new List<GestureState>();
        List<MLGestureMask> mlGestureMasks = new List<MLGestureMask>();

        protected override void FormFeatureGroupDataSource()
		{
            try
            {
                inputDeviceCharacteristics = (InputDeviceCharacteristics)((int)inputDeviceCharacteristics + (int)handedness + (int)InputDeviceCharacteristics.HandTracking);


                handFingerPointsData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { 
                    (InputFeatureUsage)CommonUsages.handData },
                    inputDeviceCharacteristics);
                
                handSimpleFeaturesData = new FeatureGroupDataSource(new List<InputFeatureUsage>() { 
                    (InputFeatureUsage)CommonUsages.devicePosition,
                    (InputFeatureUsage)CommonUsages.handData,
                    (InputFeatureUsage)MagicLeapHandUsages.WristCenter,
                    (InputFeatureUsage)MagicLeapHandUsages.WristRadial,
                    (InputFeatureUsage)MagicLeapHandUsages.WristUlnar,
                },
                    inputDeviceCharacteristics);
            
            }
            catch (Exception exc)
            {
                print(exc.Message);
            }
        }

        protected void Start()
        {
            base.Start();
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
                MLHandDevice = MLHandTracking.Right;
            else if (handedness == Handedness.Left)
                MLHandDevice = MLHandTracking.Left;

            MLHandDevice.OnHandKeyPoseBegin += NotifyBeginGestures;
            MLHandDevice.OnHandKeyPoseEnd += NotifyEndGestures;

            //MLHandTracking.Left.Wrist.Ulnar.
        }

        public Vector3 ulnarPosition;
        public Vector3 radialPosition;

        public override Quaternion handOrientation
		{
			get
			{
                if (handSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
                {
                    Vector3 centerPosition = handCenterPosition;

                    //The normal to the plane that is parallel to vector 3 of palm direction
                    Vector3 directionPlaneNormal;

                    Vector3 MCPThumbBonePosition;
                    List<Bone> bonesOut = new List<Bone>();
                    if (handData.TryGetFingerBones(HandFinger.Thumb, bonesOut))
					{
                        if ((bonesOut.Count == 5) && (bonesOut[2].TryGetPosition(out MCPThumbBonePosition)))
                        {
                            directionPlaneNormal = centerPosition - MCPThumbBonePosition;
                            directionPlaneNormal *= (handedness == Handedness.Left ? 1 : -1);
                        }
                    }
                    else
                    {
                        handSimpleFeaturesData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(handSimpleFeaturesData.GetDevice());
                        return _handOrientation;
                    }


                    /*if (handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristRadial, out radialPosition))
					{
                        directionPlaneNormal = centerPosition - radialPosition;
                    }
                    else if(handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristUlnar, out ulnarPosition))
					{
                        directionPlaneNormal = ulnarPosition - centerPosition;
                    }*/

                    

                    if (FindPositionOfAppropriateFingerPhalang(handData, out Vector3 phalangPosition))
					{
                        Vector3 forwardVector = Vector3.ProjectOnPlane(phalangPosition - centerPosition, directionPlaneNormal);
                        Vector3 upwardVector = Vector3.Cross(forwardVector, directionPlaneNormal);
                        _handOrientation =  Quaternion.LookRotation(forwardVector, upwardVector);
					}

                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.GetDevice());

                return _handOrientation;
			}
		}


        private bool FindPositionOfAppropriateFingerPhalang(Hand hand, out Vector3 bonePosition)
		{

            List<HandFinger> fingers = new List<HandFinger>()
            {
                HandFinger.Index,
                HandFinger.Middle,
                HandFinger.Ring,
                HandFinger.Pinky
            };


            List<Bone> bonesOut = new List<Bone>();

            foreach (HandFinger finger in fingers)
                if(hand.TryGetFingerBones(finger, bonesOut))
                    foreach (Bone phalang in bonesOut)
                        if (phalang.TryGetPosition(out bonePosition))
                            return true;

            //Only tip of thumb is an appropriate phalang to orientate on
            if (hand.TryGetFingerBones(HandFinger.Thumb, bonesOut))
                if ((bonesOut.Count == 5) && (bonesOut[4].TryGetPosition(out bonePosition)))
                    return true;

            bonePosition = Vector3.zero;
            return false;
		}


        private void Update()
		{
			/*if (mySubscribers != null && mySubscribers.Count != 0)
			{
                NotifyUpdatedGestures();
            }*/
		}

        public void NotifyUpdatedGestures()
        {
            for(int i = 0; i < gestureStates.Count; i ++)
			{
                if (gestureStates[i] == GestureState.Updated)
                {
                    /*if (mlgesturemasks[i].hasflag((mlgesturemask)math.pow(2.0, (int)myhand.keypose)))
                    {
                        subscribers[i].invoke();
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
                        mySubscribers[i].Invoke();
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
                        mySubscribers[i].Invoke();
                    }
                }
            }
        }

        public void AddGestureListener(Action methodListener, MLGestureMask mLGestureMask, GestureState gestureState)
        {
            if(mySubscribers.Contains(methodListener))
			{
                int indexToExtendSubscription = mySubscribers.FindIndex(0, methodListener.Equals);

                if(gestureStates[indexToExtendSubscription] == gestureState)
				{
                    //Extend mask
                    mlGestureMasks[indexToExtendSubscription] = (MLGestureMask)((int)mlGestureMasks[indexToExtendSubscription] | (int)mLGestureMask);
                    return;
                }
            }

            mySubscribers.Add(methodListener);
            mlGestureMasks.Add(mLGestureMask);
            gestureStates.Add(gestureState);
        }
        public void RemoveGestureListener(Action methodListener)
		{
            int indexToRemove = mySubscribers.FindIndex(0, methodListener.Equals);
            
            if(indexToRemove != -1)
			{
                mySubscribers.RemoveAt(indexToRemove);
                mlGestureMasks.RemoveAt(indexToRemove);
                gestureStates.RemoveAt(indexToRemove);
            }
        }
    }
}
