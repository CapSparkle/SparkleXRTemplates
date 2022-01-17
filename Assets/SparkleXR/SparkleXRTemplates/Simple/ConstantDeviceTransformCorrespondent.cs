using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using SparkleXRTemplates.MagicLeap;

namespace SparkleXRTemplates
{
    public class ConstantDeviceTransformCorrespondent : MonoBehaviour
    {
        [SerializeField]
        MLHandInputProvider inputProvider;

		[SerializeField]
		XRNodeType deviceTypeToCorrespond = XRNodeType.Hand;

		private void Update()
		{
			transform.position = inputProvider.handCenterPosition;
			transform.rotation = inputProvider.handOrientation;
			//transform.Rotate(new Vector3(55f, -5f, 0f), Space.Self);

			//print("rad = " + MLHandTracking.Left.Wrist.Radial.Position);
		}
	}

}
