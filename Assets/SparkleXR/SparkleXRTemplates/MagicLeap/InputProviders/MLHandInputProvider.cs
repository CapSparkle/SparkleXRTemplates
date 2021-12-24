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
                    (InputFeatureUsage)MagicLeapHandUsages.WristCenter,
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
        }

        public Vector3 previousPos;
        public Vector3 _wristCenterPosition = Vector3.zero;

        public Vector3 previousDir;
        protected Vector3 _handDirection = Vector3.zero;
        public Vector3 handDirection
        {
            get
            {
                if (handSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
                {
                    Vector3 centerPosition = handCenterPosition;

                    if (handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristCenter, out Vector3 newWristCenterPosition) &&
                        newWristCenterPosition != handCenterPosition)
                    {
                        previousPos = _wristCenterPosition;
                        _wristCenterPosition = newWristCenterPosition;
                        //print("some wrist data" + _wristCenterPosition.ToString());
                    }
                    else
                    {
                        print("no MagicLeapHandUsages.WristCenter data presented. Old value in use");
                    }

                    previousDir = _handDirection;
                    _handDirection = Vector3.Normalize(centerPosition - _wristCenterPosition);
                    if (_handDirection.magnitude == 0)
                        print("zero direction");
                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.GetDevice());
                else
                    print("Appropriate device is being finding. Returning old value of handDirection variable");

                return _handDirection;
            }
        }


        //public for debug purposes
        public Vector3 MPCThumbPosition;

        public override Quaternion handOrientation
        {
            get
            {
                if (handSimpleFeaturesData.deviceFindState == DeviceFindState.Found)
                {
                    List<Bone> bonesOut = new List<Bone>();

                    if (handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristCenter, out Vector3 handWristCenter) &&
                        handData.TryGetFingerBones(HandFinger.Thumb, bonesOut) &&
                        bonesOut.Count == 5 &&
                        bonesOut[2].TryGetPosition(out MPCThumbPosition) &&
                        MPCThumbPosition != handCenterPosition &&
                        handWristCenter != handCenterPosition &&
                        handCenterPosition != Vector3.zero)
                    {
                        Plane plane = new Plane();
                        plane.Set3Points(MPCThumbPosition, handWristCenter, handCenterPosition);

                        if((handDirection.magnitude == 0) || plane.normal.magnitude == 0)
						{
                            print(_wristCenterPosition.ToString() + " - Wrist| " + handCenterPosition.ToString() + " - HCP| " + MPCThumbPosition.ToString() + " - Thumb| ");
						}
                            _handOrientation = Quaternion.LookRotation(handDirection, plane.normal);
                    }
                    else
					{
                        print("some data to compute hand orientation isn't presented. Old value in use");
                    }
                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.GetDevice());
                else
                    print("Appropriate device is being finding. Returning old value of handOrientation variable");


                return _handOrientation;

            }
        }

        //Save for some time
        /*void TakeAndHandleHandFeatureData()
		{
            List<Bone> bonesOut = new List<Bone>();

            //thumbMCPPosition
            if (handData.TryGetFingerBones(HandFinger.Thumb, bonesOut))
            {
                if ((bonesOut.Count == 5) && (bonesOut[2].TryGetPosition(out Vector3 newThumbMCPPosition)))
                    thumbMCPPosition = newThumbMCPPosition;
                else
                    OnDataNotProvidedByDataSource(handSimpleFeaturesData, CommonUsages.handData);
            }

            if (handData.TryGetFingerBones(HandFinger.Index, bonesOut))
            {
                if ((bonesOut.Count == 5) && (bonesOut[2].TryGetPosition(out Vector3 newIndexMCPPosition)))
                    indexMCPPosition = newIndexMCPPosition;
                else
                    OnDataNotProvidedByDataSource(handSimpleFeaturesData, CommonUsages.handData);
            }

            if (handData.TryGetFingerBones(HandFinger.Middle, bonesOut))
            {
                if ((bonesOut.Count == 5) && (bonesOut[2].TryGetPosition(out Vector3 newRingMCPPosition)))
                    ringMCPPosition = newRingMCPPosition;
                else
                    OnDataNotProvidedByDataSource(handSimpleFeaturesData, CommonUsages.handData);
            }

            if (handData.TryGetFingerBones(HandFinger.Middle, bonesOut))
            {
                if ((bonesOut.Count == 5) && (bonesOut[2].TryGetPosition(out Vector3 newMiddleMCPPosition)))
                    middleMCPPosition = newMiddleMCPPosition;
                else
                    OnDataNotProvidedByDataSource(handSimpleFeaturesData, CommonUsages.handData);
            }

            //wristCenter
            if (handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristCenter, out Vector3 newWristCenter))
                wristCenter = newWristCenter;
            else
                OnDataNotProvidedByDataSource(handSimpleFeaturesData, MagicLeapHandUsages.WristCenter);

            //Update managed keypoint values used
            thumbMCP.Update(handFingerPointsData.inputDevice, thumbMCPPosition, indexMCPPosition, handCenterPosition);

        }

        void OnDataNotProvidedByDataSource<T>(FeatureGroupDataSource FGDataSource, InputFeatureUsage<T> featureNotProvided)
		{
            Debug.Log("The value of feature: " + featureNotProvided.name + " have not been provided at all or have not been provided correctly by device: " + FGDataSource.inputDevice.name);
            if (FGDataSource.deviceFindState != DeviceFindState.Found)
			{
                FGDataSource.deviceFindState = DeviceFindState.NotFound;
                StartCoroutine(FGDataSource.GetDevice());
            }
        }
        */

        private void Update()
		{
			if (mySubscribers != null && mySubscribers.Count != 0)
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
