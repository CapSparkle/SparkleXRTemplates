using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SparkleXRTemplates
{ 
    public static class ManagerChooseRules
    {
        public static List<GameInteractable> PrioritizeOneSelector(Selector prioritized, List<GameInteractable> interactables)
        {
            return prioritized.SortInteractables(interactables);
        }
    }
}