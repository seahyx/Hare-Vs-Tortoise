using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

	public class WinData
	{
		public int lives;
		public WinData(int lives)
		{
			this.lives = lives;
		}
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

	[Tooltip("Whether the player has lost the game.")]
	public bool hasLost = false;

	[Tooltip("Whetehr the player has won the game.")]
	public bool hasWon = false;

	[Header("Events")]

	[SerializeField, Tooltip("Invoked when the lives value changes.")]
	public UnityEvent<int> OnLivesChanged = new UnityEvent<int>();

	[SerializeField, Tooltip("Invoked when the game state changes.")]
	public UnityEvent<GameState> OnGameStateChanged = new UnityEvent<GameState>();

	[SerializeField, Tooltip("Invoked when the game is paused/unpaused.")]
	public UnityEvent<bool> OnPauseStateChanged = new UnityEvent<bool>();

	[SerializeField, Tooltip("Invoked when the player loses.")]
	public UnityEvent OnLose = new UnityEvent();

	[SerializeField, Tooltip("Invoked when the player has won the game.")]
	public UnityEvent<WinData> OnWin = new UnityEvent<WinData>();

	#endregion

	#region Member Declarations

	public int Lives
	{
		get { return lives; }
		set {
			lives = value;
			OnLivesChanged.Invoke(value);
			if (lives <= 0) AllLivesLost();
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

	#region Helper Functions

	private void AllLivesLost()
	{
		// Only lose if the player has not already won.
		if (!hasWon)
		{
			hasLost = true;
			currentGameState = GameState.End;
			OnLose.Invoke();
		}
	}

	public void AllWavesEnded()
	{
		// Only win if the player has not already lost.
		if (!hasLost)
		{
			hasWon = true;
			currentGameState = GameState.End;
			OnWin.Invoke(new WinData(Lives));
		}
	}

	#endregion
}
