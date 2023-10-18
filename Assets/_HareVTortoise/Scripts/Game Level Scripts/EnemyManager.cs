using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(GameLevelManager))]
public class EnemyManager : MonoBehaviour
{
	#region Serializables

	[SerializeField, Tooltip("The enemy path spline container.")]
	public SplineContainer EnemyPath;

	[SerializeField, Tooltip("Current alive enemy list")]
	public List<BaseEnemy> CurrentEnemies = new List<BaseEnemy>();

	#endregion

	#region Member Declarations

	private GameLevelManager gameLevelManager;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
		gameLevelManager = GetComponent<GameLevelManager>();
	}

	#endregion

	#region Enemy Spawn Functions

	/// <summary>
	/// Spawns an enemy at the starting point of the map.
	/// </summary>
	public void SpawnEnemy(BaseEnemy enemyPrefab)
	{
		// Calculate initial starting position
		float3 position, tangent, upVector;
		EnemyPath.Evaluate(0, out position, out tangent, out upVector);
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, tangent);

		// Instantiate enemy object
		BaseEnemy enemy = BaseEnemy.Spawn(enemyPrefab, (Vector3)position, rotation, EnemyPath);
		enemy.OnDestroyedEvent.AddListener((enemy, reason) =>
		{
			CurrentEnemies.Remove(enemy);
			switch (reason)
			{
				case BaseEnemy.DeathReason.ReachGoal:
					gameLevelManager.Lives -= 1;
					break;
				case BaseEnemy.DeathReason.KilledByRabbit: break;
				case BaseEnemy.DeathReason.SelfHarm: break;
			}
		});

		// Add to tracking list
		CurrentEnemies.Add(enemy);
	}

	#endregion
}
