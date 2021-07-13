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

        /*protected override bool StartInteraction(GameInteractor interactor)
        {
            if(holdingHand != null)
                return false;

            //if(hoveringHand == null ||
                (
                    (hoveringHand.handPivot.position - transform.position).magnitude > 
                    (interactor.GetComponent<Hand>().handPivot.position - transform.position).magnitude
                )
            )
            {
                hoveringHand = interactor.GetComponent<Hand>();
                return true;
            }
            return false;
        }

        protected override bool StopInteraction(GameInteractor interactor)
        {
            bool returnValue = false;

            if (hoveringHand == interactor.GetComponent<Hand>())
            {
                hoveringHand = null;
                return true;
            }
            
            if (holdingHand == interactor.GetComponent<Hand>())
            {
                holdingHand = null;
                return

            }
            else
            {
                return false;
            }
        }*/

        private Vector3 savedScale;
        private Transform previousParent;

        public void Take(GameInteractor interactor)
        {
            if (holdingHand != null)
                return;

            Hand takingHand = interactor.GetComponent<Hand>();
            if (takingHand == null)
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
            print("drop");
            if (holdingHand != interactor)
                return;

            transform.parent = previousParent;
            transform.localScale = savedScale;
            holdingHand = null;
        }
    }
}