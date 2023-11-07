using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	#region Serializables

	[SerializeField]
	public List<CardBanner> banners = new List<CardBanner>();

	[SerializeField]
	public List<Card> cards = new List<Card>();

	#endregion

	#region Member Declarations

	private TowerPlacementManager towerPlacementManager;

	#endregion

	#region Monobehaviour

	private void Start()
	{
		towerPlacementManager = GetComponent<TowerPlacementManager>();
	}

	#endregion
}
