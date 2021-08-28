using System;
using System.Collections.Generic;

using SparkleXRTemplates;

using UnityEngine.XR.InteractionSubsystems;

using Sirenix.Serialization;

#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;

namespace SparkleXRTemplates.MagicLeap
{

    public class MLRotationGestureControlsDescriptor : ControlsDescriptor
    {
        void Start()
        {


        }

        //TODO: beautify inspector interface

        // method groups subscribed on the certain same gestures in certain state
        [OdinSerialize]
        List<List<Action<GameInteractor>>> methodsToControll;
        [OdinSerialize]
        List<MLGestureMask> mlGestureMasks;



        // Update is called once per frame
        void Update()
        {

        }
    }
}
#endif