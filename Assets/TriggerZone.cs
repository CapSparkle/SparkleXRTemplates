using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
	[SerializeField]
	UnityEvent someMethod;

	[SerializeField]
	Collider targetCollider;

	private void OnTriggerEnter(Collider other)
	{
		if (other == targetCollider)
			someMethod.Invoke();
	}
}
