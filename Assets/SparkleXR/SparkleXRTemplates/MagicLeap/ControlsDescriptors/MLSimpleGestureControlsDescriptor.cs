using System;
using System.Collections.Generic;

using Sirenix.Serialization;

using UnityEngine.XR.InteractionSubsystems;


#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;

namespace SparkleXRTemplates.MagicLeap
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

    public class SubscriptionBlock
	{
        //On certain determined event from this interactor 
        GameInteractor _interactor;

        //Subscribed the following methods
        List<Action<GameInteractor>> _observingMethods;

		public SubscriptionBlock(List<Action<GameInteractor>> observingMethods, GameInteractor interactor)
		{
			_observingMethods = observingMethods;
            _interactor = interactor;

        }

		public void Notify()
		{
            foreach (Action<GameInteractor> method in _observingMethods)
                method(_interactor);
        }
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

        // method groups subscribed on the certain same gestures in certain state
        [OdinSerialize]
        List<List<Action<GameInteractor>>> methodsToControll;
        [OdinSerialize]
        List<MLGestureMask> mlGestureMasks;
        [OdinSerialize]
        List<GestureState> gestureStates;


        Dictionary<GameInteractor, List<SubscriptionBlock>> subscriptions = new Dictionary<GameInteractor, List<SubscriptionBlock>>();

        List<SubscriptionBlock> FormSubscriptionBlocks(GameInteractor interactor)
        {
            List<SubscriptionBlock> subscriptionBlocks = new List<SubscriptionBlock>();

            for (int i = 0; i < methodsToControll.Count; i++)
            {
                subscriptionBlocks.Add(new SubscriptionBlock(methodsToControll[i], interactor));
            }

            return subscriptionBlocks;
        }

        public override bool StartHandling(GameInteractor interactor)
        {
            if (!CheckInputProvider(interactor))
                return false;

            MLHandInputProvider mLHandInputProvider = interactor.myXRInputProvider.GetComponent<MLHandInputProvider>();
                
            if (mLHandInputProvider != null)
			{
                print("interactor has came");
                
                List<SubscriptionBlock> newSubscriptionBlocks = FormSubscriptionBlocks(interactor);
                subscriptions[interactor] = newSubscriptionBlocks;

                for (int i = 0; i < subscriptions[interactor].Count; i++)
                    mLHandInputProvider.AddGestureListener(subscriptions[interactor][i].Notify, mlGestureMasks[i], gestureStates[i]);

                return true;
            }

            return false;
        }
        public override bool StopHandling(GameInteractor interactor)
        {
            base.StopHandling(interactor);

            if (subscriptions.ContainsKey(interactor))
            {
                MLHandInputProvider mLHandInputProvider = interactor.myXRInputProvider.GetComponent<MLHandInputProvider>();
                
                for (int i = 0; i < subscriptions[interactor].Count; i++)
                    mLHandInputProvider.RemoveGestureListener(subscriptions[interactor][i].Notify);

                subscriptions.Remove(interactor);

                return true;
            }

            return false;
        }
    }
}

#endif
