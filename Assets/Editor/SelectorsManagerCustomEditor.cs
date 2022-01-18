using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using SparkleXRTemplates;

[CustomEditor(typeof(SelectorsManager))]
public class SelectorsManagerCustomEditor : Editor
{

	GUIStyle pressedButtongGUIStyle = new GUIStyle();
	GUIStyle unPressedButtongGUIStyle = new GUIStyle();

	GUILayoutOption[] GUILayoutOptions = 
	{
		GUILayout.ExpandWidth(false),
		GUILayout.ExpandHeight(true),
	};


	private void FormGUIStyles()
	{
		pressedButtongGUIStyle.normal.background = Resources.Load("LightBlueButtonUI") as Texture2D;

		pressedButtongGUIStyle.fixedHeight = 25;
		pressedButtongGUIStyle.fixedWidth = 70;
		
		pressedButtongGUIStyle.margin.right = 5;
		pressedButtongGUIStyle.margin.left = 5;
		pressedButtongGUIStyle.margin.top = 1;
		pressedButtongGUIStyle.margin.bottom = 5;

		pressedButtongGUIStyle.padding.right = 5;
		pressedButtongGUIStyle.padding.left = 4;
		pressedButtongGUIStyle.padding.top = 1;
		pressedButtongGUIStyle.padding.bottom = 1;

		pressedButtongGUIStyle.alignment = TextAnchor.MiddleLeft;

		pressedButtongGUIStyle.wordWrap = false;

		pressedButtongGUIStyle.clipping = TextClipping.Clip;


		unPressedButtongGUIStyle.normal.background = Resources.Load("LightGreyButtonUI") as Texture2D;


		unPressedButtongGUIStyle.fixedHeight = 25;
		unPressedButtongGUIStyle.fixedWidth = 70;

		unPressedButtongGUIStyle.margin.right = 5;
		unPressedButtongGUIStyle.margin.left = 5;
		unPressedButtongGUIStyle.margin.top = 1;
		unPressedButtongGUIStyle.margin.bottom = 5;

		unPressedButtongGUIStyle.padding.right = 5;
		unPressedButtongGUIStyle.padding.left = 4;
		unPressedButtongGUIStyle.padding.top = 1;
		unPressedButtongGUIStyle.padding.bottom = 1;

		unPressedButtongGUIStyle.alignment = TextAnchor.MiddleLeft;

		unPressedButtongGUIStyle.wordWrap = false;

		unPressedButtongGUIStyle.clipping = TextClipping.Clip;
	}

	private void Awake()
	{
		FormGUIStyles();
	}

	SelectorsManager selectorsManager;

	public override void OnInspectorGUI()
	{
		EditorGUILayout.PropertyField(serializedObject.FindProperty("correspondingGameInteractor"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("selectionPredicate"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("selectors"));

		selectorsManager = (SelectorsManager)target;

		int currentCount = selectorsManager.selectRequirements.Count;
		int countSet = EditorGUILayout.IntField("Number of Selecting Groups", currentCount);

		if (currentCount > countSet)
		{
			selectorsManager.selectRequirements.RemoveRange(countSet, currentCount - countSet);
		}
		else if (currentCount < countSet)
		{
			for (int i = 0; i < countSet - currentCount; i++)
				selectorsManager.selectRequirements.Add(new List<int>());
		}

		int numberOfSelectors = selectorsManager.selectors.Count;

		for (int i = 0; i < currentCount; i++)
		{
			EditorGUILayout.BeginHorizontal();
			for (int j = 0; j < numberOfSelectors; j++)
			{

				bool currentValue = selectorsManager.selectRequirements[i].Contains(j);


				GUIContent content = new GUIContent(j + ". " + selectorsManager.selectors[j].name);

				bool valueSet = GUILayout.Button(content, currentValue ? pressedButtongGUIStyle : unPressedButtongGUIStyle, GUILayoutOptions);


				if (valueSet)
				{
					if (currentValue)
						selectorsManager.selectRequirements[i].RemoveAll(x => { return x == j; });
					else
						selectorsManager.selectRequirements[i].Add(j);
				}
			}
			EditorGUILayout.EndHorizontal();
		}



		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}

		serializedObject.ApplyModifiedProperties();
	}

	/*public override void OnInspectorGUI()
	{
		

	}*/
}
