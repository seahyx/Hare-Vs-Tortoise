using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	#region Serializables

	[SerializeField]
	public List<CardBanner> banners = new List<CardBanner>();

	#endregion

	#region Member Declarations

	public TowerPlacementManager towerPlacementManager;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
		towerPlacementManager = GetComponent<TowerPlacementManager>();
	}

	#endregion

	public void SpawnCard(CardDataScriptableObject cardData)
	{

	}

	public void ActivateCard(CardBanner cardBanner)
	{

	}

	public void DeactivateCard(CardBanner cardBanner)
	{
		
	}

	public void ConsumeCard(CardBanner cardBanner)
	{
		if (cardBanner == null) return;
		banners.Remove(cardBanner);
	}
}
