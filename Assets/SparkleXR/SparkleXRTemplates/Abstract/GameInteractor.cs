using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


namespace SparkleXRTemplates
{
    public abstract class GameInteractor : MonoBehaviour
    {
        public XRInputProvider myXRInputProvider;

		private void Start()
		{
            gameInteractableLocked = null;
		}

		public bool LockGameInteractor(GameInteractable interactableLocking)
		{
            if (interactableLocking == null ||
                gameInteractableLocked == null )
			{
                gameInteractableLocked = interactableLocking;
                return true;
            } 
            else
			{
                return false;
            }

        }
        
        public GameInteractable gameInteractableLocked { get; private set; }

        List<GameInteractable> _currentGameInteractables = new List<GameInteractable>();
        public List<GameInteractable> currentGameInteractables
        {
            get
            {
                return _currentGameInteractables;
            }
            private set
            {
                _currentGameInteractables = value;
            }
        }

        public void SetGameInteractables(List<GameInteractable> newGameInteractables)
        {
            if (gameInteractableLocked != null)
                return;

            IEnumerable<GameInteractable> toUninteract;
            if (currentGameInteractables != null && currentGameInteractables.Count != 0)
                toUninteract = currentGameInteractables.Except(newGameInteractables);
            else
                toUninteract = new List<GameInteractable>();
            
            foreach (GameInteractable gameInteractable in toUninteract)
            {
                UnInteract(gameInteractable);
            }

            currentGameInteractables = currentGameInteractables.AsEnumerable().Except(toUninteract).ToList();

            IEnumerable<GameInteractable> toInteract;
            if (newGameInteractables != null && newGameInteractables.Count != 0)
                if (currentGameInteractables.Count != 0)
                    toInteract = newGameInteractables.Except(currentGameInteractables);
                else
                    toInteract = newGameInteractables;
            else
                toInteract = new List<GameInteractable>();

            foreach (GameInteractable gameInteractable in toInteract)
            {
                Interact(gameInteractable);
            }

            currentGameInteractables = currentGameInteractables.Concat(toInteract).ToList();
        }

        protected void Interact(GameInteractable gameInteractable)
        {
            gameInteractable.Interact(GetComponent<GameInteractor>());
        }
        protected virtual void UnInteract(GameInteractable gameInteractable)
        {
            gameInteractable.UnInteract(GetComponent<GameInteractor>());
        }
    }
}
