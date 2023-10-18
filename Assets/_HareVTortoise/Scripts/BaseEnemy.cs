using NaughtyAttributes;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class BaseEnemy : MonoBehaviour
{
	#region Type Definitions

	public enum DeathReason
	{
		ReachGoal,
		KilledByRabbit,
		SelfHarm
	}

	#endregion

	#region Serializables

	[SerializeField, Tooltip("Spline container object containing the path the enemy will follow in the map.")]
	public SplineContainer EnemyPath;

	[SerializeField, Tooltip("Progress along the enemy path. [0...1]")]
	public float Progress = 0.0f;

	[SerializeField,Range(0.0f,20.0f), Tooltip("Movement speed of the enemy in units per second.")]
	private float moveSpeed = 10.0f;

	[SerializeField,Range(10.0f,100.0f),Tooltip("Health of the Enemy Unit")]
	private float health = 10.0f;

	[SerializeField, Range(10.0f, 50.0f), Tooltip("Height and Width of the Enemy Unit")]
	private float footprint = 10.0f;

	[Header("Events")]

	[SerializeField, Tooltip("Invoked when the enemy is healed. Passes itself and healing received.")]
	public UnityEvent<BaseEnemy, float> OnHealEvent = new UnityEvent<BaseEnemy, float>();

	[SerializeField, Tooltip("Invoked when the enemy is damaged. Passes itself and damage received (positive value).")]
	public UnityEvent<BaseEnemy, float> OnDamageEvent = new UnityEvent<BaseEnemy, float>();

	[SerializeField, Tooltip("Invoked when this enemy is destroyed. Passes itself and death reason as parameters.")]
	public UnityEvent<BaseEnemy, DeathReason> OnDestroyedEvent = new UnityEvent<BaseEnemy, DeathReason>();

	#endregion

	#region Member Declarations

	public float MoveSpeed
	{
		get { return moveSpeed; }
		set { moveSpeed = value; }
	}

	public float Health
	{
		get { return health; }
		protected set { health = value; }
	}

	public float Footprint
	{
		get { return footprint; }
		set { footprint = value; }
	}

	[SerializeField, ReadOnly, Foldout("Debug")]
	protected float3 position;
	[SerializeField, ReadOnly, Foldout("Debug")]
	protected float3 tangent;
	[SerializeField, ReadOnly, Foldout("Debug")]
	protected float3 upVector;

	#endregion

	#region Monobehaviour

	protected void Update()
	{
		// Get current position, facing rotation, and up vector (comes with the package)
		EnemyPath.Evaluate(Progress, out position, out tangent, out upVector);
		tangent = Vector3.Normalize(tangent);

		// Set position and rotations
		transform.position = position;
		transform.rotation = Quaternion.LookRotation(Vector3.forward, tangent);

		// Move forwards
		Progress += MoveSpeed / EnemyPath.CalculateLength() * Time.deltaTime;
		if (Progress > 1.0f)
		{
			// Kill thyself once reach end!!!
			DestroyWithReason(DeathReason.ReachGoal);
		}

		// Debug
		Debug.DrawRay(transform.position, tangent, Color.red);
	}

	#endregion

	#region Stats Events

	public void Heal(float heal)
	{
		Health += heal;
		OnHealEvent.Invoke(this, heal);
	}

	public void DealDamage(float damage, string damageSource)
	{
		Health -= damage;
		if (Health <= 0)
		{
			DestroyWithReason(DeathReason.KilledByRabbit);
		}
		OnDamageEvent.Invoke(this, damage);
	}

	#endregion

	#region Helper Functions

	public static BaseEnemy Spawn(BaseEnemy enemyPrefab, Vector2 position, Quaternion rotation, SplineContainer enemyPath)
	{
		BaseEnemy enemy = Instantiate(enemyPrefab, position, rotation);
		enemy.EnemyPath = enemyPath;
		return enemy;
	}

	public void DestroyWithReason(DeathReason reason)
	{
		OnDestroyedEvent.Invoke(this, reason);
		Destroy(gameObject);
	}

	#endregion

}
