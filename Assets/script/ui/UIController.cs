using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIController : MonoBehaviour, GameStateManager.GameStateChangeListener {


	private GameObject mainMenuPanel, inGameMenuPanel;
	private Text textTimeRemaining, textHPAmount, textGameMode;
	private Dropdown dropdownGameMode;
	private Button buttonStartGame;

	private GameStateManager gameStateManager;

	void Start() {
		// fetch dependencies
		gameStateManager = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.GAME_STATE_MANAGER).GetComponent<GameStateManager>();

		mainMenuPanel = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.UI.PANEL_MAIN);
		inGameMenuPanel = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.UI.PANEL_IN_GAME);

		textTimeRemaining = GetTextComponent(AssemblyCSharp.TagConstants.UI.TEXT_TIME_REMAINING);
		textHPAmount = GetTextComponent(AssemblyCSharp.TagConstants.UI.TEXT_HP_AMOUNT);
		textGameMode = GetTextComponent(AssemblyCSharp.TagConstants.UI.TEXT_GAME_MODE);

		dropdownGameMode = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.UI.DROPDOWN_GAME_MODE).GetComponent<Dropdown>();
		buttonStartGame = GameObject.FindGameObjectWithTag(AssemblyCSharp.TagConstants.UI.BUTTON_START_GAME).GetComponent<Button>();

		// init listeners 
		buttonStartGame.onClick.AddListener(OnStartGame);
		dropdownGameMode.onValueChanged.AddListener(delegate {
			OnGameModeChanged();
		});

		// controll start state
		gameStateManager.RegisterListener(this);
		gameStateManager.SetNewState(GameStateManager.GameState.Paused);
	}

	void Update() {
		UpdateTimeRemaining(gameStateManager.GetRemainingTime());
	}

	void Destroy() {
		dropdownGameMode.onValueChanged.RemoveAllListeners();
	}

	public void OnStartGame() {
		gameStateManager.SetGameMode(ResolveGameMode());
		gameStateManager.SetNewState(GameStateManager.GameState.Playing);
	}

	public void UpdatePlayerLife(int lifeValue, int maxValue) { 
		textHPAmount.text = "HP: " + lifeValue  + "/" + maxValue;
	}

	public void UpdateTimeRemaining(int seconds) { 
		textTimeRemaining.text = "Time remaining: " + seconds + (seconds > 1 ? " seconds" : " SECOND!");
	}


	private GameStateManager.GameMode ResolveGameMode() {
		return 	dropdownGameMode.value == 0 ? GameStateManager.GameMode.Score : GameStateManager.GameMode.Survival;
	}

	private void OnGameModeChanged() {
		switch( ResolveGameMode() ) {
		case GameStateManager.GameMode.Score: 
				textTimeRemaining.gameObject.SetActive(true);
				break;
			case GameStateManager.GameMode.Survival: 
				textTimeRemaining.gameObject.SetActive(false);
				break;
		}

		Debug.Log(dropdownGameMode.value);
	}

	public void OnGameStateChanged(GameStateManager.GameState oldState, GameStateManager.GameState newState) {

		switch( newState ) {
			case GameStateManager.GameState.Paused:
				inGameMenuPanel.SetActive(false);
				mainMenuPanel.SetActive(true);
				break;
			case GameStateManager.GameState.Playing:
				inGameMenuPanel.SetActive(true);
				mainMenuPanel.SetActive(false);
				break;
		}
	}

	private Text GetTextComponent(string tag) {
		return GameObject.FindGameObjectWithTag(tag).GetComponent<Text>();
	}
}
