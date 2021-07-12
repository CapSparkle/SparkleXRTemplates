using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SparkleXRTemplates
{
    public class XRNodeData : MonoBehaviour
    {
        #region -XRNode data-

        [SerializeField]
        InputSourceType _inputSourceType;
        public InputSourceType inputSourceType
        {
            get
            {
                return _inputSourceType;
            }
            private set
            {
                _inputSourceType = value;
            }
        }

        [SerializeField]
        InputSourceVariant _inputSourceVariant;
        public InputSourceVariant inputSourceVariant
        {
            get
            {
                return _inputSourceVariant;
            }
            private set
            {
                _inputSourceVariant = value;
            }
        }

        #endregion -XRNode data-
    }
}

