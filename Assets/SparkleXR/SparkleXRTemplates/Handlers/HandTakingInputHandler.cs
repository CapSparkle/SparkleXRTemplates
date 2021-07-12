using System;
using System.Collections;
using System.Collections.Generic;
using SparkleXRTemplates.Examples;
using UnityEngine;
using UnityEngine.Events;

#if PLATFORM_LUMIN
/*
using UnityEngine.XR.MagicLeap;

namespace SparkleXRLib
{
    
    public class HandTakingInputHandler : Handler
    {
        
        [SerializeField]
        int controllerIndex;
        MLInput.Controller controller;

        private void Start()
        {
            MLInput.Start();
            controller = MLInput.GetController(controllerIndex);
        }
        [SerializeField]
        MLInput.Controller.Button alphaButton, bravoButton, charlieButton;

        public UnityEvent AlphaButtonPress, BravoButtonPress, CharlieButtonPress;
        public InputEvent _AlphaButtonPress, _BravoButtonPress, _CharlieButtonPress;

        private List<GameInteractor> InteractingGameInteractors;

        public void HandlePress(byte controllerId, MLInput.Controller.Button pressedButton)
        {
            if(controllerId == controller.Id && pressedButton == alphaButton)
            {
                AlphaButtonPress.Invoke();
                _AlphaButtonPress(InteractingGameInteractors);
            }
            if (controllerId == controller.Id && pressedButton == bravoButton)
            {
                BravoButtonPress.Invoke();
                _BravoButtonPress(InteractingGameInteractors);
            }
            if (controllerId == controller.Id && pressedButton == charlieButton)
            {
                CharlieButtonPress.Invoke();
                _CharlieButtonPress(InteractingGameInteractors);
            }
        }


        private bool handlingStarted = false;
        protected override IEnumerator Handling(HandlersInteractionData handlingData)
        {
            if(handlingData.inputSourceType == InputSourceType.Hand)
                if (!handlingStarted)
                {
                    controller.OnButtonDown += HandlePress;
                    handlingStarted = true;
                }
            else
                yield break;

            InteractingGameInteractors.Add(gameInteractable.UIAndGameLogicPairs[handlingData.pairedHandler]);

            while (!handlingData.ending)
            {
                yield return 0;
            }
            controller.OnButtonDown -= HandlePress;
            handlingStarted = false;
        }
       
}
 }*/

#endif