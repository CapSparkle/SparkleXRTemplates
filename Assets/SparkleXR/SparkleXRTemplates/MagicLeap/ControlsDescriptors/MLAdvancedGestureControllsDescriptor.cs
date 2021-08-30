using System;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.Serialization;

using UnityEngine.XR.InteractionSubsystems;

#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;

namespace SparkleXRTemplates.MagicLeap
{
    public class MLAdvancedGestureControllsDescriptor : MonoBehaviour
    {
        //TODO: add gesture drawer

        [OdinSerialize]
        List<Action<GameInteractor>> methodsToControll;

        [OdinSerialize]
        List<MLGestureMask> mlGestureMasks;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
#endif