using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class WaveBPItem : ISerializationCallbackReceiver
{
	#region Type Definitions

	public enum ItemType
	{
		SpawnEnemy,
		Delay,
		WaveBP
	}

	#endregion

	[SerializeField, HideInInspector]
	public string name;

	#region Serializables

	[SerializeField, Tooltip("Determines the function of this wave item.\n" +
		"SpawnEnemy - Spawns enemy prefabs with pre/post-delay.\n" +
		"WaveBP - Loads in another wave blueprint in this slot.")]
	public ItemType Type;

	#region Type = SpawnEnemy

	[SerializeField, Tooltip("Delay before spawning the enemy in seconds.")]
	[MinValue(0)]
	[ShowIf("Type", ItemType.SpawnEnemy)]
	[AllowNesting]
	public float PreDelay = 0.0f;

	[SerializeField, Tooltip("Delay after spawning the enemy in seconds.")]
	[ShowIf("Type", ItemType.SpawnEnemy)]
	[AllowNesting]
	public float PostDelay = 0.0f;

	[SerializeField, Tooltip("Enemy prefab to spawn.")]
	[ShowIf("Type", ItemType.SpawnEnemy)]
	[AllowNesting]
	public BaseEnemy EnemyPrefab;

	[SerializeField, Tooltip("How many copies of the enemy to spawn.")]
	[MinValue(1)]
	[ShowIf("Type", ItemType.SpawnEnemy)]
	[AllowNesting]
	public int NumberOfEnemies = 1;

	[SerializeField, Tooltip("How much delay between spawning each enemy in seconds.")]
	[MinValue(0)]
	[ShowIf("Type", ItemType.SpawnEnemy)]
	[AllowNesting]
	public float SpawnDelay = 0.0f;

	[SerializeField, Tooltip("If set to true, the wave system will immediately " +
		"begin the post-delay timer after the first enemy is spawned, without " +
		"waiting for all the enemies to finish spawning. This is useful for " +
		"stacking multiple different enemy spawns at the same time.\n\n" +
		"If set to false, the wave system will wait for the last enemy to be" +
		"spawned before starting the post-delay timer.")]
	[ShowIf("Type", ItemType.SpawnEnemy)]
	[AllowNesting]
	public bool DoNotWaitForSpawn = false;

	#endregion

	#region Type = Delay

	[SerializeField, Tooltip("Delay duration in seconds.")]
	[MinValue(0)]
	[ShowIf("Type", ItemType.Delay)]
	[AllowNesting]
	public float Delay = 0.0f;

	#endregion

	#region Type = WaveBP

	[SerializeField, Tooltip("A WaveBP asset to execute.")]
	[ShowIf("Type", ItemType.WaveBP)]
	[AllowNesting]
	public WaveBPScriptableObject WaveBP;

	#endregion

	#endregion

	#region ISerializationCallbackReceiver

	public void OnBeforeSerialize()
	{
		switch (Type)
		{
			case ItemType.SpawnEnemy:
				name = $"Spawn {NumberOfEnemies} x {EnemyPrefab?.name ?? "???"} - pre: {PreDelay}s/pos: {PostDelay}s/spw: {SpawnDelay}s{(DoNotWaitForSpawn ? "/No Wait" : "")}";
				break;
			case ItemType.Delay:
				name = $"Delay {Delay}s";
				break;
			case ItemType.WaveBP:
				name = $"Exec BP: {WaveBP?.name ?? "???"}";
				break;
		}
	}

	public void OnAfterDeserialize() { }

	#endregion

	#region Wave Execution Functions

	public IEnumerator Execute(WaveManager waveManager)
	{
		Debug.Log($"Executing wave: {name}");
		switch (Type)
		{
			case ItemType.SpawnEnemy:
				yield return waveManager.StartCoroutine(SpawnEnemyCoroutine(waveManager));
				break;

			case ItemType.Delay:
				yield return new WaitWhileUnpaused(Delay, waveManager);
				break;

			case ItemType.WaveBP:
				yield return waveManager.StartCoroutine(WaveBP.Execute(waveManager));
				break;
		}
	}

	public IEnumerator SpawnEnemyCoroutine(WaveManager waveManager)
	{
		// Pre-delay
		yield return new WaitWhileUnpaused(PreDelay, waveManager);

		// Enemy spawn loop
		int enemiesLeft = NumberOfEnemies - 1;
		waveManager.EnemyManager.SpawnEnemy(EnemyPrefab);

		if (enemiesLeft > 0)
		{
			Coroutine repeatSpawnEnemy = waveManager.StartCoroutine(RepeatSpawnEnemyCoroutine(waveManager, enemiesLeft));
			if (!DoNotWaitForSpawn)
			{
				yield return repeatSpawnEnemy;
			}
		}

		// Post-delay
		yield return new WaitWhileUnpaused(PostDelay, waveManager);
	}

	public IEnumerator RepeatSpawnEnemyCoroutine(WaveManager waveManager, int enemiesLeft)
	{
		while (enemiesLeft > 0)
		{
			yield return new WaitWhileUnpaused(SpawnDelay, waveManager);

			waveManager.EnemyManager.SpawnEnemy(EnemyPrefab);
			enemiesLeft--;
		}
	}

	#endregion
}