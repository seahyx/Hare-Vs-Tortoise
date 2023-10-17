using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Serializables

    [SerializeField, Range(10.0f, 100.0f), Tooltip("Speed of bullet travel")]
    public float speed = 10.0f;
    [SerializeField, Range(0.0f, 100.0f), Tooltip("Damage of bullet")]
    public float damage = 10.0f;
    [SerializeField, Range(0.0f, 100.0f), Tooltip("Damage of bullet")]
    public float range = 10.0f;

    public float bulletLifespan = 1.0f;
    public Vector2 direction = Vector2.zero;

    #endregion
    private void Start()
    {
        bulletLifespan = range / speed * 1.5f;

    }
    void Update()
    {
        if (Time.deltaTime < bulletLifespan) 
        {
            transform.Translate(direction * speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
