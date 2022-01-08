using System;
using UnityEngine;
using UnityEngine.Events;
//using Sirenix.Serialization;


namespace SparkleXRTemplates.Examples
{
    [Serializable]
    public class UnityEventHand : UnityEvent<Hand> { }

    //[RequireComponent(typeof(TakingInputHandler))]
    public class Takeable : GameInteractable
    {
        [SerializeField]
        bool childToHoldingHand;

        [SerializeField]
        bool lockOnTake;

        protected string myDescription;

        protected Hand _holdingHand = null;
        public virtual Hand holdingHand 
        {
            get
            {
                return _holdingHand;   
            }
            protected set
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

        private Transform previousParent;

        [SerializeField]
        UnityEventHand OnTakenBy;
        public void Take(GameInteractor interactor)
        {
            print("take occured");
            Hand takingHand = interactor.GetComponent<Hand>();

            if (takingHand == null)
                return;


            if (holdingHand != null ||
                takingHand == null ||
                takingHand.busy)
                return;


            if(childToHoldingHand)
			{
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
                    transform.position = takingHand.transform.position;
                    transform.rotation = Quaternion.identity;
                }
            }

            holdingHand = takingHand;

            //Lock interacting interactor 
            if(lockOnTake)
			{
                interactor.LockGameInteractor(this);
            }

            if (OnTakenBy != null)
                OnTakenBy.Invoke(holdingHand);
        }

        public Action<Hand> RightBeforeDroppedBy;
        public void TryDrop(GameInteractor interactor)
        {
            print("drop occured");
            Hand dropingHand;

            if ((dropingHand = interactor.GetComponent<Hand>()) == null)
                return;


            if (holdingHand != dropingHand)
                return;

            if (lockOnTake)
            {
                holdingHand.LockGameInteractor(null);
            }

            Drop();
        }

        protected void Drop()
		{
            print("drop");
            if (childToHoldingHand)
            {
                transform.parent = previousParent;
            }

            if(RightBeforeDroppedBy != null)
                RightBeforeDroppedBy?.Invoke(holdingHand);
            holdingHand = null;
            

        }
    }
}