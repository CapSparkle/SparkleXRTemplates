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

		int numberOfSelectors = selectorsManager.selectors.Count;

		for (int i = 0; i < currentCount; i++)
		{
			EditorGUILayout.BeginHorizontal();
			for(int j = 0; j < numberOfSelectors; j ++)
			{
				bool currentValue = selectorsManager.minSelectRequirements[i].Contains(j);

				GUILayoutOption[] GUILayoutOptions = {
					GUILayout.ExpandWidth(false),
					GUILayout.ExpandHeight(false),
					GUILayout.Width(25)
				};

				GUIStyle GUIStyle = new GUIStyle();
				GUIStyle.fixedHeight = 25;
				GUIStyle.fixedWidth = 25;
				GUIStyle.normal.background = Texture2D.whiteTexture;
				GUIStyle.normal.background = Resources.Load("asdasd") as Texture2D;
				//GUIStyle.border = new RectOffset();
				GUIStyle.margin.right = 0;
				GUIStyle.margin.left = 0;
				GUIStyle.margin.top = 15;
				GUIStyle.margin.left = 0;
				//GUIStyle.contentOffset = new Vector2(200f, 200f);

				bool valueSet = EditorGUILayout.Toggle(selectorsManager.minSelectRequirements[i].Contains(j), GUIStyle, GUILayoutOptions);
				
				if(valueSet != currentValue)
				{
					if(valueSet)
						selectorsManager.minSelectRequirements[i].Add(j);
					else
						selectorsManager.minSelectRequirements[i].RemoveAll(x => { return x == j; });
				}
			}
			EditorGUILayout.EndHorizontal();
		}



		base.serializedObject.ApplyModifiedProperties();
	}
}
