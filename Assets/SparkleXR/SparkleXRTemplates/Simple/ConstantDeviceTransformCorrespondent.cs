using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using SparkleXRTemplates.MagicLeap;
using MagicLeapTools;

namespace SparkleXRTemplates
{
    public class ConstantDeviceTransformCorrespondent : MonoBehaviour
    {
        [SerializeField]
        MLHandInputProvider inputProvider;

		[SerializeField]
		HandInput handInput;

		[SerializeField]
		XRNodeType deviceTypeToCorrespond = XRNodeType.Hand;

		private void Update()
		{
			transform.position = inputProvider.handCenterPosition;

			try
			{
				transform.rotation = HandInput.Left.Skeleton.Rotation;
				print(HandInput.Right.Skeleton.Rotation.ToString());
			}
			catch(Exception exc)
			{
				print(exc.Message);
			}
	

			//print("rad = " + MLHandTracking.Left.Wrist.Radial.Position);
			
			/*Quaternion.identity * inputProvider.handOrientation;
		print("x = " + inputProvider.handOrientation.x.ToString() +
			"; y = " + inputProvider.handOrientation.y.ToString() +
			"; z = " + inputProvider.handOrientation.z.ToString() +
			"; w = " + inputProvider.handOrientation.w.ToString()); */
		}
	}

}
