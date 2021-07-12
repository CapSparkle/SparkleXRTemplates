using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SparkleXRTemplates.Examples
{
    public class StateLabel
    {
        int someData;
    }

    //As element of game content Hand "don't want to know" anything about XR input system.
    //Hand "lives" the most independent way it can
    public class Hand : GameInteractor
    {
        public bool busy = false;
        List<StateLabel> stateOfHand = new List<StateLabel>();

        [SerializeField]
        Transform _handPivot;
        public Transform handPivot
        {
            get
            {
                return _handPivot;
            }
            protected set
            {
                _handPivot = value;
            }
        }
    }
}