using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SparkleXRTemplates
{
    // Base class for selector-side selection analysis logic
    public class SelectionPredicate : MonoBehaviour
    {
        public virtual
        public virtual bool Check(GameInteractable objectToCheck, Object additionalData = null)
        {
            return true;
        }
    }
}