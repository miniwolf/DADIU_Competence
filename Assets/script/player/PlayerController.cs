using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using Assets.script;
using Assets.script.components.registers;

public class PlayerController : MonoBehaviour, GameStateManager.GameStateChangeListener, GameEntity {


	private int playerHp;
	private GameStateManager gameManager;
	private FirstPersonController fpController;
	private LongbowShoot bowController;
	// Use this for initialization

	void Awake() {
		InjectionRegister.Register(this);
	}

	void Start() {
		gameManager = GameObject.FindGameObjectWithTag(Assets.script.TagConstants.GAME_STATE_MANAGER).GetComponent<GameStateManager>();
		fpController = GetComponent<FirstPersonController>();
		bowController = GetComponentInChildren<LongbowShoot>();

		bowController.enabled = false;
		fpController.enabled = false;
		gameManager.RegisterListener(this);
	}
	
	// Update is called once per frame
	void Update() {
	
	}


	public void OnGameStateChanged(GameStateManager.GameState oldState, GameStateManager.GameState newState) {

		bool behaviorScriptsEnabled = newState == GameStateManager.GameState.Playing;

		fpController.enabled = behaviorScriptsEnabled;
		bowController.enabled = behaviorScriptsEnabled;
	}

	public string GetTag() {
		return Assets.script.TagConstants.PLAYER;
	}

	public void SetupComponents() {
		// todo 
	}
}
