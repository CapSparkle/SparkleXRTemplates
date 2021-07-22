using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SparkleXRTemplates
{
    public class SpherecastSelector : Selector
    {
        // Director and cast source must be different GameObjects!

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
        float _radius;
        public float radius 
        {
            get
            { 
                return _radius; 
            }
            private set
            {
                _radius = value;
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
        LayerMask includedLayers;

        public Action<RaycastHit[]> spherecastHitInfo;
        
        protected override void GetInteractables()
        {
            selectedInteractables.Clear();

            if(castSource.position != director.position)
            {
                RaycastHit[] hits = Physics.SphereCastAll(castSource.position, _radius, director.position - castSource.position, _maxDistance);
                int i = 0;
                foreach (RaycastHit hit in hits)
                {
                    print("Sphere Caster (uid:" + m_selectorUID + ") " + "hit №" + i + " - " + hit.transform.name);
                    GameInteractable handler;
                    if (handler = hit.transform.GetComponent<GameInteractable>())
                        AddInteractable(handler);
                    i++;
                }
            }
            else
            {
                Collider[] colliders = Physics.OverlapSphere(castSource.position, _radius, includedLayers);
                int i = 0;
                print("i am working");
                foreach (Collider collider in colliders)
                {
                    print("Sphere Caster (uid:" + m_selectorUID + ") " + "hit №" + i + " - " + collider.transform.name);
                    GameInteractable gameInteractable = null;
                    if (gameInteractable = collider.transform.GetComponent<GameInteractable>())
                        AddInteractable(gameInteractable);

                    i++;
                }
            }
        }

        #region -sorting-
        public override List<GameInteractable> SortInteractables(List<GameInteractable> interactablesToSort = null)
        {
            if (interactablesToSort != null)
            {
                interactablesToSort.Sort(Compare);
                return interactablesToSort;
            }
            interactablesToSort.Sort(Compare);
            return interactablesToSort;
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