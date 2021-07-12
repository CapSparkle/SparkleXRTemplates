using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

namespace SparkleXRTemplates
{
    public enum XRNodeFeatureGroup
    {
        Controller,
        Hand,
        HMD
    }


    // One XRInputProvider may be logically corresponding to several UnityEngine.XR.InputDevice -s
    public abstract class XRInputProvider : MonoBehaviour
    {
        XRNodeFeatureGroup _xrNodeFeatureGroup;

        public XRNodeFeatureGroup xrNodeFeatureGroup
        {
            get
            {
                return _xrNodeFeatureGroup;
            }
            protected set
            {
                _xrNodeFeatureGroup = value;
            }
        }
    }
}

