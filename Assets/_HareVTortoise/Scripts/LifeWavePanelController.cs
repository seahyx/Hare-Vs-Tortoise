using NaughtyAttributes;
using UnityEngine;

public class LifeWavePanelController : MonoBehaviour
{
	#region Serializables

	[SerializeField, Tooltip("GameLevelManager reference.")]
	[Required]
	private GameLevelManager gameLevelManager;

	[SerializeField, Tooltip("WaveManager reference.")]
	[Required]
	private WaveManager waveManager;

	[SerializeField, Tooltip("Lives value reference.")]
	private LayeredText livesValue;

	[SerializeField, Tooltip("Current wave value reference.")]
	private LayeredText waveValue;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
		gameLevelManager.OnLivesChanged.AddListener((lives) =>
		{
			livesValue.Text = lives.ToString();
		});
		waveManager.OnCurrentWaveChanged.AddListener((currentWave, totalWaves) =>
		{
			waveValue.Text = $"{currentWave + 1}";
		});
	}

	#endregion
}
