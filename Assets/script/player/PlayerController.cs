using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour, GameStateManager.GameStateChangeListener {


	private int playerHp;
	private GameStateManager gameManager;
	private FirstPersonController fpController;
	private LongbowShoot bowController;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag(Assets.script.TagConstants.GAME_STATE_MANAGER).GetComponent<GameStateManager>();
		fpController = GetComponent<FirstPersonController>();
		bowController = GetComponentInChildren<LongbowShoot>();

		bowController.enabled = false;
		fpController.enabled = false;
		gameManager.RegisterListener(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnGameStateChanged(GameStateManager.GameState oldState, GameStateManager.GameState newState) {

		bool behaviorScriptsEnabled = newState == GameStateManager.GameState.Playing;

		fpController.enabled = behaviorScriptsEnabled;
		bowController.enabled = behaviorScriptsEnabled;
	}
}
