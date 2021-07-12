using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;


namespace SparkleXRTemplates.Examples
{
    //[RequireComponent(typeof(TakingInputHandler))]
    public class Takeable : GameInteractable
    {
        
        public Hand holdingHand { get; private set; }
        public Hand hoveringHand { get; private set; }

        //public List<Hand> hoveringHands;
        void Awake()
        {
            holdingHand = null;
        }



        protected override bool StartInteraction(GameInteractor interactor)
        {
            if(holdingHand != null)
                return false;

            if(hoveringHand == null)
            {
                hoveringHand = interactor.GetComponent<Hand>();
                return true;
            }
            return false;
        }

        protected override bool StopInteraction(GameInteractor interactor)
        {
            if (hoveringHand == interactor.GetComponent<Hand>())
            {
                hoveringHand = null;
                return true;
            }
            else
            {
                return false;
            }
        }



        [OdinSerialize]
        Transform onAttachTransform;

        private Vector3 savedScale;
        private Transform previousParent;

        public void Take(GameInteractor interactor)
        {
            print("Take");
            if(hoveringHand != interactor)
                return;
            
            if(holdingHand == interactor)
                return;

            holdingHand = hoveringHand;;
            previousParent = transform.parent;
            savedScale = transform.localScale;

            if (onAttachTransform == null)
            {
                transform.parent = holdingHand.handPivot;
                transform.position = holdingHand.handPivot.position;
                transform.rotation = holdingHand.handPivot.rotation;
                transform.localScale = holdingHand.handPivot.localScale;
            }
            else
            {
                onAttachTransform.parent = holdingHand.handPivot;
                onAttachTransform.position = onAttachTransform.position;
                onAttachTransform.rotation = onAttachTransform.rotation;
                onAttachTransform.localScale = onAttachTransform.localScale;
            }
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