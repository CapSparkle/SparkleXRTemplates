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
				
			print(MLHandTracking.Left.TryGet)
				/*Quaternion.identity * inputProvider.handOrientation;
			print("x = " + inputProvider.handOrientation.x.ToString() +
				"; y = " + inputProvider.handOrientation.y.ToString() +
				"; z = " + inputProvider.handOrientation.z.ToString() +
				"; w = " + inputProvider.handOrientation.w.ToString()); */
		}
	}

}
