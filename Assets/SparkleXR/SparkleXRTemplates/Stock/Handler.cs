using System;
using System.Collections;
using System.Collections.Generic;
//using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;


namespace SparkleXRTemplates
{
 /*   public enum InputSourceType
    {
        HMD,
        Hand,
        Controller,
        None
    }
    public enum InputSourceVariant
    {
        Left,
        Right,
        Any,
        None
    }

    public abstract class Handler : SerializedMonoBehaviour
    {
        [SerializeField] protected GameInteractable gameInteractable;

        public delegate void InputEvent (List<GameInteractor> interactor);
        
        protected Dictionary<XRNodeData, HandlersInteractionData> Interactions = new Dictionary<XRNodeData, HandlersInteractionData>();

        public virtual void Proceed(XRNodeData interactingHandler)
        {
            HandlersInteractionData handligForInteraction = new HandlersInteractionData();
            handligForInteraction.inputSourceVariant = interactingHandler.inputSourceVariant;
            handligForInteraction.inputSourceType = interactingHandler.inputSourceType;
            handligForInteraction.interactionCoroutine = Handling(handligForInteraction);

            print(interactingHandler);
            if (needPairing)
            {
                handligForInteraction.Pair(interactingHandler);
            }

            Interactions[interactingHandler] = handligForInteraction;
            StartCoroutine(Interactions[interactingHandler].interactionCoroutine);
        }


        public virtual void Stop(XRNodeData interactingHandler)
        {
            Interactions[interactingHandler].Ending();
            Interactions.Remove(interactingHandler);
        }

        protected virtual IEnumerator Handling(HandlersInteractionData handlingData)
        {
            yield return null;
        }


        //We are using pairing
        //if handler need to change behaviour in dependency of 
        //another handler InputSourceType
        #region -pairing-

        [SerializeField]
        private bool needPairing = true;

        #endregion -pairing-
    }

    public class HandlersInteractionData
    {
        public HandlersInteractionData(InputSourceType inputSourceType = InputSourceType.None, 
            InputSourceVariant inputSourceVariant = InputSourceVariant.None, 
            IEnumerator interactionCoroutine = null, 
            XRNodeData pairedHandler = null)
        {
            this.inputSourceType = inputSourceType;
            this.inputSourceVariant = inputSourceVariant;
            this.interactionCoroutine = interactionCoroutine;
            this.pairedHandler = pairedHandler;
            bool ending = false;
        }

        public InputSourceType inputSourceType;
        public InputSourceVariant inputSourceVariant;

        public IEnumerator interactionCoroutine;

        public XRNodeData pairedHandler;
        public bool ending;

        public void Ending()
        {
            ending = true;
        }


        public void Pair(XRNodeData pairingHandler)
        {
            pairedHandler = pairingHandler;
        }

        public void ChangeInputSourceType()
        {
            inputSourceType = pairedHandler.inputSourceType;
        }
        public void ChangeInputSourceVariant()
        {
            inputSourceVariant = pairedHandler.inputSourceVariant;
        }
    }*/
}