
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

        /*[SerializeField]
        MLHandInputProvider handInputProvider;*/

        [SerializeField]
        MLControllerConnectionHandlerBehavior _controllerConnectionHandler;
        MLInput.Controller controller = null;


        void Start()
        {
            if(_controllerConnectionHandler == null &&
                deviceTypeToRefer == XRNodeType.Controller)
			{
                _controllerConnectionHandler = GetComponent<MLControllerConnectionHandlerBehavior>();
			}
        }

        // Update is called once per frame
        void Update()
        {
            switch (deviceTypeToRefer)
            {
                case (XRNodeType.Hand):
                    transform.position = handInputProvider.handCenterPosition;
                    transform.rotation = handInputProvider.handOrientation;
                    break;

                case (XRNodeType.HMD):
                    break;

                case (XRNodeType.Controller):
                    if (controller != null)
                    {
                        transform.position = controller.Position;
                        transform.rotation = controller.Orientation;
                    }
                    else
                    {
                        controller = _controllerConnectionHandler.ConnectedController;
                        Debug.Log("no controller presented. try to take");
                    }
                    break;
            }
        }
    }
}
#endif