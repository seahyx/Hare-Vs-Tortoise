using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class BaseEnemy : MonoBehaviour
{
	#region Serializables

	[SerializeField, Tooltip("Spline container object containing the path the enemy will follow in the map.")]
	public SplineContainer EnemyPath;

	[SerializeField, Tooltip("Movement speed of the enemy in units per second.")]
	public float MoveSpeed = 10.0f;

	#endregion

	#region Member Declarations

	[Header("Debug Values")]
	[SerializeField]
	private float progress = 0.0f;
	[SerializeField]
	private float3 position;
	[SerializeField]
	private float3 tangent;
	[SerializeField]
	private float3 upVector;

	#endregion

	#region Monobehaviour

	protected void OnEnable()
	{
		if (EnemyPath == null)
		{
			Debug.LogError($"Enemy does not have an enemyPath spline assigned.");
			gameObject.SetActive(false);
		}
	}

	protected void Update()
	{
		// Get current position, facing rotation, and up vector (comes with the package)
		EnemyPath.Evaluate(progress, out position, out tangent, out upVector);
		tangent = Vector3.Normalize(tangent);

		// Set position and rotations
		transform.position = position;
		transform.rotation = Quaternion.LookRotation(Vector3.forward, tangent);

		// Move forwards
		progress += MoveSpeed / EnemyPath.CalculateLength() * Time.deltaTime;
		if (progress > 1.0f) progress = 0.0f;

		// Debug
		Debug.DrawRay(transform.position, tangent, Color.red);
	}

	#endregion

	#region Helper Functions

	#endregion

}
