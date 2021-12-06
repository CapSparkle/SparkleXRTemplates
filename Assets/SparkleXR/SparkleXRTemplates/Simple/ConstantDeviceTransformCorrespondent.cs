using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SparkleXRTemplates
{
    public class ConstantDeviceTransformCorrespondent : MonoBehaviour
    {
        [SerializeField]
        SimpleHandInputProvider inputProvider;

		[SerializeField]
		XRNodeType deviceTypeToCorrespond = XRNodeType.Hand;

		private void Update()
		{
			transform.position = inputProvider.handCenterPosition;
			transform.rotation = inputProvider.handOrientation;
			print("x = " + inputProvider.handOrientation.x.ToString() +
				"; y = " + inputProvider.handOrientation.y.ToString() +
				"; z = " + inputProvider.handOrientation.z.ToString() +
				"; w = " + inputProvider.handOrientation.w.ToString());
		}
	}

}
