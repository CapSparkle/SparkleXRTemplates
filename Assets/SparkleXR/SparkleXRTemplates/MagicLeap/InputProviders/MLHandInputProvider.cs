using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SparkleXRTemplates;
using UnityEngine.XR;
using UnityEngine.XR.MagicLeap;
using UnityEngine.XR.InteractionSubsystems;

enum Handedness
{ 
    None,
    Left = 256,
    Right = 512
}

//MagicLeapKeyPose
//MagicLeapKeyPoseGestureEvent

public class MLHandInputProvider : XRInputProvider
{
    [SerializeField]
    Handedness handedness = Handedness.None;



    private void Start()
    {
        #region -enable key poses-
        //TODO: bring that code block to the separate class
        
        List<MLHandTracking.HandKeyPose> defaultEnabledKeyPoses = new List<MLHandTracking.HandKeyPose> {
            MLHandTracking.HandKeyPose.Fist,
            MLHandTracking.HandKeyPose.C,
            MLHandTracking.HandKeyPose.Ok,
            MLHandTracking.HandKeyPose.OpenHand,
            MLHandTracking.HandKeyPose.Pinch,
            MLHandTracking.HandKeyPose.Finger,
            MLHandTracking.HandKeyPose.L,

            MLHandTracking.HandKeyPose.NoPose,
            MLHandTracking.HandKeyPose.NoHand
        };

        MLHandTracking.KeyPoseManager.EnableKeyPoses(defaultEnabledKeyPoses.ToArray(), true);

        #endregion 

        xrNodeFeatureGroup = XRNodeFeatureGroup.Hand;


        if(handedness == Handedness.Right)
		{
            //MLHandTracking.Right
        }
        else if(handedness == Handedness.Left)
		{
            //MLHandTracking.Left
        }

       //MLHandTracking.KeyposeManager.OnHandKeyPoseBeginDelegate
    }

    public void SubscribeOnGesture()
	{

	}

	//TODO: Some hand events, gesture data, etc


}
