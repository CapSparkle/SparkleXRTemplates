using SparkleXRTemplates.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SparkleXRTemplates
{
    
    public class HandSelectingPredicate : SelectionController
    {

        public override bool CheckConditions(GameInteractable objectToCheck, Object additionalData = null)
        {
            if (objectToCheck.GetComponentInChildren<Takeable>() != null)
                return true;
            else
                return false;
        }
    }
}

