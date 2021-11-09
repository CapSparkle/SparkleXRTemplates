using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SparkleXRTemplates.Examples
{
    public class LavaDrop : MonoBehaviour
    {
        [SerializeField]
        int damageOnTake = 1;

        public void DealDamage(Hand hand)
        {
            try
			{
                hand.GetComponent<GrippingDevice>().TakeDamage(damageOnTake);
			}
            catch (Exception exc)
			{
                Debug.Log(exc.Message);
			}
        }
    }
}
