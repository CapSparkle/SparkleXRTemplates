using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace SparkleXRTemplates
{
    public class MLHandWristTiltProvider : SimpleHandInputProvider
    {
        MLHandTracking.Hand handDevice;
        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            if (handedness == Handedness.Right)
                handDevice = MLHandTracking.Right;
            else if (handedness == Handedness.Left)
                handDevice = MLHandTracking.Left;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}



