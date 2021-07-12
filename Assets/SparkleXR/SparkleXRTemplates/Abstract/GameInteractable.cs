using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace SparkleXRTemplates
{
    public abstract class GameInteractable : SerializedMonoBehaviour
    {
        [OdinSerialize]
        List<ControlsDescriptor> myControlls;

        public void Interact(GameInteractor interactor)
        {
            foreach (ControlsDescriptor controls in myControlls)
            {
                controls.StartHandling(interactor);
            }
            StartInteraction(interactor);
        }

        public void UnInteract(GameInteractor interactor)
        {
            print("uninteract");
            foreach (ControlsDescriptor controls in myControlls)
            {
                controls.StopHandling(interactor);
            }

            StopInteraction(interactor);
        }


        protected virtual bool StartInteraction(GameInteractor interactor)
        {
            return false;
        }
        protected virtual bool StopInteraction(GameInteractor interactor)
        {
            return false;
        }
    }
}