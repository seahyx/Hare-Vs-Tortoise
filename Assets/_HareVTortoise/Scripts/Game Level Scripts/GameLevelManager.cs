using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLevelManager : MonoBehaviour
{
	#region Type Definitions

	public enum GameState
	{
		WarmUp,
		InWave,
		Intermission,
		Boss,
		End
	}

	#endregion

	#region Serializables

	[SerializeField, Tooltip("Number of lives the player has.")]
	private int lives = 100;

	[SerializeField, Tooltip("Current wave number.")]
	private int currentWave = 0;

	[SerializeField, Tooltip("Current wave timer.")]
	public float CurrentWaveTimer = 0.0f;

	[SerializeField, Tooltip("Warm up duration.")]
	private float warmUpDuration = 15.0f;

	[SerializeField, Tooltip("Warm up timer.")]
	private float warmUpTimer = 0.0f;

	[SerializeField, Tooltip("Intermission duration.")]
	private float intermissionDuration = 15.0f;

	[SerializeField, Tooltip("Intermission timer.")]
	private float intermissionTimer = 0.0f;

	[SerializeField, Tooltip("Current game state.")]
	private GameState currentGameState = GameState.WarmUp;

	[SerializeField, Tooltip("Whether the game is paused.")]
	private bool isPaused = false;

	[Header("Events")]

	[SerializeField, Tooltip("Invoked when the lives value changes.")]
	public UnityEvent<int> onLivesChanged = new UnityEvent<int>();

	[SerializeField, Tooltip("Invoked when the current wave changes.")]
	public UnityEvent<int> onCurrentWaveChanged = new UnityEvent<int>();

	[SerializeField, Tooltip("Invoked when the game state changes.")]
	public UnityEvent<GameState> onGameStateChanged = new UnityEvent<GameState>();

	[SerializeField, Tooltip("Invoked when the game is paused/unpaused.")]
	public UnityEvent<bool> onPauseStateChanged = new UnityEvent<bool>();

	#endregion

	#region Member Declarations

	public int Lives
	{
		get { return lives; }
		set {
			lives = value;
			onLivesChanged.Invoke(value);
		}
	}
	public int CurrentWave
	{
		get { return currentWave; }
		set {
			currentWave = value;
			onCurrentWaveChanged.Invoke(value);
		}
	}
	public float WarmUpTimer { get { return warmUpTimer; } private set { warmUpTimer = value; } }
	public float IntermissionTimer { get { return intermissionTimer; } private set { intermissionTimer = value; } }
	public GameState CurrentGameState {
		get { return currentGameState; }
		private set
		{
			currentGameState = value;
			onGameStateChanged.Invoke(value);
		}
	}
	public bool IsPaused {
		get { return isPaused; }
		set 
		{
			isPaused = value;
			onPauseStateChanged.Invoke(value);
		}
	}

	#endregion

	#region Monobehaviour

	private void Start()
	{
		// Invoke the initial game state events
		Lives = Lives;
		CurrentWave = CurrentWave;
		CurrentGameState = CurrentGameState;
	}

	#endregion
}
