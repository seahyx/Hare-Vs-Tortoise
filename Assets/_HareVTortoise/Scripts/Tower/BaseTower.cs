using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
	public const float PIXELS_PER_UNIT = 100.0f;

	#region Serializables

	[SerializeField, Tooltip("Damage of the tower per hit.")]
	public float Damage = 1.0f;

	[SerializeField, Tooltip("Number of enemies it can go through.")]
	public float Pierce = 1.0f;

	[SerializeField, Tooltip("Attacks per second.")]
	public float FireRate = 1.0f;

	[SerializeField, Tooltip("Radius of detecting enemies in px.")]
	public float Range = 100.0f;

	[SerializeField, Tooltip("Speed of projectile in px per second.")]
	public float ProjectileSpeed = 100.0f;

	[SerializeField, Tooltip("Height and width of tower in px.")]
	public float Footprint = 20.0f;

	[SerializeField, Tooltip("Surfaces that this tower can be built on.")]
	public TowerPlacementManager.Surfaces PlaceableSurfaces = TowerPlacementManager.Surfaces.Land;

	[SerializeField, Tooltip("Bullet prefab.")]
	public Bullet BulletPrefab;

	[SerializeField]
	public SpriteRenderer TowerSprite;

	[SerializeField]
	public Transform RangeTransform;

	[SerializeField]
	public TowerRangeTriggerHandler RangeTriggerHandler;

	[Header("Others")]

	[SerializeField, Tooltip("Detected enemies in range.")]
	public List<BaseEnemy> EnemiesInRange = new List<BaseEnemy>();

	#endregion

	#region Member Declarations

	public float AttackCooldown => 1 / FireRate;

	protected float attackCooldownTimer = 0.0f;

	public BaseEnemy target => EnemiesInRange.Count > 0 ? EnemiesInRange[0] : null;

	/// <summary>
	/// Range of the tower in Unity units.
	/// </summary>
	public float RangeUnityUnit => Range / PIXELS_PER_UNIT;

	#endregion

	#region Monobehaviour

	protected void Awake()
	{
		RangeTriggerHandler.tower = this;
	}

	protected void Update()
	{
		UpdateRange();
		if (target != null)
		{
			Vector2 directionToEnemy = target.transform.position - transform.position;
			directionToEnemy.Normalize();
			Quaternion rotation = Quaternion.LookRotation(Vector3.forward, directionToEnemy);
			transform.rotation = rotation;

			if (attackCooldownTimer <= 0.0f)
			{
				Attack();
				attackCooldownTimer = AttackCooldown;
			}
		}

		if (attackCooldownTimer > 0.0f)
		{
			attackCooldownTimer -= Time.deltaTime;
		}
	}
	
	private void OnDrawGizmosSelected()
	{
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, Range / PIXELS_PER_UNIT);
	}

	#endregion

	#region Attacking Functions

	private void Attack()
	{
		// Create and launch a projectile toward the target.
		if (target != null)
		{
			Vector2 directionToEnemy = target.transform.position - transform.position;
			directionToEnemy.Normalize();
			LaunchProjectile(10.0f, Damage, Range, directionToEnemy);
		}
	}

	private void LaunchProjectile(float speed, float damage, float range, Vector2 direction)
	{
		Bullet bullet = Bullet.Spawn(BulletPrefab, transform.position, direction);
		bullet.Speed = speed;
		bullet.Damage = damage;
		bullet.Range = range;
	}

	public void OnEnemyEnterRange(Collider2D other)
	{
		BaseEnemy enemy;
		if (other.attachedRigidbody.gameObject.TryGetComponent(out enemy))
		{
			enemy.OnDestroyedEvent.AddListener(OnEnemyDestroyed);
		}
		EnemiesInRange.Add(enemy);
	}

	public void OnEnemyExitRange(Collider2D other)
	{
		BaseEnemy enemy;
		if (other.attachedRigidbody.gameObject.TryGetComponent(out enemy))
		{
			enemy.OnDestroyedEvent.RemoveListener(OnEnemyDestroyed);
		}
		EnemiesInRange.Remove(enemy);
	}


	#endregion

	#region Helper Functions

	private void OnEnemyDestroyed(BaseEnemy enemy, BaseEnemy.DeathReason reason)
	{
		EnemiesInRange.Remove(enemy);
	}

	private void UpdateRange()
	{
		if (RangeTriggerHandler != null)
		{
			RangeTriggerHandler.UpdateRange(RangeUnityUnit);
		}

		if (RangeTransform == null) return;
		RangeTransform.localScale = new Vector3(RangeUnityUnit, RangeUnityUnit, 1);
	}

	private void ShowRange(bool show)
	{
		UpdateRange();
		RangeTransform.gameObject.SetActive(show);
	}

	#endregion
}