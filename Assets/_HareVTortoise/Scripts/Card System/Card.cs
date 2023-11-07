using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	#region Serializables

	[SerializeField]
	private Image cardImage;

	[SerializeField]
	private TextMeshProUGUI cardName;

	[SerializeField]
	private TextMeshProUGUI cardDesc;

	[SerializeField]
	public CardDataScriptableObject CardData;

	#endregion

	private void OnEnable()
	{
		UpdateCardData();
	}

	public virtual void UpdateCardData()
	{
		if (CardData == null) return;
		cardImage.sprite = CardData.CardSprite;
		cardName.text = CardData.CardName;
		cardDesc.text = CardData.Description;
	}
}
