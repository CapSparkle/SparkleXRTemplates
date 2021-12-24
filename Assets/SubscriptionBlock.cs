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
        List<Action<GameInteractor>> _observingMethods;

        public SubscriptionBlock(List<Action<GameInteractor>> observingMethods, GameInteractor interactor)
        {
            _observingMethods = observingMethods;
            _interactor = interactor;

        }

        public void Notify()
        {
            foreach (Action<GameInteractor> method in _observingMethods)
                method(_interactor);
        }
    }

    public class SubscriptionBlock<T>
    {
        //On certain determined event from this interactor 
        GameInteractor _interactor;

        //Subscribed the following methods
        List<Action<GameInteractor, T>> _observingMethods;
        
        List<UnityEvent<GameInteractor,T>> __observingMethods;

        public SubscriptionBlock(List<Action<GameInteractor, T>> observingMethods, GameInteractor interactor)
        {
            _observingMethods = observingMethods;
            _interactor = interactor;

        }

        public void Notify(T t)
        {
            foreach (Action<GameInteractor, T> method in _observingMethods)
                method(_interactor, t);
        }
    }
}


