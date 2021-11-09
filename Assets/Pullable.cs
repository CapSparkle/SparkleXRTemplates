using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SparkleXRTemplates.Examples
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pullable : GameInteractable
    {
        string myDescription;

        Hand _holdingHand = null;
        public Hand holdingHand
        {
            get
            {
                return _holdingHand;
            }
            private set
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
                }
            }
        }


        private Vector3 savedScale;
        private Transform previousParent;

        public void Take(GameInteractor interactor)
        {
            Hand takingHand;

            if ((takingHand = interactor.GetComponent<Hand>()) == null)
                return;

            if (holdingHand != null ||
                takingHand == null ||
                takingHand.busy)
                return;


            holdingHand = takingHand;

        }
        public void Drop(GameInteractor interactor)
        {
            Hand dropingHand;

            if ((dropingHand = interactor.GetComponent<Hand>()) == null)
                return;

            print("drop");
            if (holdingHand != dropingHand)
                return;


            holdingHand = null;
        }


        Vector3 targetVelocity = Vector3.zero;
        float velocityModifier = 1f;

        public void Pull(GameInteractor interactor, float pullPower)
        {
            Hand pullingHand;
            if ((pullingHand = interactor.GetComponent<Hand>()) == null)
                return;

            if (holdingHand != pullingHand)
                return;


            targetVelocity = (pullingHand.handPivot.position - transform.position).normalized * velocityModifier * pullPower;

            //this.GetComponent<Rigidbody>().AddForce(new Vector3(1f, 1f, 1f), ForceMode.)

        }

        public void Push(GameInteractor interactor, float pullPower)
        {
            Hand pushingHand;
            if ((pushingHand = interactor.GetComponent<Hand>()) == null)
                return;

            if (holdingHand != pushingHand)
                return;

            targetVelocity = (transform.position - pushingHand.handPivot.position).normalized * velocityModifier * pullPower;
        }


        float changingSpeed = 2f;

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
