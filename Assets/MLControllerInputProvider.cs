using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.InteractionSubsystems;
using UnityEngine.XR.MagicLeap;
using UnityEngine.XR;


namespace SparkleXRTemplates.MagicLeap
{
    public class MLControllerInputProvider : XRInputProvider
    {
        [SerializeField]
        MLControllerConnectionHandlerBehavior controllerConnectionHandler = null;
        MLInput.Controller controller = null;

        FeatureGroupDataSource controllerData;


        List<Action<bool>> bumperSubscribers;

        List<Action> touchSubscribers;
        List<Action<Vector2>> touchPadPoseSubscribers;

        List<Action<float>> triggerSubscribers;

        void Start()
		{
			if (controllerConnectionHandler == null)
				    controllerConnectionHandler = GetComponent<MLControllerConnectionHandlerBehavior>();

            touchSubscribers = new List<Action>();
			touchPadPoseSubscribers = new List<Action<Vector2>>();
			triggerSubscribers = new List<Action<float>>();

            try
            {
                inputDeviceCharacteristics = (InputDeviceCharacteristics)(inputDeviceCharacteristics | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand);


                controllerData = new FeatureGroupDataSource(new List<InputFeatureUsage>() 
                {
                    (InputFeatureUsage)CommonUsages.trigger, 
                    (InputFeatureUsage)CommonUsages.gripButton,
                    (InputFeatureUsage)CommonUsages.primary2DAxis,
                    (InputFeatureUsage)CommonUsages.primary2DAxisTouch
                },
                    inputDeviceCharacteristics);
            }
            catch (Exception exc)
            {
                print(exc.Message);
            }
        }


        bool previousIsBumberDown;
		void Update()
		{
            if (controllerData.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                if (triggerValue > 0)
                    foreach (Action<float> subscriber in triggerSubscribers)
                        subscriber.Invoke(triggerValue);
            }
            else if (controllerData.deviceFindState != DeviceFindState.Finding)
                controllerData.FindDevice();


            if (controllerData.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool isBumperDown))
            {
                if (previousIsBumberDown != isBumperDown)
                    foreach (Action<bool> subscriber in bumperSubscribers)
                        subscriber.Invoke(isBumperDown);

                previousIsBumberDown = isBumperDown;
            }
            else if (controllerData.deviceFindState != DeviceFindState.Finding)
                controllerData.FindDevice();


            if (controllerData.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool touch))
            {
                foreach (Action subscriber in touchSubscribers)
                        subscriber.Invoke();
            }
            else if(controllerData.deviceFindState != DeviceFindState.Finding)
                controllerData.FindDevice();


            if (controllerData.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 touchValue))
            {
                foreach (Action<Vector2> subscriber in touchPadPoseSubscribers)
                    subscriber.Invoke(touchValue);
            }
            else if (controllerData.deviceFindState != DeviceFindState.Finding)
                controllerData.FindDevice();
        }
    }
}


