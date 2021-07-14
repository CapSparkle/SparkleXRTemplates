using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SparkleXRTemplates
{
    public abstract class Selector : MonoBehaviour
    {
        protected static List<Selector> s_selectorsList = new List<Selector>();

        private void Awake()
        {
            s_selectorsList.Add(this);
        }

        #region -interactables list operations-

        protected List<GameInteractable> m_selectedInteractables = new List<GameInteractable>() { };
        public List<GameInteractable> selectedInteractables
        {
            get
            { 
                return m_selectedInteractables; 
            }
            protected set
            {
                print("change occured!");
                foreach(GameInteractable interactable in value)
                {
                    print("Try add an GameInteractable");
                    AddInteractable(interactable);
                }
            }
        }

        public void AddInteractable(GameInteractable extention)
        {
            if (m_selectingPredicate == null)
            {
                m_selectedInteractables.Add(extention);
            }
            else if (m_selectingPredicate.Check(extention))
            {
                m_selectedInteractables.Add(extention);
            }
        }

        [SerializeField]
        protected SelectionPredicate m_selectingPredicate;

        #endregion -interactables list operations-

        #region -naming-
        //TODO: make name uniqueness resolving in UnityEditor

        [SerializeField]
        static int s_selectorUIDCounter = -1;
        
        [SerializeField]
        protected int m_selectorUID;

        [SerializeField]
        string m_selectorDescriptor = "";
        public string SelectorDescriptor
        {
            get
            { return m_selectorDescriptor;}
            private set
            { m_selectorDescriptor = value;}
        }

        [SerializeField]
        static List<string> s_descriptorsList = new List<string>();

        void Naming()
        {
            s_selectorUIDCounter += 1;
            m_selectorUID = s_selectorUIDCounter;

            if (m_selectorDescriptor == "")
                m_selectorDescriptor += m_selectorUID.ToString();

            if (s_descriptorsList.Contains(m_selectorDescriptor))
            {
                for (uint i = 1; i < uint.MaxValue; i++)
                {
                    if (!s_descriptorsList.Contains(m_selectorDescriptor + "." + i.ToString()))
                    {
                        m_selectorDescriptor = m_selectorDescriptor + "." + i.ToString();
                        break;
                    }
                }
            }

            s_descriptorsList.Add(m_selectorDescriptor);
        }

        #endregion -naming-

        protected abstract void GetInteractables();

        public virtual List<GameInteractable> SortInteractables(List<GameInteractable> interactables = null)
        {
            return m_selectedInteractables;
        }
    }
}