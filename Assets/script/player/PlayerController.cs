using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using Assets.script;
using Assets.script.components.registers;

public class PlayerController : MonoBehaviour, GameStateManager.GameStateChangeListener, GameEntity {

	private GameStateManager gameManager;
	private FirstPersonController fpController;
	private LongbowShoot bowController;
	private UIController uiController;
	// Use this for initialization

	private int currentHealth = 100;
	private int maxHealth = 100;
	private int currentScore = 0;

	void Awake() {
		InjectionRegister.Register(this);
	}

	void Start() {
		gameManager = GameObject.FindGameObjectWithTag(Assets.script.TagConstants.GAME_STATE_MANAGER).GetComponent<GameStateManager>();
		uiController = GameObject.FindGameObjectWithTag(Assets.script.TagConstants.UI.UI_CONTROLLER).GetComponent<UIController>();

		fpController = GetComponent<FirstPersonController>();
		bowController = GetComponentInChildren<LongbowShoot>();

		bowController.enabled = false;
		fpController.enabled = false;
		gameManager.RegisterListener(this);
	}
	
	// Update is called once per frame
	void Update() {
		uiController.UpdatePlayerLife(currentHealth, maxHealth);
		uiController.UpdateCurrentScore(currentScore);
		CheckDead();
	}

	void Destroy() {

	}

	public void OnGameStateChanged(GameStateManager.GameState oldState, GameStateManager.GameState newState) {

		bool behaviorScriptsEnabled = newState == GameStateManager.GameState.Playing;

		fpController.enabled = behaviorScriptsEnabled;
		bowController.enabled = behaviorScriptsEnabled;
	}

	public void UpdateScore(int score) { 
		currentScore = score;
	}

	public void UpdateLifeMax(int addLifeMax) {
		currentHealth += addLifeMax;
		maxHealth = addLifeMax;
	}

	public void DealDamage(int dmg) {
		currentHealth -= dmg;
	}

	private void CheckDead() {
		if( currentHealth <= 0 ) {
			gameManager.SetNewState(GameStateManager.GameState.Paused);
		}
	}

	public string GetTag() {
		return Assets.script.TagConstants.PLAYER;
	}

	public void SetupComponents() {
		// todo 
	}
}
