using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeTriggerHandler : MonoBehaviour
{
	[HideInInspector]
	public BaseTower tower;
	protected CircleCollider2D circleCollider;

	#region Monobehaviour

	protected void Awake()
	{
		circleCollider = GetComponent<CircleCollider2D>();
	}

	protected void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.tag.Equals("Enemy")) return;

		tower.OnEnemyEnterRange(other);
	}

	protected void OnTriggerExit2D(Collider2D other)
	{
		if (!other.gameObject.tag.Equals("Enemy")) return;

		tower.OnEnemyExitRange(other);
	}

	#endregion

	/// <summary>
	/// Update the range trigger colliders and sprite renderer display.
	/// </summary>
	/// <param name="radius">Range radius in Unity units.</param>
	public void UpdateRange(float radius)
	{
		if (circleCollider == null) return;
		circleCollider.radius = radius;
	}
}
