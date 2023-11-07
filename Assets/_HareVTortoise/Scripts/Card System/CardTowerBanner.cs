using UnityEngine;

public class CardTowerBanner : CardBanner
{
	#region Card Banner Event Overrides

	protected override void OnActivate()
	{
		base.OnActivate();

		if (CardData is CardTowerDataScriptableObject towerData)
		{
			cardManager.towerPlacementManager.StartPlaceTower(towerData.Tower);
		}
		else
		{
			Debug.LogWarning("Card data is not of the correct tower type.");
		}
	}

	protected override void OnActivateRelease()
	{
		base.OnActivateRelease();

		if (CardData is CardTowerDataScriptableObject towerData)
		{
			cardManager.towerPlacementManager.FinishPlaceTower();
		}
		else
		{
			Debug.LogWarning("Card data is not of the correct tower type.");
		}
	}

	protected override void OnActivateCancel()
	{
		base.OnActivateCancel();

		if (CardData is CardTowerDataScriptableObject towerData)
		{
			cardManager.towerPlacementManager.CancelPlaceTower();
		}
		else
		{
			Debug.LogWarning("Card data is not of the correct tower type.");
		}
	}

	protected override void OnConsumeCard()
	{
		base.OnConsumeCard();

		Destroy(Card.gameObject);
		Destroy(gameObject);
	}

	#endregion
}
