using System;
using System.Collections.Generic;
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
            _observingMethods = observingMethods;
            _interactor = interactor;
        }

        public void Notify()
        {
            _observingMethods.Invoke(_interactor);
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


