using System;
using System.Collections;
using System.Collections.Generic;
//using Sirenix.Serialization;
using SparkleXRTemplates.Examples;
using UnityEngine;
using UnityEngine.Events;


#if PLATFORM_LUMIN

using UnityEngine.XR.MagicLeap;
/*namespace SparkleXRLib
{
    
    public class ControllerTakingInputHandler : Handler
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

        [OdinSerialize]
        public InputEvent _AlphaButtonPress, _BravoButtonPress, _CharlieButtonPress;

        private List<GameInteractor> InteractingGameInteractors = new List<GameInteractor>();

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
            if (handlingData.inputSourceType == InputSourceType.Controller)
            {
                if (!handlingStarted)
                {
                    controller.OnButtonDown += HandlePress;
                    handlingStarted = true;
                }
                //print(handlingData.pairedHandler);
                print(handlingData.pairedHandler);
                InteractingGameInteractors.Add(gameInteractable.UIAndGameLogicPairs[handlingData.pairedHandler]);
            }
            else
                yield break;

            

            while (!handlingData.ending)
            {
                yield return 0;
            }
            InteractingGameInteractors.Remove(gameInteractable.UIAndGameLogicPairs[handlingData.pairedHandler]);
            gameInteractable.UIAndGameLogicPairs.Remove(handlingData.pairedHandler);
            // ^ bad case of coupling ^

            if (InteractingGameInteractors.Count == 0)
            {
                controller.OnButtonDown -= HandlePress;
                handlingStarted = false;
            }  
        }
    }
}
*/
#endif