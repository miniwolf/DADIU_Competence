using Assets.script.animals;
using Assets.script.animals.types;
using Assets.script.controllers;
using UnityEngine;

namespace Assets.script.colliders {
	public class KillRange : MonoBehaviour {
		private AnimalHandler handler;
		private Wolf wolf;

		protected void Start() {
			handler = GetComponentInParent<AnimalHandler>();
			wolf = GetComponentInParent<Wolf>();
		}

		protected void OnTriggerEnter(Collider other) {
			if ( handler.Target == null || handler.Target != other || !wolf.CanKill ) {
				return;
			}

			handler.GetActionable().ExecuteAction(ControllableActions.Kill);
			wolf.CanKill = false;
		}

		protected void OnTriggerExit(Collider other) {
			if ( handler.Target == null || handler.Target != other ) {
				return;
			}

			wolf.CanKill = true;
			wolf.CanMove = true;
		}
	}
}
