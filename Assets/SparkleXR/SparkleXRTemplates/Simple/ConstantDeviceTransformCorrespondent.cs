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
		}
	}

}
