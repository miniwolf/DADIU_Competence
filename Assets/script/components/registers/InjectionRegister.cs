using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp {
	public class InjectionRegister : MonoBehaviour {
		private static List<GameEntity> components = new List<GameEntity>();
		private PlayerFactory controllableFactory;

		void Awake() {
			controllableFactory = new PlayerFactory();
		}

		void Start() {
			InitializeComponents();
		}

		void OnDestroy() {
			components.Clear();
		}

		public static void Register(GameEntity component) {
			components.Add(component);
		}

		private void InitializeComponents() {
			foreach ( GameEntity component in components ) {
				InitializeComponent(component);
				component.SetupComponents();
			}
		}

		private void InitializeComponent(GameEntity component) {
			switch ( component.GetTag() ) {
			case TagConstants.PLAYER:
				controllableFactory.CreatePlayer((Actionable) component);
				break;
			}
		}
	}
}