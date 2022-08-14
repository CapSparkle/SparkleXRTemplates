
using UnityEngine;
using UnityEngine.XR.MagicLeap;

#if PLATFORM_LUMIN
namespace SparkleXRTemplates.MagicLeap
{
    public class ML6DOFReference : MonoBehaviour
    {
        [SerializeField]
        XRNodeType deviceTypeToRefer;


        [SerializeField]
        MLHandInputProvider handInputProvider;

        [SerializeField]
        MLControllerInputProvider controllerInputProvider;


        // Update is called once per frame
        void Update()
        {
            switch (deviceTypeToRefer)
            {
                case (XRNodeType.Hand):
                    transform.position = handInputProvider.handCenterPosition;
                    transform.rotation = handInputProvider.handOrientation;
                    break;

                /*case (XRNodeType.HMD):
                    break;*/

                case (XRNodeType.Controller):
                    transform.position = controllerInputProvider.controllerPosition;
                    transform.rotation = controllerInputProvider.controllerOrientation;
                    break;

                default:
                    break;
            }
        }
    }
}
#endif