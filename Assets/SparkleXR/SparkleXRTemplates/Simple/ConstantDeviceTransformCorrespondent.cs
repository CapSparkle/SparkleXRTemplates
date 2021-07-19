using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SparkleXRTemplates
{
    public class ConstantDeviceTransformCorrespondent : MonoBehaviour
    {
        [SerializeField]
        XRInputProvider inputProvider;

		[SerializeField]
		XRNodeFeatureGroup deviceTypeToCorrespond = XRNodeFeatureGroup.Hand;

		private void Start()
		{

			if (inputProvider.xrNodeFeatureGroup != deviceTypeToCorrespond ||
				inputProvider.GetComponent<SimpleHandInputProvider>() == null)
			{
				Destroy(this);
			}
		}

		private void Update()
		{
			transform.position = inputProvider.GetComponent<SimpleHandInputProvider>().handCenterPosition;
			transform.rotation = inputProvider.GetComponent<SimpleHandInputProvider>().handOrientation;
		}
	}

}
