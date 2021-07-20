using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;


namespace SparkleXRTemplates.Examples
{
    //[RequireComponent(typeof(TakingInputHandler))]
    public class Takeable : GameInteractable
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
                if(value != _holdingHand)
				{
                    if(_holdingHand != null)
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


            previousParent = transform.parent;
            savedScale = transform.localScale;

            transform.parent = takingHand.transform;

            if (takingHand.handPivot != null)
            {
                transform.parent = holdingHand.handPivot;
                transform.position = holdingHand.handPivot.position;
                transform.rotation = holdingHand.handPivot.rotation;
                transform.localScale = holdingHand.handPivot.localScale;
            }
            else
            {
                transform.parent = holdingHand.handPivot;
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }

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

            transform.parent = previousParent;
            transform.localScale = savedScale;
            holdingHand = null;
        }
    }
}