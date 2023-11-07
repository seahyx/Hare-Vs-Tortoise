using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
	#region Serializables

	[SerializeField]
	private SpriteRenderer cardImage;

	[SerializeField]
	private TextMeshPro cardName;

	[SerializeField]
	private TextMeshPro cardDesc;

	[SerializeField]
	private CardDataScriptableObject cardData;

	#endregion

	private void OnEnable()
	{
		UpdateCardData();
	}

	protected void UpdateCardData()
	{
		if (cardData == null) return;
		cardImage.sprite = cardData.CardSprite;
		cardName.text = cardData.CardName;
		cardDesc.text = cardData.Description;
	}
}
