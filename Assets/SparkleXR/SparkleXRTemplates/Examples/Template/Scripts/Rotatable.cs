using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SparkleXRTemplates.Examples
{

    public class Rotatable : GameInteractable
    {
        string myDescription;

        public float currentDegree;

        public void Rotate(float degreeToApply)
		{
            transform.RotateAround(transform.position, Vector3.up, degreeToApply);
		}

        Hand _rotatingHand = null;
        public Hand rotatingHand
        {
            get
            {
                return _rotatingHand;
            }
            private set
            {
                if (value != _rotatingHand)
                {
                    if (_rotatingHand != null)
                    {
                        _rotatingHand.StateOfHand.Remove(this);
                    }

                    _rotatingHand = value;

                    if (_rotatingHand != null)
                    {
                        _rotatingHand.StateOfHand.Add(this, new StateLabel(StateLabelType.carrying, myDescription));
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

            if (rotatingHand != null ||
                takingHand == null ||
                takingHand.busy)
                return;


            previousParent = transform.parent;
            //savedScale = transform.localScale;

            transform.parent = takingHand.transform;

            if (takingHand.handPivot != null)
            {
                transform.parent = takingHand.handPivot;
                transform.position = takingHand.handPivot.position;
                transform.rotation = takingHand.handPivot.rotation;
                //transform.localScale = takingHand.handPivot.localScale;
            }
            else
            {
                transform.parent = takingHand.transform;
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }

            rotatingHand = takingHand;

        }
        public void Drop(GameInteractor interactor)
        {
            Hand dropingHand;

            if ((dropingHand = interactor.GetComponent<Hand>()) == null)
                return;

            print("drop");
            if (rotatingHand != dropingHand)
                return;

            transform.parent = previousParent;
            //transform.localScale = savedScale;
            rotatingHand = null;
        }
    }
}
