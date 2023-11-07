using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	#region Serializables

	[SerializeField]
	private int maxCards = 4;

	[SerializeField]
	public List<CardBanner> banners = new List<CardBanner>();

	[SerializeField]
	private Transform cardDisplayParent;

	[SerializeField]
	private Transform cardBannerParent;

	[SerializeField, Tooltip("Height offset from the card banner parent object to spawn the card banner.")]
	private float cardSpawnHeightOffset = 3;

	#endregion

	#region Member Declarations

	[HideInInspector]
	public TowerPlacementManager towerPlacementManager;

	private CardBanner currentActivatedCardBanner;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
		towerPlacementManager = GetComponent<TowerPlacementManager>();
	}

	#endregion

	public void SpawnCard(CardDataScriptableObject cardData)
	{
		if (banners.Count > maxCards)
		{
			Debug.Log("Max number of cards in the conveyor reached. Discard incoming card.");
			return;
		}

		Card card = Instantiate(cardData.Card, cardDisplayParent);
		card.CardData = cardData;
		card.UpdateCardData();
		card.gameObject.SetActive(false);

		CardBanner banner = Instantiate(cardData.Banner, cardBannerParent);
		banner.transform.Translate(0, cardSpawnHeightOffset, 0);
		banner.Card = card;
		banner.CardData = cardData;
		banner.CardManager = this;

		banners.Add(banner);
	}

	public void ActivateCard(CardBanner cardBanner)
	{
		if (currentActivatedCardBanner != null)
		{
			currentActivatedCardBanner.ActivateCancel();
		}
		currentActivatedCardBanner = cardBanner;
	}

	public void DeactivateCard(CardBanner cardBanner)
	{
		currentActivatedCardBanner = null;
	}

	public void ConsumeCard(CardBanner cardBanner)
	{
		if (cardBanner == null) return;
		currentActivatedCardBanner = null;
		banners.Remove(cardBanner);
	}
}
