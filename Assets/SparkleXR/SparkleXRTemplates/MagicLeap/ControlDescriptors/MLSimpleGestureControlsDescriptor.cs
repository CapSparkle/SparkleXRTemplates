using System;
using System.Collections;
using System.Collections.Generic;
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
        //TODO: beautify inspector interface

        [OdinSerialize]
        List<List<Action<GameInteractor>>> methods;
        [OdinSerialize]
        List<GestureState> gestureStates;
        [OdinSerialize]
        List<MLGestureMask> mlGestureMasks;
        
        #region -tracking requests-

        static bool isKeyPoseTrackingRequestsInitialized = false;
        static Dictionary<MLHandTracking.HandKeyPose, int> KeyPoseTrackingRequests = new Dictionary<MLHandTracking.HandKeyPose, int>();
        private static void RequestKeyPoseTracking(MLHandTracking.HandKeyPose[] requestToBeEnabled)
        {
            MLHandTracking.KeyPoseManager.EnableKeyPoses(requestToBeEnabled, true);
            foreach(MLHandTracking.HandKeyPose handKeyPose in requestToBeEnabled)
                KeyPoseTrackingRequests[handKeyPose] += 1;
        }

        private static void WithdrawKeyPoseTracking(MLHandTracking.HandKeyPose[] requestToBeDisabled)
        {
            foreach (MLHandTracking.HandKeyPose handKeyPose in requestToBeDisabled)
                KeyPoseTrackingRequests[handKeyPose] -= 1;

            foreach(KeyValuePair<MLHandTracking.HandKeyPose, int> handKeyPose in KeyPoseTrackingRequests)
            {
                if (handKeyPose.Value == 0)
                {
                    MLHandTracking.KeyPoseManager.EnableKeyPoses(new MLHandTracking.HandKeyPose[1]{ handKeyPose.Key}, false);
                }
                else if(handKeyPose.Value < 0)
                {
                    throw new Exception("To many MLHandTracking.HandKeyPose tracking withdrawals!");
                }
            }

        }

        #endregion -tracking requests-


        //[OdinSerialize]
        //CallMoment alphaGestureCallMoment = CallMoment.onBegin,
                   //bravoGestureCallMoment = CallMoment.onBegin;

        [OdinSerialize]
        MLHandTracking.HandKeyPose AlphaGesture, BravoGesture;

        [OdinSerialize]
        public Action<GameInteractor> AlphaGestureOccured, BravoGestureOccured;


        //public delegate void OnKeyPose(InputSourceVariant inpSV, MLHandTracking.HandKeyPose handKP, MLHandTracking.HandType handT);


        void Start()
        {
            MLHandTracking.Start();

            if(!isKeyPoseTrackingRequestsInitialized)
            {
                foreach (MLHandTracking.HandKeyPose handKeyPose in Enum.GetValues(typeof(MLHandTracking.HandKeyPose)))
                    KeyPoseTrackingRequests[handKeyPose] = 0;

                isKeyPoseTrackingRequestsInitialized = true;
            }
        }


        //TODO: Speciall class for handling certain gesture in certain time moment!!!

        /*Dictionary<InputSourceVariant, MLHandTracking.KeyposeManager.OnHandKeyPoseEndDelegate> xrNodeEndHandlingMethods =
            new Dictionary<InputSourceVariant, MLHandTracking.KeyposeManager.OnHandKeyPoseEndDelegate>();

        Dictionary<InputSourceVariant, MLHandTracking.KeyposeManager.OnHandKeyPoseBeginDelegate> xrNodeBeginHandlingMethods =
            new Dictionary<InputSourceVariant, MLHandTracking.KeyposeManager.OnHandKeyPoseBeginDelegate>();*/


        public override bool StartHandling(GameInteractor interactor)
        {
            return base.StartHandling(interactor);
           /* XRNodeData xrNodeData = interactor.myXRNode;

            if (xrNodeData.inputSourceType != requiredInputSourceType)
                return;

            if (alphaGestureCallMoment == CallMoment.onEnd)
            {
                MLHandTracking.KeyposeManager.OnHandKeyPoseEndDelegate method = (keyPose, handType) =>
                {
                    if (keyPose == AlphaGesture)
                        if (handType == MLHandTracking.HandType.Left && xrNodeData.inputSourceVariant == InputSourceVariant.Left ||
                            handType == MLHandTracking.HandType.Right && xrNodeData.inputSourceVariant == InputSourceVariant.Right)
                            {
                                AlphaGestureOccured(interactor);
                            }
                };

                MLHandTracking.KeyPoseManager.OnKeyPoseEnd += method;
                xrNodeEndHandlingMethods[xrNodeData.inputSourceVariant] = method;
            }
            else
            {
                MLHandTracking.KeyposeManager.OnHandKeyPoseBeginDelegate method = (keyPose, handType) =>
                {
                    if (keyPose == AlphaGesture)
                        if (handType == MLHandTracking.HandType.Left && xrNodeData.inputSourceVariant == InputSourceVariant.Left ||
                            handType == MLHandTracking.HandType.Right && xrNodeData.inputSourceVariant == InputSourceVariant.Right)
                            {
                                AlphaGestureOccured(interactor);
                            }
                };
                MLHandTracking.KeyPoseManager.OnKeyPoseBegin += method;
                xrNodeBeginHandlingMethods[xrNodeData.inputSourceVariant] = method;
            }


            if (bravoGestureCallMoment == CallMoment.onEnd)
            {
                MLHandTracking.KeyposeManager.OnHandKeyPoseEndDelegate method = (keyPose, handType) =>
                {
                    if (keyPose == BravoGesture)
                        if (handType == MLHandTracking.HandType.Left && xrNodeData.inputSourceVariant == InputSourceVariant.Left ||
                            handType == MLHandTracking.HandType.Right && xrNodeData.inputSourceVariant == InputSourceVariant.Right)
                            {
                                BravoGestureOccured(interactor);
                            }
                };

                MLHandTracking.KeyPoseManager.OnKeyPoseEnd += method;
                xrNodeEndHandlingMethods[xrNodeData.inputSourceVariant] = method;
            }
            else
            {
                MLHandTracking.KeyposeManager.OnHandKeyPoseBeginDelegate method = (keyPose, handType) =>
                {
                    if (keyPose == BravoGesture)
                        if (handType == MLHandTracking.HandType.Left && xrNodeData.inputSourceVariant == InputSourceVariant.Left ||
                            handType == MLHandTracking.HandType.Right && xrNodeData.inputSourceVariant == InputSourceVariant.Right)
                            {
                                BravoGestureOccured(interactor);
                            }
                };
                MLHandTracking.KeyPoseManager.OnKeyPoseBegin += method;
                xrNodeBeginHandlingMethods[xrNodeData.inputSourceVariant] = method;
            }*/
        }

        public override bool StopHandling(GameInteractor interactor)
        {
            return base.StopHandling(interactor);
            /*XRNodeData xrNodeData = interactor.myXRNode;
            if (xrNodeEndHandlingMethods.ContainsKey(xrNodeData.inputSourceVariant))
                MLHandTracking.KeyPoseManager.OnKeyPoseEnd -= xrNodeEndHandlingMethods[xrNodeData.inputSourceVariant];

            if (xrNodeBeginHandlingMethods.ContainsKey(xrNodeData.inputSourceVariant))
                MLHandTracking.KeyPoseManager.OnKeyPoseBegin -= xrNodeBeginHandlingMethods[xrNodeData.inputSourceVariant];*/
        }

        private void OnDestroy()
        {
            MLHandTracking.Stop();
        }
    }
}

#endif
