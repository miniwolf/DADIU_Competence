using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.ui {
	public class UIController : MonoBehaviour, GameStateManager.GameStateChangeListener {

		private GameObject mainMenuPanel, inGameMenuPanel;
		private Text textTimeRemaining, textHPAmount, textGameMode, textCurrentScore;
		private Dropdown dropdownGameMode;
		private Button buttonStartGame;

		private GameStateManager gameStateManager;

		void Start() {
			// fetch dependencies
			gameStateManager = GameObject.FindGameObjectWithTag(TagConstants.GAME_STATE_MANAGER).GetComponent<GameStateManager>();

			mainMenuPanel = GameObject.FindGameObjectWithTag(TagConstants.UI.PANEL_MAIN);
			inGameMenuPanel = GameObject.FindGameObjectWithTag(TagConstants.UI.PANEL_IN_GAME);

			textTimeRemaining = GetTextComponent(TagConstants.UI.TEXT_TIME_REMAINING);
			textHPAmount = GetTextComponent(TagConstants.UI.TEXT_HP_AMOUNT);
			textGameMode = GetTextComponent(TagConstants.UI.TEXT_GAME_MODE);
			textCurrentScore = GetTextComponent(TagConstants.UI.TEXT_CURRENT_SCORE);

			dropdownGameMode = GameObject.FindGameObjectWithTag(TagConstants.UI.DROPDOWN_GAME_MODE).GetComponent<Dropdown>();
			buttonStartGame = GameObject.FindGameObjectWithTag(TagConstants.UI.BUTTON_START_GAME).GetComponent<Button>();

			// init listeners
			buttonStartGame.onClick.AddListener(OnStartGame);
			dropdownGameMode.onValueChanged.AddListener(delegate {
				OnGameModeChanged();
			});

			// controll start state
			textGameMode.text = "Game mode: Score";
			gameStateManager.RegisterGameStateListener(this);
			gameStateManager.SetNewState(GameStateManager.GameState.Paused);
		}

		protected void Update() {
			UpdateTimeRemaining(gameStateManager.GetRemainingTime());
		}

		protected void Destroy() {
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

		public void UpdateCurrentScore(int score) {
			textCurrentScore.text = "Score: " + score;
		}

		private GameStateManager.GameMode ResolveGameMode() {
			return dropdownGameMode.value == 0 ? GameStateManager.GameMode.Score : GameStateManager.GameMode.Survival;
		}

		private void OnGameModeChanged() {
			switch( ResolveGameMode() ) {
				case GameStateManager.GameMode.Score:
					textTimeRemaining.gameObject.SetActive(true);
					textGameMode.text = "Game mode: Score";
					break;
				case GameStateManager.GameMode.Survival:
					textTimeRemaining.gameObject.SetActive(false);
					textGameMode.text = "Game mode: Survival";
					break;
			}
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
}
