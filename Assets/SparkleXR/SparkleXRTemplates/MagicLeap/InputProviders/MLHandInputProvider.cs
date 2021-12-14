using System;
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

                    if(handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristRadial, out radialPosition))
					{
                        directionPlaneNormal = radialPosition - centerPosition;
                    }
                    else if(handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristUlnar, out ulnarPosition))
					{
                        directionPlaneNormal = ulnarPosition - centerPosition;
                    }
                    else
					{
                        handSimpleFeaturesData.deviceFindState = DeviceFindState.NotFound;
                        StartCoroutine(handSimpleFeaturesData.GetDevice());
                        return _handOrientation;
                    }

                    handData



                    if (!handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristRadial, out ulnarPosition)
                        && !handSimpleFeaturesData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.WristUlnar, out radialPosition))
                    {
                        
                    }
                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.GetDevice());

                return _handOrientation;


                //Vector3 centerPosition = this.handCenterPosition;

                //Vector3 ulnarPosition = handSimpleFeaturesData.de; 
                //Vector3 radialPosition = Vector3.zero;

               /* try
				{
                    ulnarPosition = MLHandDevice.Wrist.Ulnar.Position;
                    print("ulnar position = " + ulnarPosition);
                    radialPosition = MLHandDevice.Wrist.Radial.Position;
                    print("radial position = " + radialPosition);
                }
                catch (Exception Exc)
				{
                    print(Exc.Message);
                }*/


                return Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, (centerPosition - ulnarPosition)));
			}
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
