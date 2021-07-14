using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SparkleXRTemplates
{
    public class RaycastSelector : Selector
    {
        [SerializeField]
        Transform _director;
        public Transform director
        {
            get
            {
                return _director;
            }
            private set
            {
                _director = value;
            }
        }

        [SerializeField]
        Transform _castSource;
        public Transform castSource
        {
            get
            {
                return _castSource;
            }
            private set
            {
                _castSource = value;
            }
        }

        [SerializeField]
        float _maxDistance;
        public float maxDistance
        {
            get
            {
                return _maxDistance;
            }
            private set
            {
                _maxDistance = value;
            }
        }
        
        [SerializeField]
        LayerMask ignoredLayers;

        public Action<RaycastHit[]> raycastHitInfo;

        protected override void GetInteractables()
        {
            m_selectedInteractables.Clear();

            RaycastHit[] hits = Physics.RaycastAll(castSource.position, director.position - castSource.position, _maxDistance);

            int i = 0;
            foreach (RaycastHit hit in hits)
            {
                print("Ray Caster (uid:" + m_selectorUID + ") " + "hit №" + i + " - " + hit.transform.name);
                GameInteractable handler;
                if (handler = hit.transform.GetComponent<GameInteractable>())
                    AddInteractable(handler);
                i++;
            }
        }

        #region -sorting-

        public override List<GameInteractable> SortInteractables(List<GameInteractable> sortedInteractables = null)
        {
            if (sortedInteractables != null)
            {
                selectedInteractables.Sort(Compare);
                return selectedInteractables;
            }
            sortedInteractables.Sort(Compare);
            return sortedInteractables;
        }

        public int Compare(GameInteractable x, GameInteractable y)
        {
            float xRange = Math.Abs((x.transform.position - castSource.position).magnitude);
            float yRange = Math.Abs((y.transform.position - castSource.position).magnitude);
            if (xRange > yRange)
                return 1;
            else if (xRange < yRange)
                return -1;
            else
                return 0;
        }

        #endregion -sorting-

        void Update()
        {
            GetInteractables();
        }
    }
}