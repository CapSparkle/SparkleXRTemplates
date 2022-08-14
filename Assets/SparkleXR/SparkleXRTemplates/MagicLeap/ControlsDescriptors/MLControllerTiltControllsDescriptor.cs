
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SparkleXRTemplates.MagicLeap
{
    public class MLControllerTiltControllsDescriptor : ControlsDescriptor
	{
		/// <summary>
		/// Methods to controll by trigger button
		/// </summary>
		[SerializeField]
		List<UnityEventGameInteractorFloat> methodsToControll;

		/*[SerializeField]
		List<UnityEventGameInteractorBool> bumperMethods;

		[SerializeField]
		List<UnityEventGameInteractorBool> touchPadTouchMethods;

		[SerializeField]
		List<UnityEventGameInteractorVector2> touchPadPoseMethods;*/

		[SerializeField]
		List<TiltGesture> tiltGestureState;

		//public List<UnityActionSer> ssd;
		Dictionary<GameInteractor, List<SubscriptionBlock<float>>> triggerSubscriptions = new Dictionary<GameInteractor, List<SubscriptionBlock<float>>>();

		List<SubscriptionBlock<T>> FormSubscriptionBlocks<T>(GameInteractor interactor, List<UnityEvent<GameInteractor, T>> methods)
		{
			List<SubscriptionBlock<T>> subscriptionBlocks = new List<SubscriptionBlock<T>>();

			for (int i = 0; i < methods.Count; i++)
			{
				subscriptionBlocks.Add(new SubscriptionBlock<T>(methods[i], interactor));
			}

			return subscriptionBlocks;
		}


		MLControllerTiltProvider providerActing;
		public override bool StartHandling(GameInteractor interactor)
		{
			if (!CheckInputProvider(interactor))
				return false;

			MLControllerTiltProvider newProviderActing = interactor.myXRInputProvider.GetComponent<MLControllerTiltProvider>();

			if (newProviderActing != null)
			{
				providerActing = newProviderActing;

				List<UnityEvent<GameInteractor, float>> inputList = methodsToControll
					.Select(x => (UnityEvent<GameInteractor, float>)x)
					.ToList();

				List<SubscriptionBlock<float>> newSubscriptionBlocks = FormSubscriptionBlocks(interactor, inputList);

				triggerSubscriptions[interactor] = newSubscriptionBlocks;
				
				for (int i = 0; i < methodsToControll.Count; i++)
					providerActing.AddGestureListener(triggerSubscriptions[interactor][i].Notify, tiltGestureState[i]);

				return true;
			}

			return false;
		}

		public override bool StopHandling(GameInteractor interactor)
		{
			MLControllerTiltProvider newProviderActing = interactor.myXRInputProvider.GetComponent<MLControllerTiltProvider>();

			if (newProviderActing != null && newProviderActing == providerActing)
			{
				foreach (SubscriptionBlock<float> subscriptionBlock in triggerSubscriptions[interactor])
					providerActing.RemoveGestureListener(subscriptionBlock.Notify);

				triggerSubscriptions.Remove(interactor);

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
