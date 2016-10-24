using Assets.script.animals;
using Assets.script.controllers;
using UnityEngine;

namespace Assets.script.colliders {
	public class Notice : MonoBehaviour {
		private AnimalHandler handler;

		protected void Start() {
			handler = GetComponentInParent<AnimalHandler>();
		}

		protected void OnTriggerEnter(Collider other) {
			if ( handler.Target != null ) {
				return;
			}

			handler.Target = other;
			handler.GetActionable().ExecuteAction(ControllableActions.Notice);
		}

		protected void OnTriggerExit(Collider other) {
			if ( handler.Target == null ) {
				return;
			}

			if ( handler.Target == other ) {
				handler.GetActionable().ExecuteAction(ControllableActions.Nodanger);
			}
		}
	}
}
