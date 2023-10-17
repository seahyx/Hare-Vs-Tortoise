using NaughtyAttributes;
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

	[SerializeField,Range(10.0f,120.0f), Tooltip("Movement speed of the enemy in units per second.")]
	public float MoveSpeed = 10.0f;

	[SerializeField,Range(10.0f,100.0f),Tooltip("Health of the Enemy Unit")]
	public float Health = 10.0f;

	[SerializeField, Range(10.0f, 50.0f), Tooltip("Height and Width of the Enemy Unit")]
	public float Footprint = (10.0f);
	#endregion

	#region Member Declarations

	[SerializeField, ReadOnly, Foldout("Debug")]
	private float progress = 0.0f;
	[SerializeField, ReadOnly, Foldout("Debug")]
	private float3 position;
	[SerializeField, ReadOnly, Foldout("Debug")]
	private float3 tangent;
	[SerializeField, ReadOnly, Foldout("Debug")]
	private float3 upVector;

	#endregion

	#region Monobehaviour

	protected void OnEnable()
	{
		//if (EnemyPath == null)
		//{
		//	Debug.LogWarning($"Enemy does not have an enemyPath spline assigned.");
		//	gameObject.SetActive(false);
		//}
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
		if (progress > 1.0f)
		{
			// Kill thyself once reach end!!!
			Destroy(gameObject);
		}

		// Debug
		Debug.DrawRay(transform.position, tangent, Color.red);
	}

	#endregion

	#region Helper Functions

	#endregion

}
