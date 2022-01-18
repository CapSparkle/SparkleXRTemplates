using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.InteractionSubsystems;
using UnityEngine.XR.MagicLeap;
using UnityEngine.XR;


namespace SparkleXRTemplates.MagicLeap
{
    [RequireComponent(typeof(MLControllerConnectionHandlerBehavior))]
    public class MLControllerInputProvider : XRInputProvider
    {
        [SerializeField]
        MLControllerConnectionHandlerBehavior controllerConnectionHandler = null;
        //MLInput.Controller controller = null;

        FeatureGroupDataSource controllerData;

        [HideInInspector]
        public List<Action<bool>> bumperSubscribers;

        [HideInInspector]
        public List<Action<bool>> touchSubscribers;

        [HideInInspector]
        public List<Action<Vector2>> touchPadPoseSubscribers;

        [HideInInspector]
        public List<Action<float>> triggerSubscribers;

        protected void Start()
		{
			if (controllerConnectionHandler == null)
				    controllerConnectionHandler = GetComponent<MLControllerConnectionHandlerBehavior>();

            touchSubscribers = new List<Action<bool>>();
            touchSubscribers = new List<Action<bool>>();
            touchPadPoseSubscribers = new List<Action<Vector2>>();
			triggerSubscribers = new List<Action<float>>();

            try
            {
                inputDeviceCharacteristics = (inputDeviceCharacteristics | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand);


                controllerData = new FeatureGroupDataSource(new List<InputFeatureUsage>() 
                {
                    (InputFeatureUsage)CommonUsages.trigger, 
                    (InputFeatureUsage)CommonUsages.gripButton,
                    (InputFeatureUsage)CommonUsages.primary2DAxis,
                    (InputFeatureUsage)CommonUsages.primary2DAxisTouch
                },
                    inputDeviceCharacteristics);

                print("Why am i here?");
                StartCoroutine(controllerData.FindDevice());
            }
            catch (Exception exc)
            {
                print(exc.Message);
            }
        }

        public bool bumperPressed { get; protected set; }
        public float triggerValue { get; protected set; }
        public Vector2 touchPadPose { get; protected set; }
        public bool touchPadTouch { get; protected set; }
        public Vector3 controllerPosition { get; protected set; }
        public Quaternion controllerOrientation { get; protected set; }

        protected void Update()
		{
            controllerPosition = controllerConnectionHandler.ConnectedController.Position;
            controllerOrientation = controllerConnectionHandler.ConnectedController.Orientation;

            if (controllerData.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                if (triggerValue > 0)
                {
                    this.triggerValue = triggerValue;
                    foreach (Action<float> subscriber in triggerSubscribers)
                        subscriber.Invoke(triggerValue);
                }
                else
                    this.triggerValue = 0;

            }
            else if (controllerData.deviceFindState != DeviceFindState.Finding)
                StartCoroutine(controllerData.FindDevice());


            if (controllerData.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool isBumperDown))
            {
                if (this.bumperPressed != isBumperDown)
				{
                    foreach (Action<bool> subscriber in bumperSubscribers)
                        subscriber.Invoke(isBumperDown);

                    this.bumperPressed = isBumperDown;
                }

            }
            else if (controllerData.deviceFindState != DeviceFindState.Finding)
                StartCoroutine(controllerData.FindDevice());


            if (controllerData.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool touch))
            {
                if(this.touchPadTouch != touch)
				{
                    foreach (Action<bool> subscriber in touchSubscribers)
                        subscriber.Invoke(touch);

                    this.touchPadTouch = touch;
                }

            }
            else if(controllerData.deviceFindState != DeviceFindState.Finding)
                StartCoroutine(controllerData.FindDevice());


            if (controllerData.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 touchValue))
            {
                foreach (Action<Vector2> subscriber in touchPadPoseSubscribers)
                    subscriber.Invoke(touchValue);

                this.touchPadPose = touchValue;
            }
            else if (controllerData.deviceFindState != DeviceFindState.Finding)
                StartCoroutine(controllerData.FindDevice());
        }
    }
}


