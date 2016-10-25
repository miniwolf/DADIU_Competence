using Assets.Longbow.Scripts.CS;
using Assets.script.components;
using Assets.script.components.registers;
using Assets.script.ui;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.script.player {
	public class PlayerController : MonoBehaviour, GameStateManager.GameStateChangeListener, GameEntity, Damageable {

		private GameStateManager gameManager;
		private FirstPersonController fpController;
		private LongbowShoot bowController;
		private UIController uiController;
		// Use this for initialization

		public int startHealth = 100;
		private int currentHealth;
		private int maxHealth = 100;
		private int currentScore;
		private bool abilityGainHp = false;
		private System.Random random = new System.Random();

		void Awake() {
			InjectionRegister.Register(this);
			currentHealth = startHealth;
		}

		void Start() {
			gameManager = GameObject.FindGameObjectWithTag(TagConstants.GAME_STATE_MANAGER).GetComponent<GameStateManager>();
			uiController = GameObject.FindGameObjectWithTag(TagConstants.UI.UI_CONTROLLER).GetComponent<UIController>();

			fpController = GetComponent<FirstPersonController>();
			bowController = GetComponentInChildren<LongbowShoot>();

			bowController.enabled = false;
			fpController.enabled = false;
			gameManager.RegisterGameStateListener(this);
		}

		// Update is called once per frame
		void Update() {
			uiController.UpdatePlayerLife(currentHealth, maxHealth);
			uiController.UpdateCurrentScore(currentScore);
			CheckDead();
		}

		public void OnGameStateChanged(GameStateManager.GameState oldState, GameStateManager.GameState newState) {
			bool behaviorScriptsEnabled = newState == GameStateManager.GameState.Playing;

			fpController.enabled = behaviorScriptsEnabled;
			bowController.enabled = behaviorScriptsEnabled;
		}

		public void UpdateScore(int score) {
			currentScore = score;
			UpdatePerks();
		}

		public void IncrementScore(int score) {
			Debug.Log("Player score increment: " + score);
			currentScore += score;
			UpdatePerks();
		}

		public void UpdateLifeMax(int addHealth) {
			currentHealth += addHealth;
			maxHealth += addHealth;
		}

		public void IncrementLife(int incr) {
			currentHealth += incr;
			if(currentHealth > maxHealth) 
				currentHealth = maxHealth;
		}

		private void UpdatePerks() {
			if( currentScore % 5 == 0 ) {
				maxHealth += 10;
				IncrementLife(10);
			}

			if( abilityGainHp ) {
				IncrementLife(5);
			}

			if(random.Next(2) == 0) {
				if( !abilityGainHp ) {
					GainAbility();
				} else {
					RemoveAbility();					
				}
			}
		}

		private void GainAbility() {
			abilityGainHp = true;
			Renderer[] rs = GetComponentsInChildren<Renderer>(); 
			foreach( Renderer r in rs ) {
				Debug.Log("Original color: " + r.material.color);
				r.material.color = Color.blue;
			}
		}

		private void RemoveAbility() {
			abilityGainHp = false;
			Renderer[] rs = GetComponentsInChildren<Renderer>(); 
			foreach( Renderer r in rs ) {
				Debug.Log("Original color: " + r.material.color);
				r.material.color = Color.white;
			}
		}

		private void CheckDead() {
			if( currentHealth <= 0 ) {
				gameManager.SetNewState(GameStateManager.GameState.Paused);
			}
		}

		public string GetTag() {
			return TagConstants.PLAYER;
		}

		public void SetupComponents() {
			// todo
		}

		public void TakeDamage(int dmg) {
			currentHealth -= dmg;
		}
	}
}
