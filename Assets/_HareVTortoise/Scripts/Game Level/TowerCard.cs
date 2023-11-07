using UnityEngine;

public class TowerCard : Card
{
	#region Serializables

	[SerializeField, Tooltip("Tower prefab reference.")]
	public BaseTower tower;

	#endregion

	protected override void OnActivate()
	{

	}
}
