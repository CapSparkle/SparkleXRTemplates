using System.Collections;
using System.Collections.Generic;
using UnityEngine;




#if PLATFORM_LUMIN
using MagicLeap;
using UnityEngine.XR.MagicLeap;

namespace SparkleXRTemplates.MagicLeap

{
    public class EyeFixationPointFollower : MonoBehaviour
    {
        void Start()
        {
            MLEyes.Start();
        }

        void Update()
        {
            transform.position = MLEyes.FixationPoint;
        }
    }
}
#endif