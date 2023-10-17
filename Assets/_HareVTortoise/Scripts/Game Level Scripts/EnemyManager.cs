using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	#region Serializables

	[SerializeField, Tooltip("Enemy wave table.")]
	public GameObject WaveTable;

	[SerializeField, Tooltip("Current alive enemy list")]
	public List<BaseEnemy> CurrentEnemies = new List<BaseEnemy>();

	#endregion
}
