using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class BaseTower : MonoBehaviour
{
	#region Serializables

    [SerializeField, Tooltip("Damage of the tower per hit")]
    public float Damage = 1.0f;

    [SerializeField, Tooltip("Number of enemies it can go through")]
    public float Pierce = 1.0f;

    [SerializeField, Tooltip("Interval between attacks in seconds")]
    public float FireRate = 1.0f;

    [SerializeField, Tooltip("Radius of detecting enemies in px")]
    public float Range = 100.0f;

    [SerializeField, Tooltip("Speed of projectile in px per second")]
    public float ProjectileSpeed = 100.0f;

    [SerializeField, Tooltip("Height and width of tower in px")]
    public float Footprint = 20.0f;

    #endregion

    public static float pixelsPerUnit { get; private set; } = 100.0f;
    private float lastAttackTime;
    private Transform target;

    protected void OnEnable()
    {
        lastAttackTime = Time.time;
    }

    protected void Update()
    {
        Attack();
    }


    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range / pixelsPerUnit);
    }

    private void Attack()
    {
        if (target != null && Time.time - lastAttackTime >= FireRate)
        {
            // Create and launch a projectile toward the target.
            //LaunchProjectile(target);

            // Reset the attack timer.
            lastAttackTime = Time.time;
        }
    }

    private void LaunchProjectile()
    {
    }
}