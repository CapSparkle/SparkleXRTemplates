using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SparkleXRTemplates.Examples
{
    //Semantically pullable is highly associated with takeable.
    //By presented logic you cannot pull or push something without takeing it
    //Pullable is an extension of Takeable
    [RequireComponent(typeof(Rigidbody))]
    public class Pullable : Takeable
    {
        GrippingDevice currentlyGrippingDevice = null;
        public override Hand holdingHand
        {
			get
			{
                return base.holdingHand;
			}
            protected set
			{
                if (value != _holdingHand)
                {
                    if (_holdingHand != null)
                    {
                        _holdingHand.StateOfHand.Remove(this);
                    }

                    _holdingHand = value;


                    if (_holdingHand != null)
                    {
                        _holdingHand.StateOfHand.Add(this, new StateLabel(StateLabelType.carrying, myDescription));
                    }


                    GrippingDevice newGrippingDevice = value.GetComponent<GrippingDevice>();

                    if (currentlyGrippingDevice != newGrippingDevice)
                    {
                        if (currentlyGrippingDevice != null)
                        {
                            currentlyGrippingDevice.OnDamageTaken -= Drop;
                        }

                        currentlyGrippingDevice = newGrippingDevice;

                        if (currentlyGrippingDevice != null)
                        {
                            currentlyGrippingDevice.OnDamageTaken += Drop;
                        }
                    }
                }

                //GrippingDevice grippingDevice = holdingHand.GetComponent<GrippingDevice>();

                if (currentlyGrippingDevice != null)
                    currentlyGrippingDevice.OnDamageTaken += Drop;

                targetVelocity = Vector3.zero;
            }
        }


        Vector3 targetVelocity = Vector3.zero;
        float velocityModifier = 1f;

        public void PullOrPush(GameInteractor interactor, float pullPower)
        {
            GrippingDevice grippingDevice = interactor.GetComponent<GrippingDevice>();
            if (grippingDevice == null)
                return;


            if (holdingHand != grippingDevice.GetComponent<Hand>())
                return;

            targetVelocity = (transform.position - holdingHand.handPivot.position).normalized * velocityModifier * pullPower;
        }


        float changingSpeed = 1f;
        float limitThreshold = 0.1f;
        private void Move()
		{
            Vector3 addingVelocityVector = (targetVelocity - rigidbody.velocity) * Time.deltaTime * changingSpeed;

            if ((targetVelocity - rigidbody.velocity).magnitude < limitThreshold)
                addingVelocityVector = targetVelocity - rigidbody.velocity;

            rigidbody.velocity += addingVelocityVector;

            targetVelocity = Vector3.zero;

        }

        Rigidbody rigidbody;
		private void Start()
		{
            rigidbody = GetComponent<Rigidbody>();
        }

		void Update()
        {
            Move();
        }
    }
}
