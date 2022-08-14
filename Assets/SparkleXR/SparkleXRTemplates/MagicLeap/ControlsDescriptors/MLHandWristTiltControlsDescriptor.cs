using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//TODO: undepend from magic leap
using SparkleXRTemplates.MagicLeap;

namespace SparkleXRTemplates
{
	public class MLHandWristTiltControlsDescriptor : ControlsDescriptor
    {
		[SerializeField]
		List<UnityEventGameInteractorFloat> methodsToControll;

		[SerializeField]
		List<TiltGesture> tiltGestureState;

		//public List<UnityActionSer> ssd;
		Dictionary<GameInteractor, List<SubscriptionBlock<float>>> subscriptions = new Dictionary<GameInteractor, List<SubscriptionBlock<float>>>();

		List<SubscriptionBlock<float>> FormSubscriptionBlocks(GameInteractor interactor)
		{
			List<SubscriptionBlock<float>> subscriptionBlocks = new List<SubscriptionBlock<float>>();

			for (int i = 0; i < methodsToControll.Count; i++)
			{
				subscriptionBlocks.Add(new SubscriptionBlock<float>(methodsToControll[i], interactor));
			}

			return subscriptionBlocks;
		}


		MLHandWristTiltProvider providerActing;
		public override bool StartHandling(GameInteractor interactor)
		{
			if (!CheckInputProvider(interactor))
				return false;

			MLHandWristTiltProvider newProviderActing = interactor.myXRInputProvider.GetComponent<MLHandWristTiltProvider>();

			if (newProviderActing != null)
			{
				providerActing = newProviderActing;

				List<SubscriptionBlock<float>> newSubscriptionBlocks = FormSubscriptionBlocks(interactor);
				subscriptions[interactor] = newSubscriptionBlocks;

				for (int i = 0; i < methodsToControll.Count; i++)
					providerActing.AddGestureListener(subscriptions[interactor][i].Notify, tiltGestureState[i]);

				return true;
			}

			return false;
		}

		public override bool StopHandling(GameInteractor interactor)
		{
			MLHandWristTiltProvider newProviderActing = interactor.myXRInputProvider.GetComponent<MLHandWristTiltProvider>();

			if (newProviderActing != null && newProviderActing == providerActing)
			{
				foreach (SubscriptionBlock<float> subscriptionBlock in subscriptions[interactor])
					providerActing.RemoveGestureListener(subscriptionBlock.Notify);

				subscriptions.Remove(interactor);

				providerActing = null;
				return true;
			}

			providerActing = null;
			return false;
		}

		private void Start()
		{
			requiredXRNodetypeOfInputProvider = XRNodeType.Hand;
		}
	}
}

