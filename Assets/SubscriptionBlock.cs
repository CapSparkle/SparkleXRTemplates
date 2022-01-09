
using UnityEngine;
using UnityEngine.Events;

namespace SparkleXRTemplates {
    public class SubscriptionBlock
    {
        //On certain determined event from this interactor 
        GameInteractor _interactor;

        //Subscribed the following methods
        UnityEvent<GameInteractor> _observingMethods;

        public SubscriptionBlock(UnityEvent<GameInteractor> observingMethods, GameInteractor interactor)
        {
            Debug.Log("Subscr BLOCK CUNSTRUCTED");
            _observingMethods = observingMethods;
            _interactor = interactor;
            callsCounter = 0;
        }

        int callsCounter;
        public void Notify()
        {
            //Debug.Log(_interactor.ToString());
            //Debug.Log(_observingMethods.ToString());
            //Debug.Log(typeof(_observingMethods));

            int countOfSubscribers = _observingMethods.GetPersistentEventCount();

            for (int i = 0; i < countOfSubscribers; i++)
			{
                //Here is such a strange solution because if method with one parameter serializes via UnityEvent<T>
                //it invokes only with parameter that is setup via inspector
                UnityEngine.Object targetObject = _observingMethods.GetPersistentTarget(i);
                Debug.Log("CALL NUMBER " + callsCounter.ToString() + " of interactor: " + _interactor.ToString());
                callsCounter += 1;
                targetObject.GetType().GetMethod(_observingMethods.GetPersistentMethodName(i)).Invoke(targetObject, new[] { _interactor });
            }
        }
    }

    public class SubscriptionBlock<T>
    {
        //On certain determined event from this interactor 
        GameInteractor _interactor;

        //Subscribed the following methods
        UnityEvent<GameInteractor,T> _observingMethods;

        public SubscriptionBlock(UnityEvent<GameInteractor, T> observingMethods, GameInteractor interactor)
        {
            _observingMethods = observingMethods;
            _interactor = interactor;
        }

        public void Notify(T t)
        {
            _observingMethods.Invoke(_interactor, t);
        }
    }
}


