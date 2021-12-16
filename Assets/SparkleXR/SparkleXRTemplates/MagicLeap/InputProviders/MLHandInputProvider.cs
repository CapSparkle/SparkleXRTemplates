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
                        //Vector3 forwardVector = Vector3.ProjectOnPlane(phalangPosition - centerPosition, directionPlaneNormal);
                        //Vector3 upwardVector = Vector3.Cross(forwardVector, directionPlaneNormal);
                        //_handOrientation =  Quaternion.LookRotation(forwardVector, upwardVector);
					}

                }
                else if (handSimpleFeaturesData.deviceFindState == DeviceFindState.NotFound)
                    StartCoroutine(handSimpleFeaturesData.GetDevice());

                return _handOrientation;
			}
		}

        Vector3 thumbMCPPosition;
        Vector3 indexMCPPosition;
        Vector3 middleMCPPosition;
        Vector3 ringMCPPosition;

        Vector3 wristCenter;

        void TakeHandFeatureData()
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

        Quaternion rotationOfHand;
        Camera _mainCamera;

        public void CalculateRotation()
        {

            if (handFingerPointsData.inputDevice.TryGetFeatureValue(MagicLeapHandUsages.Confidence, out float deviceHandConfidence))
                if (deviceHandConfidence < 0.85f)
                    return;

            //correct distances:
            float thumbMcpToWristDistance = Vector3.Distance(thumbMCPPosition, wristCenter) * .5f;
            //fix the distance between the wrist and thumbMcp as it incorrectly expands as the hand gets further from the camera:
            float distancePercentage = Mathf.Clamp01(Vector3.Distance(_mainCamera.transform.position, wristCenter) / .5f);
            distancePercentage = 1 - Percentage(distancePercentage, .90f, 1) * .4f;
            thumbMcpToWristDistance *= distancePercentage;
            Vector3 wristToPalmDirection = Vector3.Normalize(Vector3.Normalize(handCenterPosition - wristCenter));
            Vector3 center = wristCenter + (wristToPalmDirection * thumbMcpToWristDistance);
            Vector3 camToWristDirection = Vector3.Normalize(wristCenter - _mainCamera.transform.position);

            //rays needed for planarity discovery for in/out palm facing direction:
            Vector3 camToWrist = new Ray(wristCenter, camToWristDirection).GetPoint(1);
            Vector3 camToThumbMcp = new Ray(thumbMCPPosition, Vector3.Normalize(thumbMCPPosition - _mainCamera.transform.position)).GetPoint(1);
            Vector3 camToPalm = new Ray(center, Vector3.Normalize(center - _mainCamera.transform.position)).GetPoint(1);

            //discover palm facing direction to camera:
            Plane palmFacingPlane = new Plane(camToWrist, camToPalm, camToThumbMcp);
            if (handedness == Handedness.Left)
            {
                palmFacingPlane.Flip();
            }
            float palmForwardFacing = Mathf.Sign(Vector3.Dot(palmFacingPlane.normal, _mainCamera.transform.forward));

            //use thumb/palm/wrist alignment to determine amount of roll in the hand:
            Vector3 toThumbMcp = Vector3.Normalize(thumbMCPPosition - center);
            Vector3 toPalm = Vector3.Normalize(center - wristCenter);
            float handRollAmount = (1 - Vector3.Dot(toThumbMcp, toPalm)) * palmForwardFacing;

            //where between the wrist and thumbMcp should we slide inwards to get the palm in the center:
            Vector3 toPalmOrigin = Vector3.Lerp(wristCenter, thumbMCPPosition, .35f);

            //get a direction from the camera to toPalmOrigin as psuedo up for use in quaternion construction:
            Vector3 toCam = Vector3.Normalize(_mainCamera.transform.position - toPalmOrigin);

            //construct a quaternion that helps get angles needed between the wrist and thumbMCP to point towards the palm center:
            Vector3 wristToThumbMcp = Vector3.Normalize(thumbMCPPosition - wristCenter);
            Quaternion towardsCamUpReference = Quaternion.identity;
            if (wristToThumbMcp != Vector3.zero && toCam != Vector3.zero)
            {
                towardsCamUpReference = Quaternion.LookRotation(wristToThumbMcp, toCam);
            }

            //rotate the inwards vector depending on hand roll to know where to push the palm back:
            float inwardsVectorRotation = 90;
            if (handedness == Handedness.Left)
            {
                inwardsVectorRotation = -90;
            }
            towardsCamUpReference = Quaternion.AngleAxis(handRollAmount * inwardsVectorRotation, towardsCamUpReference * Vector3.forward) * towardsCamUpReference;
            Vector3 inwardsVector = towardsCamUpReference * Vector3.up;

            //slide palm location along inwards vector to get it into proper physical location in the center of the hand:
            center = toPalmOrigin - inwardsVector * thumbMcpToWristDistance;
            Vector3 deadCenter = center;

            //as the hand flattens back out balance corrected location with originally provided location for better forward origin:
            center = Vector3.Lerp(center, handCenterPosition, Mathf.Abs(handRollAmount));

            //get a forward using the corrected palm location:
            Vector3 forward = Vector3.Normalize(center - wristCenter);

            //switch back to physical center of hand - this reduces surface-to-surface movement of the center between back and palm:
            center = deadCenter;

            //get an initial hand up:
            Plane handPlane = new Plane(wristCenter, thumbMCPPosition, center);
            if (handedness == Handedness.Left)
            {
                handPlane.Flip();
            }
            Vector3 up = handPlane.normal;

            //find out how much the back of the hand is facing the camera so we have a safe set of features for a stronger forward: 
            Vector3 centerToCam = Vector3.Normalize(_mainCamera.transform.position - wristCenter);
            float facingDot = Vector3.Dot(centerToCam, up);

            if (facingDot > .5f)
            {
                float handBackFacingCamAmount = Percentage(facingDot, .5f, 1);

                //steer forward for more accuracy based on the visibility of the back of the hand:
                if (middleMCPPosition.Visible)
                {
                    Vector3 toMiddleMcp = Vector3.Normalize(middleMCPPosition - center);
                    forward = Vector3.Lerp(forward, toMiddleMcp, handBackFacingCamAmount);
                }
                else if (indexMCPPosition.Visible)
                {
                    Vector3 inIndexMcp = Vector3.Normalize(indexMCPPosition - center);
                    forward = Vector3.Lerp(forward, inIndexMcp, handBackFacingCamAmount);
                }
            }

            //make sure palm distance from wrist is consistant while also leveraging steered forward:
            center = wristCenter + (forward * thumbMcpToWristDistance);

            //an initial rotation of the hand:
            Quaternion orientation = Quaternion.identity;
            if (forward != Vector3.zero && up != Vector3.zero)
            {
                orientation = Quaternion.LookRotation(forward, up);
            }

            //as the hand rolls counter-clockwise the thumbMcp loses accuracy so we need to interpolate to the back of the hand's features:
            if (indexMCPPosition.Visible && middleMCPPosition.Visible)
            {
                Vector3 knucklesVector = Vector3.Normalize(middleMCPPosition - indexMCPPosition);
                float knucklesDot = Vector3.Dot(knucklesVector, Vector3.up);
                if (knucklesDot > .5f)
                {
                    float counterClockwiseRoll = Percentage(Vector3.Dot(knucklesVector, Vector3.up), .35f, .7f);
                    center = Vector3.Lerp(center, handCenterPosition, counterClockwiseRoll);
                    forward = Vector3.Lerp(forward, Vector3.Normalize(middleMCPPosition - handCenterPosition), counterClockwiseRoll);
                    Plane backHandPlane = new Plane(handCenterPosition, indexMCPPosition, middleMCPPosition);
                    if (handedness == Handedness.Left)
                    {
                        backHandPlane.Flip();
                    }
                    up = Vector3.Lerp(up, backHandPlane.normal, counterClockwiseRoll);
                    orientation = Quaternion.LookRotation(forward, up);
                }
            }

            //as the wrist tilts away from the camera (with the thumb down) at extreme angles the hand center will move toward the thumb:
            float handTiltAwayAmount = 1 - Percentage(Vector3.Distance(handCenterPosition, wristCenter), .025f, .04f);
            Vector3 handTiltAwayCorrectionPoint = wristCenter + camToWristDirection * thumbMcpToWristDistance;
            center = Vector3.Lerp(center, handTiltAwayCorrectionPoint, handTiltAwayAmount);
            forward = Vector3.Lerp(forward, Vector3.Normalize(handTiltAwayCorrectionPoint - wristCenter), handTiltAwayAmount);
            Plane wristPlane = new Plane(wristCenter, thumbMCPPosition, center);
            if (handedness == Handedness.Left)
            {
                wristPlane.Flip();
            }
            up = Vector3.Lerp(up, wristPlane.normal, handTiltAwayAmount);
            if (forward != Vector3.zero && up != Vector3.zero)
            {
                orientation = Quaternion.LookRotation(forward, up);
            }

            //steering for if thumb/index are not available from self-occlusion to help rotate the hand better outwards better:
            float forwardUpAmount = Vector3.Dot(forward, Vector3.up);
            if (forwardUpAmount > .7f && indexMCPPosition.Visible && ringMCPPosition.Visible)
            {
                float angle = 0;
                if (handedness == Handedness.Right)
                {
                    Vector3 knucklesVector = Vector3.Normalize(ringMCPPosition - indexMCPPosition);
                    angle = Vector3.Angle(knucklesVector, orientation * Vector3.right);
                    angle *= -1;
                }
                else
                {
                    Vector3 knucklesVector = Vector3.Normalize(indexMCPPosition - ringMCPPosition);
                    angle = Vector3.Angle(knucklesVector, orientation * Vector3.right);
                }
                Quaternion selfOcclusionSteering = Quaternion.AngleAxis(angle, forward);
                orientation = selfOcclusionSteering * orientation;
            }
            else
            {
                //when palm is facing down we need to rotate some to compensate for an offset:
                float rollCorrection = Mathf.Clamp01(Vector3.Dot(orientation * Vector3.up, Vector3.up));
                float rollCorrectionAmount = -30;
                if (handedness == Handedness.Left)
                {
                    rollCorrectionAmount = 30;
                }
                orientation = Quaternion.AngleAxis(rollCorrectionAmount * rollCorrection, forward) * orientation;
            }


            rotationOfHand = orientation;

        }

        private float Percentage(float value, float minimum, float maximum)
        {
            value -= minimum;
            value = Mathf.Max(0, value);
            return Mathf.Clamp01(value / (maximum - minimum));
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
