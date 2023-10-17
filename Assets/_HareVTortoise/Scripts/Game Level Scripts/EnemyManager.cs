using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyManager : MonoBehaviour
{
	#region Serializables

	[SerializeField, Tooltip("The enemy path spline container.")]
	public SplineContainer EnemyPath;

	[SerializeField, Tooltip("Current alive enemy list")]
	public List<BaseEnemy> CurrentEnemies = new List<BaseEnemy>();



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
		BaseEnemy enemy = Instantiate(enemyPrefab, position, rotation);
		enemy.EnemyPath = EnemyPath;

		// Add to tracking list
		CurrentEnemies.Add(enemy);
	}

	#endregion
}
