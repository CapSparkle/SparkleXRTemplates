using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SparkleXRTemplates;

[CustomEditor(typeof(SelectorsManager))]
public class SelectorsManagerCustomEditor : Editor
{
	SelectorsManager selectorsManager;
	//public int configureSelectingGroups;

	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		selectorsManager = (SelectorsManager)target;

		int currentCount = selectorsManager.minSelectRequirements.Count;
		int countSet = EditorGUILayout.IntField("Number of Selecting Groups", currentCount);

		if(currentCount > countSet)
		{
			selectorsManager.minSelectRequirements.RemoveRange(countSet, currentCount - countSet);
		}
		else if(currentCount < countSet)
		{
			for(int i = 0; i < countSet - currentCount; i ++)
				selectorsManager.minSelectRequirements.Add(new List<int>());
		}

		for(int i = 0; i < currentCount; i++)
		{

		}



		base.serializedObject.ApplyModifiedProperties();
	}
}
