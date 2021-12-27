using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SparkleXRTemplates.MagicLeap;

public class MLHandDataVisualizer : MonoBehaviour
{
    [SerializeField]
    MLHandInputProvider MLHandInputProvider;

    public LineRenderer one, two;

    // Update is called once per frame
    void Update()
    {
        Vector3 MPC = MLHandInputProvider.MPCThumbPosition;
        Vector3 palmCenter = MLHandInputProvider.handCenterPosition;
        Vector3 wristCenter = MLHandInputProvider.wristCenterPosition;

        one.SetPosition(0, wristCenter);
        one.SetPosition(1, palmCenter);

        two.SetPosition(0, wristCenter);
        two.SetPosition(1, MPC);
    }
}
