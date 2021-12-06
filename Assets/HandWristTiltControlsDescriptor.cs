using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SparkleXRTemplates
{
    public class HandWristTiltControlsDescriptor : ControlsDescriptor
    {
		[SerializeField]
		List<Action<float>> methodsToControll;
		[SerializeField]
		List<HandWristTilt> tiltGestureState;


		MLHandWristTiltProvider providerActing;
		public override bool StartHandling(GameInteractor interactor)
		{
			if (!CheckInputProvider(interactor))
				return false;

			MLHandWristTiltProvider newProviderActing = interactor.myXRInputProvider.GetComponent<MLHandWristTiltProvider>();

			if (newProviderActing != null)
			{
				providerActing = newProviderActing;

				for (int i = 0; i < methodsToControll.Count; i++)
					providerActing.AddGestureListener(methodsToControll[i], tiltGestureState[i]);

				return true;
			}

			return false;
		}

		public override bool StopHandling(GameInteractor interactor)
		{
			MLHandWristTiltProvider newProviderActing = interactor.myXRInputProvider.GetComponent<MLHandWristTiltProvider>();

			if (newProviderActing != null && newProviderActing == providerActing)
			{
				foreach (Action<float> method in methodsToControll)
					providerActing.RemoveGestureListener(method);


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

