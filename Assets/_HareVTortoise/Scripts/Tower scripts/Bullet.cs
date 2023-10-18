using UnityEngine;

public class Bullet : MonoBehaviour
{
	#region Serializables

	[SerializeField, Range(10.0f, 100.0f), Tooltip("Speed of bullet travel")]
	public float Speed = 10.0f;
	[SerializeField, Range(0.0f, 100.0f), Tooltip("Damage of bullet")]
	public float Damage = 10.0f;
	[SerializeField, Range(0.0f, 100.0f), Tooltip("Range of Attack")]
	public float Range = 10.0f;
	[SerializeField ,Range(0.0f, 10.0f), Tooltip("How long will the bullet last for if it does not hit target")]
	public float Lifespan = 1.0f;

	[SerializeField, Tooltip("Bullet raycast collision layer mask.")]
	public LayerMask CollisionLayerMask = 0;

	#endregion

	#region Member Declarations

	private float startTime = 0.0f;
	private float lifetime => Time.time - startTime;

	private Vector3 lastPosition = Vector3.zero;

	#endregion

	#region Monobehaviour

	private void Start()
	{
		startTime = Time.time;
		lastPosition = transform.position;
	}

	void Update()
	{
		transform.Translate(Vector3.up * Speed * Time.deltaTime);

		// Check collision
		ContactFilter2D filter2D = new();
		filter2D.layerMask = CollisionLayerMask;
		filter2D.useLayerMask = true;

		RaycastHit2D[] hits = new RaycastHit2D[1];

		if (Physics2D.Linecast(lastPosition, transform.position, filter2D, hits) > 0)
		{
			// Has collision!
			// Attempt to get the BaseEnemy component
			GameObject collidedGameObj = hits[0].collider.attachedRigidbody.gameObject;
			BaseEnemy enemy;
			if (collidedGameObj.TryGetComponent(out enemy))
			{
				// Manage to hit enemy! Time to die.
				enemy.DealDamage(Damage, "Boolet");
				Destroy(gameObject);
			}
		}

		if (lifetime > Lifespan)
		{
			Destroy(gameObject);
		}
	}

	#endregion

	public static Bullet Spawn(Bullet bulletPrefab, Vector2 position, Vector2 direction)
	{
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
		return Instantiate(bulletPrefab, position, rotation);
	}
}
