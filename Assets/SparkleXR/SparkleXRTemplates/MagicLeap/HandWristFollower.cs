using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
namespace SparkleXRTemplates.MagicLeap
{


    public class HandWristFollower : MonoBehaviour
    {
        [SerializeField]
        public MLHandTracking.HandType handType;
        MLHandTracking.Hand targetHand;

        void Start()
        {
            MLHandTracking.Start();
            if (handType == MLHandTracking.Right.Type)
                targetHand = MLHandTracking.Right;
            else if (handType == MLHandTracking.Left.Type)
                targetHand = MLHandTracking.Left;
        }

        void Update()
        {
            transform.position = targetHand.Wrist.Center.Position;
        }
    }
}
#endif