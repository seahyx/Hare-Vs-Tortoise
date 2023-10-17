using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float radius = 1f; // Collision radius.
    float radiusSq; // Radius squared; optimisation.
    Transform target; // Who we are homing at.

    void OnEnable()
    {
        // Pre-compute the value. 
        radiusSq = radius * radius;
    }


    #region Serializables

    [SerializeField, Range(10.0f, 100.0f), Tooltip("Speed of bullet travel")]
    public float speed = 10.0f;
    [SerializeField, Range(0.0f, 100.0f), Tooltip("Damage of bullet")]
    public float damage = 10.0f;
    [SerializeField, Range(0.0f, 100.0f), Tooltip("Range of Attack")]
    public float range = 10.0f;
    [SerializeField ,Range(0.0f, 10.0f), Tooltip("How long will the bullet last for if it does not hit target")]
    public float bulletLifespan = 1.0f;

    public Vector2 direction = Vector2.zero;

    #endregion
    //private void Start()
    //{
    //    bulletLifespan = range / speed * 1.5f;
    //}

    void Update()
    {
        // make a storage variable that remembers the previous updates direction
        //this is to ensure that if there is no target and the bullet has no original trajectory it will be destroyed
        if (!target && direction == Vector2.zero)
        {
            Destroy(gameObject);
            return;
        }
        if (!target && direction != Vector2.zero)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
            return;
        }
        //Move Bullet towards target at every frame
        //calculate the direction vector between the tower and the enemy at every frame
        direction = (Vector2)transform.position - (Vector2)target.position;
        //the bullet position is adjusted
        transform.Translate(direction.normalized * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);


        //Destroy the bullets if too close too the target
        if (direction.sqrMagnitude < radiusSq)
        {
            Destroy(gameObject);
            //splat code add here
            //deal damage code add here
        }

        //if (Time.deltaTime < bulletLifespan) 
        //{
        //    transform.Translate(direction * speed * Time.deltaTime);
        //    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        //}

        else
        {
            Destroy(gameObject);
        }
    }
    // So that other scripts can use Projectile.Spawn to spawn a projectile.
    public static Bullet Spawn(GameObject prefab, Vector2 position, Quaternion rotation, Transform target)
    {
        // Spawn a GameObject based on a prefab, and returns its Projectile component.
        GameObject go = Instantiate(prefab, position, rotation);
        Bullet p = go.GetComponent<Bullet>();

        // Rightfully, we should throw an error here instead of fixing the error for the user. 
        if (!p) p = go.AddComponent<Bullet>();

        // Set the projectile's target, so that it can work.
        p.target = target;
        return p;
    }
}
