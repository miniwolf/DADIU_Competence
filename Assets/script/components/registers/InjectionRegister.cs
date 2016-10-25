using System.Collections.Generic;
using Assets.script.animals;
using Assets.script.animals.types;
using Assets.script.components.factory;
using UnityEngine;

namespace Assets.script.components.registers {
	public class InjectionRegister : MonoBehaviour {
		private static readonly List<GameEntity> components = new List<GameEntity>();
		private new static Camera camera;
		private static bool done;
		private static PlayerController player;


		protected void Start() {
			camera = GameObject.FindGameObjectWithTag(TagConstants.CAMERA).GetComponent<Camera>();		
			player = GameObject.FindGameObjectWithTag(TagConstants.PLAYER).GetComponent<PlayerController>();

			InitializeComponents();
			components.Clear();
			done = true;
		}

		protected void OnDestroy() {
			components.Clear();
		}

		public static void Register(GameEntity component) {
			components.Add(component);
		}

		public static void ReDo() {
			if ( !done ) {
				return;
			}
			InitializeComponents();
			components.Clear();
		}

		private static void InitializeComponents() {
			foreach ( var component in components ) {
				InitializeComponent(component);
				component.SetupComponents();
			}
		}

		private static void InitializeComponent(GameEntity component) {
			switch ( component.GetTag() ) {
				case TagConstants.PLAYER:
					//controllableFactory.CreatePlayer((Actionable) component);
					break;
				case TagConstants.DEER:
					new DeerFactory(((AnimalHandler) component).GetActionable(), (AnimalHandler)component, camera, player).Build();
					break;
				case TagConstants.WOLF:
					new WolfFactory(((AnimalHandler) component).GetActionable(), (AnimalHandler)component, player, (Wolf) component).Build();
					break;
			}
		}
	}
}