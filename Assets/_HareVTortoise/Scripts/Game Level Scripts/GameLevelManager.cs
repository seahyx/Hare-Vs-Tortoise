using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLevelManager : MonoBehaviour, IPausable
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
	private int lives = 10;

	[SerializeField, Tooltip("Warm up duration.")]
	private float warmUpDuration = 15.0f;

	[SerializeField, Tooltip("Warm up timer.")]
	private float warmUpTimer = 0.0f;

	[SerializeField, Tooltip("Intermission duration.")]
	private float intermissionDuration = 15.0f;

	[SerializeField, Tooltip("Intermission timer.")]
	private float intermissionTimer = 0.0f;

	[Tooltip("Current game state.")]
	private GameState currentGameState = GameState.WarmUp;

	[Tooltip("Whether the game is paused.")]
	private bool isPaused = false;

	[Header("Events")]

	[SerializeField, Tooltip("Invoked when the lives value changes.")]
	public UnityEvent<int> OnLivesChanged = new UnityEvent<int>();

	[SerializeField, Tooltip("Invoked when the game state changes.")]
	public UnityEvent<GameState> OnGameStateChanged = new UnityEvent<GameState>();

	[SerializeField, Tooltip("Invoked when the game is paused/unpaused.")]
	public UnityEvent<bool> OnPauseStateChanged = new UnityEvent<bool>();

	#endregion

	#region Member Declarations

	public int Lives
	{
		get { return lives; }
		set {
			lives = value;
			OnLivesChanged.Invoke(value);
		}
	}
	public float WarmUpTimer { get { return warmUpTimer; } private set { warmUpTimer = value; } }
	public float IntermissionTimer { get { return intermissionTimer; } private set { intermissionTimer = value; } }
	public GameState CurrentGameState {
		get { return currentGameState; }
		private set
		{
			currentGameState = value;
			OnGameStateChanged.Invoke(value);
		}
	}
	public bool IsPaused {
		get { return isPaused; }
		set 
		{
			isPaused = value;
			OnPauseStateChanged.Invoke(value);
		}
	}

	#endregion

	#region Monobehaviour

	private void Start()
	{
		// Invoke the initial game state events
		Lives = Lives;
		CurrentGameState = CurrentGameState;
	}

	#endregion
}
