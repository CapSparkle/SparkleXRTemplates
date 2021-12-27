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
        [SerializeField]
        GameObject eyeGazeCastDirector;

        PositionSmoothier gazeSmooth = new PositionSmoothier();

        void Start()
        {
            MLEyes.Start();
        }

        void Update()
        {
            transform.position = gazeSmooth.PerformSmoothing(MLEyes.FixationPoint);
            eyeGazeCastDirector.transform.position = gazeSmooth.smoothedPosition + (gazeSmooth.smoothedPosition - Camera.main.transform.position).normalized;
        }
    }
}
#endif