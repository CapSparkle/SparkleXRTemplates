using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SparkleXRTemplates;


namespace SparkleXRTemplates.Examples
{
    public class SightVisor : MonoBehaviour
    {
        bool isSightVisorOn;

        public void TurnOn()
		{
            isSightVisorOn = true;
            selectionGroupToggle.ToggleOnAdvancedSelecting();

        }

        public void TurnOff()
		{
            selectionGroupToggle.ToggleOffAdvancedSelecting();
            isSightVisorOn = false;
		}

        [SerializeField]
        GameObject sightVisorModel;

        [SerializeField]
        GameObject visorInterface;

        [SerializeField]
        SelectionGroupToggle selectionGroupToggle;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
