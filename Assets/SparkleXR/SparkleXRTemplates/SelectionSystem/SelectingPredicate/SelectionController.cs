using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SparkleXRTemplates
{
    // Base class for selector-side selection analysis logic
    public class SelectionController : MonoBehaviour
    {
        public virtual bool CheckConditions(GameInteractable objectToCheck, Object additionalData = null)
        {
            return true;
        }
    }
}