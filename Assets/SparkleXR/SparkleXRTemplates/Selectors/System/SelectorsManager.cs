
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Xml.Serialization;

namespace SparkleXRTemplates
{
    public class SelectorsManager : MonoBehaviour
    {
        [SerializeField] private GameInteractor correspondingGameInteractor;

        [SerializeField]
        List<Selector> selectors;

        [SerializeField]
        string selectionPolicy = "";

        List<List<int>> minSelectRequirments = new List<List<int>>
        {
            new List<int>
            {
                0,
            },
        };
        // Priority by list index

        List<GameInteractable> selectedSet = new List<GameInteractable>() { };
        List<GameInteractable> previousSelectedSet = new List<GameInteractable>() { };

        int lastSelectingMinGroupIndex;
        List<GameInteractable> ChooseInteractables()
        {
            lastSelectingMinGroupIndex = 0;
            List<GameInteractable> IntersectSet = new List<GameInteractable>() { };
            foreach (List<int> selectorsSet in minSelectRequirments)
            {
                IntersectSet = selectors[selectorsSet[0]].selectedInteractables;
              
                List<int> listOfInts = new List<int>(selectorsSet);
                listOfInts.RemoveAt(0);

                foreach (int selectorIndex in listOfInts)
                {
                    if (IntersectSet != null)
                        IntersectSet = IntersectSet.Intersect(selectors[selectorIndex].selectedInteractables).ToList();
                    else
                        break;
                }

                if (IntersectSet.Count != 0)
                    return IntersectSet;

                lastSelectingMinGroupIndex ++;
            }

            return IntersectSet;
        }

        #region -policy parsing-

        void ParseSelectionPolicy()
        {
            int i = 0;
            int list_index = -1;
            minSelectRequirments = new List<List<int>>();
            while (i < selectionPolicy.Length)
            {
                if(selectionPolicy[i] == '(')
                {
                    i += 1;

                    if ((minSelectRequirments.Count == 0) || (minSelectRequirments[list_index].Count > 0))
                    {
                        minSelectRequirments.Add(new List<int>());
                        list_index += 1;
                    }    

                    for (; i < selectionPolicy.Length; i += 1)
                    {
                        try
                        {
                            if(selectionPolicy[i] == ')')
                            {
                                i += 1;
                                break;
                            }

                            int currentInt = Convert.ToInt32(selectionPolicy[i]) - 48;

                            if ((0 <= currentInt) && (currentInt < selectors.Count))
                            {
                                minSelectRequirments[list_index].Add(currentInt);
                            }
                            else
                            {
                                print(currentInt);
                                ParseErrorOccured();
                                return;
                            }
                        }
                        catch
                        {
                            ParseErrorOccured();
                            return;
                        }
                    }
                }
                else
                {
                    ParseErrorOccured();
                    return;
                }
            }

            print("Selection policy string parsed succesfully");
            foreach (List<int> lint in minSelectRequirments)
            {
                string strToPrint = "";
                foreach (int numb in lint)
                    strToPrint += numb.ToString();

                //print(strToPrint);
            }
                
        }

        void ParseErrorOccured()
        {
            minSelectRequirments = new List<List<int>>();
            print("selection policy string is incorrect");
        }

        #endregion -policy parsing-

        // Start is called before the first frame update
        void Start()
        {
            if (selectionPolicy != "")
                ParseSelectionPolicy();

            SelectedInteractables += DummyMethod;
        }

        public delegate void InteractabelSetNotify(List<GameInteractable> lint);
        public InteractabelSetNotify SelectedInteractables;

        public void DummyMethod (List<GameInteractable> linteractables)
        {
            //Do nothing;
        }

        // Update is called once per frame
        void Update()
        {
            previousSelectedSet = selectedSet;
            selectedSet = ChooseInteractables();

            switch (lastSelectingMinGroupIndex)
            {
                case 0 :
                    selectedSet = selectors[0].SortInteractables(selectedSet);
                    break;
                case 1:
                    print(selectedSet.Count);
                    selectedSet = ManagerChooseRules.PrioritizeOneSelector(selectors[0], selectedSet);
                    break;
                default:
                    break;
            }

            SelectedInteractables(selectedSet);
            correspondingGameInteractor.SetGameInteractables(selectedSet);

            //==== Debug output ==========

            if (selectedSet.Count != 0)
            {
                bool fflag = true;
                foreach (GameInteractable inter in selectedSet)
                {
                    if (fflag)
                    {
                        print("FIRST SELECTED: " + inter.transform.name);
                        fflag = false;
                    }
                    else
                        print(inter.transform.name);
                }
            }
            // ============================
        }

        public virtual GameInteractable ChooseFirstSelected()
        {
            return selectedSet[0];
        }

    }
}