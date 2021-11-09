using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using SparkleXRTemplates;

public class HandWristTiltControllsDescriptor : ControlsDescriptor
{
    [OdinSerialize]
    List<Action<GameInteractor, float>> onBentUpSubscribers;

    [OdinSerialize]
    List<Action<GameInteractor, float>> onBentDownSubscribers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
