using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDestructionDelegate : MonoBehaviour
{
    public delegate void EnemyDelegate(GameObject enemy);

    public EnemyDelegate enemyDelegate;

    public UnityEvent<GameObject> onDieEvent = new UnityEvent<GameObject>();

    void OnDestroy()
    {
        if (enemyDelegate != null)
        {
            enemyDelegate(gameObject);
            onDieEvent.Invoke(gameObject);
        }
    }
}
