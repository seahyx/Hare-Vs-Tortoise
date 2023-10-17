using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyManager))]
public class WaveManager : MonoBehaviour, IPausable
{
	#region Serializable

	[SerializeField, Tooltip("Wave BP to determing spawning sequence.")]
	public WaveBPScriptableObject WaveBP;

	[SerializeField, Tooltip("Whether the wave spawning is paused.")]
	private bool isPaused = false;

	#endregion

	#region Member Declarations

	public bool IsPaused
	{
		get { return isPaused; }
		set { isPaused = value; }
	}

	[HideInInspector]
	public EnemyManager EnemyManager;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
		EnemyManager = GetComponent<EnemyManager>();
	}

	#endregion
}
