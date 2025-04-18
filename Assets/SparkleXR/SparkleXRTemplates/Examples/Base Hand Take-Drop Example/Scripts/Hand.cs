﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SparkleXRTemplates.Examples
{
    public enum StateLabelType
    {
        smeared,

        carrying,
        rotating,

        tired,
		tensed
	}

	public class StateLabel
	{
        public StateLabelType stateLabelType;
        public string description;

		public StateLabel(StateLabelType stateLabelType, string description)
		{
			this.stateLabelType = stateLabelType;
			this.description = description;
		}
	}

	//As element of game content Hand "don't know" anything about XR input system.
	//Hand "lives" the most independent way it can
	public class Hand : GameInteractor
    {
        public bool busy = false;

        // <who have assigned that label, and the label itself>
        private Dictionary<GameInteractable, StateLabel> _stateOfHand = new Dictionary<GameInteractable, StateLabel>();
        public Dictionary<GameInteractable, StateLabel> StateOfHand
        {
            get
            {
                return _stateOfHand;
            }
            set
            {
                _stateOfHand = value;
                if (HaveIFlag(StateLabelType.carrying))
                    busy = true;
                else
                    busy = false;
            }
        }

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

        bool HaveIFlag(StateLabelType stateLabelType)
		{
            foreach (KeyValuePair<GameInteractable, StateLabel> kvp in _stateOfHand)
                if(kvp.Value.stateLabelType == stateLabelType)
                    return true;

            return false;
		}
    }
}