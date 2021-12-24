using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//TODO: undepend from magic leap
using SparkleXRTemplates;

namespace SparkleXRTemplates
{
	[Serializable]
	public class UnityEventMethodHolder : UnityEvent<GameInteractor, float> {
	
	}

	[Serializable]
	public class GameInteractorFloatAction : UnityEvent<GameInteractor, float>
	{

		/*GameInteractor _interactor;


		public GameInteractorFloatAction(GameInteractor interactor)
		{
			_interactor = interactor;
		}

		public void Notify(float f)
		{
			this.Invoke(_interactor, f);
		}*/

	}

}

