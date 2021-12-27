
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Xml.Serialization;
//using Sirenix.Serialization;
//using Sirenix.OdinInspector;

namespace SparkleXRTemplates
{
    public class SelectorsManager : MonoBehaviour
    {
        [SerializeField] 
        GameInteractor correspondingGameInteractor;

        [SerializeField]
        List<Selector> selectors;

        //TODO: Apply to every GameInteractable from every selector
        [SerializeField]
        SelectionController selectionPredicate;

        //TODO: Add a visualisation in editor or reference to a config
        [SerializeField]
		List<List<int>> _minSelectRequirments = new List<List<int>>()
		{
            new List<int> (){ 0, 1 },
            new List<int> (){ 0, 2 }
		};

		public List<List<int>> minSelectRequirments
		{
            get
			{
                return _minSelectRequirments;
			}
            set
			{
                
                for(int i = 0; i < value.Count; i ++)
				{
                    //sanitize
                    value[i] = value[i].Distinct().ToList();

                    //validate
                    foreach (int selectorIndex in value[i])
                        if ((selectorIndex >= selectors.Count()) ||
                            selectorIndex < 0)
                            return;

                }

                _minSelectRequirments = value;
			}
		}


		#region -selection rules forming-


		// Priority by list index

		/*void ParseSelectionPolicy()
        {
            int i = 0;
            int list_index = -1;
            minSelectRequirments = new List<List<int>>();
            while (i < selectionRules.Length)
            {
                if (selectionRules[i] == '(')
                {
                    i += 1;

                    if ((minSelectRequirments.Count == 0) || (minSelectRequirments[list_index].Count > 0))
                    {
                        minSelectRequirments.Add(new List<int>());
                        list_index += 1;
                    }

                    for (; i < selectionRules.Length; i += 1)
                    {
                        try
                        {
                            if (selectionRules[i] == ')')
                            {
                                i += 1;
                                break;
                            }

                            int currentInt = Convert.ToInt32(selectionRules[i]) - 48;

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
        }*/

		#endregion -selection rules parsing-


		int selectGroupIndex;
        List<GameInteractable> ChooseInteractables()
        {
            selectGroupIndex = 0;
            List<GameInteractable> IntersectSet = new List<GameInteractable>() { };
            foreach (List<int> selectorsIndexGroup in minSelectRequirments)
            {
                IntersectSet = selectors[selectorsIndexGroup[0]].selectedInteractables;

                List<int> listOfInts = new List<int>(selectorsIndexGroup);
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

                selectGroupIndex++;
            }

            selectGroupIndex = -1;

            return IntersectSet;
        }

        public Action<List<GameInteractable>> OnSelectedInteractablesSet;
        List<GameInteractable> selectedSet = new List<GameInteractable>() { };
        List<GameInteractable> previousSelectedSet = new List<GameInteractable>() { };


        public void DummyMethod (List<GameInteractable> linteractables)
        {
            //Do nothing;
        }
        void Start()
        {
            OnSelectedInteractablesSet += DummyMethod;
        }

        void Update()
        {
            previousSelectedSet = selectedSet;
            selectedSet = ChooseInteractables();

            if(selectGroupIndex >= 0)
			{
                selectedSet = ManagerChooseRules.PrioritizeOneSelector(selectors[minSelectRequirments[selectGroupIndex][0]], selectedSet);
            }
            else
			{
                selectedSet = new List<GameInteractable>() { };
            }

            OnSelectedInteractablesSet(selectedSet);
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
    }
}