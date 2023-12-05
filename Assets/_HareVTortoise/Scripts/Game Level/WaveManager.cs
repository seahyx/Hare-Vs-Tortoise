using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyManager), typeof(GameLevelManager))]
public class WaveManager : MonoBehaviour, IPausable
{
	#region Serializable

	[SerializeField, Tooltip("Wave BP to determing spawning sequence.")]
	public WaveBPScriptableObject WaveBP;

	[SerializeField, Tooltip("Whether the wave spawning is paused.")]
	private bool isPaused = false;

	[SerializeField, Tooltip("Current wave number.")]
	private int currentWave = 0;

	[Header("Events")]

	[SerializeField, Tooltip("Invoked when the current wave changes. Passes the current wave number and total waves, respectively.")]
	public UnityEvent<int, int> OnCurrentWaveChanged = new UnityEvent<int, int>();

	[SerializeField, Tooltip("Invoked when all the waves have concluded.")]
	public UnityEvent OnAllWavesEnded = new UnityEvent();

	#endregion

	#region Member Declarations

	public bool IsPaused
	{
		get { return isPaused; }
		set { isPaused = value; }
	}
	public int CurrentWave
	{
		get { return currentWave; }
		set
		{
			currentWave = value;
			OnCurrentWaveChanged.Invoke(value, TotalWaves);
		}
	}

	public int TotalWaves => WaveBP?.CountTotalWaves() ?? 0;

	private GameLevelManager gameLevelManager;

	[HideInInspector]
	public EnemyManager EnemyManager;

	[HideInInspector]
	public CardManager CardManager;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
		gameLevelManager = GetComponent<GameLevelManager>();
		EnemyManager = GetComponent<EnemyManager>();
		CardManager = GetComponent<CardManager>();
	}

	private void Start()
	{
		// Invoke the initial game state events
		CurrentWave = CurrentWave;
	}

	#endregion

	[Button]
	public void StartWave()
	{
		StartCoroutine(WaveBP.Execute(this, true));
	}

	[Button]
	public void EndWaves()
	{
		StopAllCoroutines();
	}

	public void AllWavesEnded()
	{
		OnAllWavesEnded.Invoke();
		gameLevelManager.AllWavesEnded();
	}
}
