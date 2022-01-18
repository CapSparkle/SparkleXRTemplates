using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SparkleXRTemplates;

#if PLATFORM_LUMIN
namespace SparkleXRTemplates.MagicLeap
{
    public class MLSimpleControllerControlsDescriptor : ControlsDescriptor
    {
		[SerializeField]
		List<UnityEventGameInteractorFloat> triggerMethods;

		[SerializeField]
		List<UnityEventGameInteractorBool> bumperMethods;

		[SerializeField]
		List<UnityEventGameInteractorBool> touchPadTouchMethods;

		[SerializeField]
		List<UnityEventGameInteractorVector2> touchPadPoseMethods;


		Dictionary<GameInteractor, List<SubscriptionBlock<float>>> triggerSubscriptions = new Dictionary<GameInteractor, List<SubscriptionBlock<float>>>();
		Dictionary<GameInteractor, List<SubscriptionBlock<bool>>> bumperSubscriptions = new Dictionary<GameInteractor, List<SubscriptionBlock<bool>>>();
		Dictionary<GameInteractor, List<SubscriptionBlock<bool>>> touchPadTouchSubscriptions = new Dictionary<GameInteractor, List<SubscriptionBlock<bool>>>();
		Dictionary<GameInteractor, List<SubscriptionBlock<Vector2>>> touchPadPoseSubscriptions = new Dictionary<GameInteractor, List<SubscriptionBlock<Vector2>>>();

		List<SubscriptionBlock<T>> FormSubscriptionBlocks<T>(GameInteractor interactor, List<UnityEvent<GameInteractor, T>> methods)
		{
			List<SubscriptionBlock<T>> subscriptionBlocks = new List<SubscriptionBlock<T>>();

			for (int i = 0; i < methods.Count; i++)
			{
				subscriptionBlocks.Add(new SubscriptionBlock<T>(methods[i], interactor));
			}

			return subscriptionBlocks;
		}


		MLControllerInputProvider providerActing;
		public override bool StartHandling(GameInteractor interactor)
		{
			if (!CheckInputProvider(interactor))
				return false;
				

			MLControllerInputProvider newProviderActing = interactor.myXRInputProvider.GetComponent<MLControllerInputProvider>();

			print(newProviderActing.name + " " + newProviderActing.GetType().ToString());

			if (newProviderActing != null)
			{
				providerActing = newProviderActing;

				var triggerMethodsCasted = triggerMethods
					.Select(x => (UnityEvent<GameInteractor, float>)x)
					.ToList();
				var bumperMethodsCasted = bumperMethods
					.Select(x => (UnityEvent<GameInteractor, bool>)x)
					.ToList();
				var touchPadTouchMethodsCasted = touchPadTouchMethods
					.Select(x => (UnityEvent<GameInteractor, bool>)x)
					.ToList();
				var touchPadPoseMethodsCasted = touchPadPoseMethods
					.Select(x => (UnityEvent<GameInteractor, Vector2>)x)
					.ToList();

				List<SubscriptionBlock<float>> newTriggerSubscriptionBlocks = FormSubscriptionBlocks(interactor, triggerMethodsCasted);
				List<SubscriptionBlock<bool>> newBumperSubscriptionBlocks = FormSubscriptionBlocks(interactor, bumperMethodsCasted);
				List<SubscriptionBlock<bool>> newtouchPadTouchSubscriptionBlocks = FormSubscriptionBlocks(interactor, touchPadTouchMethodsCasted);
				List<SubscriptionBlock<Vector2>> newtouchPadPoseSubscriptionBlocks = FormSubscriptionBlocks(interactor, touchPadPoseMethodsCasted);

				triggerSubscriptions[interactor] = newTriggerSubscriptionBlocks;
				bumperSubscriptions[interactor] = newBumperSubscriptionBlocks;
				touchPadTouchSubscriptions[interactor] = newtouchPadTouchSubscriptionBlocks;
				touchPadPoseSubscriptions[interactor] = newtouchPadPoseSubscriptionBlocks;


				print(triggerSubscriptions[interactor].Count);

				for (int i = 0; i < newTriggerSubscriptionBlocks.Count; i++)
					providerActing.triggerSubscribers.Add(triggerSubscriptions[interactor][i].Notify);

				for (int i = 0; i < newBumperSubscriptionBlocks.Count; i++)
					providerActing.bumperSubscribers.Add(bumperSubscriptions[interactor][i].Notify);
				for (int i = 0; i < newtouchPadTouchSubscriptionBlocks.Count; i++)
					providerActing.touchSubscribers.Add(touchPadTouchSubscriptions[interactor][i].Notify);
				for (int i = 0; i < newtouchPadPoseSubscriptionBlocks.Count; i++)
					providerActing.touchPadPoseSubscribers.Add(touchPadPoseSubscriptions[interactor][i].Notify);

				return true;
			}

			return false;
		}

		public override bool StopHandling(GameInteractor interactor)
		{
			MLControllerInputProvider newProviderActing = interactor.myXRInputProvider.GetComponent<MLControllerInputProvider>();

			if (newProviderActing != null && newProviderActing == providerActing)
			{
				foreach (SubscriptionBlock<float> subscriptionBlock in triggerSubscriptions[interactor])
					providerActing.triggerSubscribers.Remove(subscriptionBlock.Notify);
				foreach (SubscriptionBlock<bool> subscriptionBlock in bumperSubscriptions[interactor])
					providerActing.bumperSubscribers.Remove(subscriptionBlock.Notify);
				foreach (SubscriptionBlock<bool> subscriptionBlock in touchPadTouchSubscriptions[interactor])
					providerActing.touchSubscribers.Remove(subscriptionBlock.Notify);
				foreach (SubscriptionBlock<Vector2> subscriptionBlock in touchPadPoseSubscriptions[interactor])
					providerActing.touchPadPoseSubscribers.Remove(subscriptionBlock.Notify);

				triggerSubscriptions.Remove(interactor);
				bumperSubscriptions.Remove(interactor);
				touchPadTouchSubscriptions.Remove(interactor);
				touchPadPoseSubscriptions.Remove(interactor);

				providerActing = null;
				return true;
			}

			providerActing = null;
			return false;
		}

		private void Start()
		{
			requiredXRNodetypeOfInputProvider = XRNodeType.Controller;
		}
	}
}
#endif