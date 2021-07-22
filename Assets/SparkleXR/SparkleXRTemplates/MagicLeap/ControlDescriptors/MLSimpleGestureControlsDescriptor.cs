using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using SparkleXRTemplates;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.XR.InteractionSubsystems;

using System.Linq;
using System.Diagnostics;

#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;

namespace SparkleXRLib.MagicLeap
{
    [Flags]
	public enum MLGestureMask 
    {
        None = 0,
        Finger = 1,
        Fist = 2,
        Pinch = 4,
        Thumb = 8,
        LShape = 16,
        OpenHand = 32,
        Ok = 64,
        CShape = 128,
        NoPose = 256,
        NoHand = 512
    }


	public class MLSimpleGestureControlsDescriptor : ControlsDescriptor
    {
        #region -tracking requests-

        static bool isKeyPoseTrackingRequestsInitialized = false;
        static Dictionary<MLHandTracking.HandKeyPose, int> KeyPoseTrackingRequests = new Dictionary<MLHandTracking.HandKeyPose, int>();
        private static void RequestKeyPoseTracking(MLHandTracking.HandKeyPose[] requestToBeEnabled)
        {
            MLHandTracking.KeyPoseManager.EnableKeyPoses(requestToBeEnabled, true);
            foreach (MLHandTracking.HandKeyPose handKeyPose in requestToBeEnabled)
                KeyPoseTrackingRequests[handKeyPose] += 1;
        }

        private static void WithdrawKeyPoseTracking(MLHandTracking.HandKeyPose[] requestToBeDisabled)
        {
            foreach (MLHandTracking.HandKeyPose handKeyPose in requestToBeDisabled)
                KeyPoseTrackingRequests[handKeyPose] -= 1;

            foreach (KeyValuePair<MLHandTracking.HandKeyPose, int> handKeyPose in KeyPoseTrackingRequests)
            {
                if (handKeyPose.Value == 0)
                {
                    MLHandTracking.KeyPoseManager.EnableKeyPoses(new MLHandTracking.HandKeyPose[1] { handKeyPose.Key }, false);
                }
                else if (handKeyPose.Value < 0)
                {
                    throw new Exception("To many MLHandTracking.HandKeyPose tracking withdrawals!");
                }
            }
        }


        #endregion -tracking requests-

        void Start()
        {
            requiredXRNodetypeOfInputProvider = XRNodeType.Hand; 

            if (!isKeyPoseTrackingRequestsInitialized)
            {
                foreach (MLHandTracking.HandKeyPose handKeyPose in Enum.GetValues(typeof(MLHandTracking.HandKeyPose)))
                    KeyPoseTrackingRequests[handKeyPose] = 0;

                isKeyPoseTrackingRequestsInitialized = true;
            }
        }

        //TODO: beautify inspector interface

        [OdinSerialize]
        List<List<Action<GameInteractor>>> methods;

        Dictionary<GameInteractor, List<Action>> methodGroups = new Dictionary<GameInteractor, List<Action>>();
        
        [OdinSerialize]
        List<MLGestureMask> mlGestureMasks;
        [OdinSerialize]
        List<GestureState> gestureStates;


        void SetupSubscribingMethodGroups(GameInteractor interactor)
		{
            methodGroups[interactor] = new List<Action>();

            for (int i = 0; i < methods.Count; i++)
			{
                methodGroups[interactor].Add(() =>
                {
                    for (int j = 0; j < methods[i].Count; j++)  //Action<GameInteractor> method in )s
                    {
                        print(j.ToString());
                        methods[i][j](interactor);
                    }                                   
                });
			}
        }

        public override bool StartHandling(GameInteractor interactor)
        {
            if (!CheckInputProvider(interactor))
                return false;

            if(interactor.myXRInputProvider.xrNodeType == XRNodeType.Hand)
			{
                MLHandInputProvider mLHandInputProvider = null;
                mLHandInputProvider = interactor.myXRInputProvider.GetComponent<MLHandInputProvider>();
                
                if (mLHandInputProvider != null)
				{
                    print("interactor has came");
                    SetupSubscribingMethodGroups(interactor);

                    for (int i = 0; i < methodGroups[interactor].Count; i++)
                        mLHandInputProvider.AddGestureListener(methodGroups[interactor][i], mlGestureMasks[i], gestureStates[i]);

                    return true;
                }
            }

            return false;
        }
        public override bool StopHandling(GameInteractor interactor)
        {
            base.StopHandling(interactor);

            if (interactor.myXRInputProvider.xrNodeType == XRNodeType.Hand)
            {
                MLHandInputProvider mLHandInputProvider = null;
                mLHandInputProvider = interactor.myXRInputProvider.GetComponent<MLHandInputProvider>();

                if (mLHandInputProvider != null)
                {
                    if(methodGroups.ContainsKey(interactor))

                    for (int i = 0; i < methods.Count; i++)
                        mLHandInputProvider.RemoveGestureListener(methodGroups[interactor][i]);

                    return true;
                }
            }
            return false;

        }
    }
}

#endif
