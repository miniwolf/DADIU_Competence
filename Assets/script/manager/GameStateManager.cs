using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

	public enum GameState {
		Playing,
		Paused
	}

	public enum GameMode {
		Score,
		Survival
	}

	public interface GameStateChangeListener {
		void OnGameStateChanged(GameState oldState, GameState newState);
	}

	public interface GameModeChangeListener {
		void OnGameModeChanged(GameMode newMode);
	}

	public int gameLevelTimeLimit;
	//	public int endGameReloadTime;

	// Time.timeSinceLevelLoad - time spent in menu
	private int gameStartTime;
	private GameMode gameMode;
	private GameState gameState;
	private List<GameStateChangeListener> changeListeners = new List<GameStateChangeListener>();
	private List<GameModeChangeListener> modeListeners = new List<GameModeChangeListener>();

	// Use this for initialization
	void Start() {
		gameState = GameState.Paused;
		PausePlaying();
	}
	
	// Update is called once per frame
	void Update() {
		CheckEndGame();
	}

	void Destroy() {
		changeListeners.Clear();
		modeListeners.Clear();
		changeListeners = null;
		modeListeners = null;
	}

	public int GetRemainingTime() {
		if( gameMode == GameMode.Survival )
			return int.MaxValue;
		else
			// when time spent in menu + gameplay time is less than the total time since the level is loaded
			return  ( gameStartTime + gameLevelTimeLimit ) - ( (int)Time.timeSinceLevelLoad ); 
	}

	public void RegisterGameStateListener(GameStateChangeListener l) {
		changeListeners.Add(l);
	}

	public void UnRegisterGameStateListener(GameStateChangeListener l) {
		changeListeners.Remove(l);
	}

	public void RegisterModeListener(GameModeChangeListener l) {
		modeListeners.Add(l);
	}

	public void UnRegisterModeListener(GameModeChangeListener l) {
		modeListeners.Remove(l);
	}

	public void SetGameMode(GameMode newGameMode) {
		if( newGameMode != gameMode )
			foreach( GameModeChangeListener l in modeListeners )
				l.OnGameModeChanged(gameMode);

		gameMode = newGameMode;
	}

	public void SetNewState(GameState newState) {
		if( newState != gameState ) {

			foreach( GameStateChangeListener l in changeListeners ) {
				l.OnGameStateChanged(gameState, newState);
			}

			switch( newState ) {
			case GameState.Playing:
				StartPlaying();
				break;
			case GameState.Paused:
				PausePlaying();
				break;
			}

			gameState = newState;
		}
	}

	private void CheckEndGame() {
		if( gameMode == GameMode.Score ) {
			if( GameTimePassed() ) {
				StartCoroutine(EndGame());
			}	
		}
	}

	private bool GameTimePassed() {
		return GetRemainingTime() <= 0;
	}

	private IEnumerator EndGame() {
		yield return null;// new WaitForSeconds(endGameReloadTime); 
		SetNewState(GameState.Paused);
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}

	void StartPlaying() {
		Time.timeScale = 1f;
		gameStartTime = (int)Time.timeSinceLevelLoad;
	}

	void PausePlaying() {
		Time.timeScale = 0f;
		gameStartTime = 0;
	}
}
