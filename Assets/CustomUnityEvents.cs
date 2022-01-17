using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//TODO: undepend from magic leap
using SparkleXRTemplates;

namespace SparkleXRTemplates
{
	[Serializable]
	public class UnityEventGameInteractor : UnityEvent<GameInteractor>
	{

	}

	[Serializable]
	public class UnityEventGameInteractorBool : UnityEvent<GameInteractor, bool>
	{

	}

	[Serializable]
	public class UnityEventGameInteractorFloat : UnityEvent<GameInteractor, float> {
	
	}

	[Serializable]
	public class UnityEventGameInteractorVector2 : UnityEvent<GameInteractor, Vector2>
	{

	}

}

