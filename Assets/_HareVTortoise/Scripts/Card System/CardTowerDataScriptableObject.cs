using UnityEngine;

[CreateAssetMenu(fileName = "Card Tower Data", menuName = "Hare V Tortoise/Card Tower Data", order = 2)]
[System.Serializable]
public class CardTowerDataScriptableObject : CardDataScriptableObject
{
    [SerializeField, Tooltip("Tower prefab.")]
    public BaseTower Tower;
}
