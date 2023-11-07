using UnityEngine;

[System.Serializable]
public class CardDataScriptableObject : ScriptableObject
{
	[SerializeField]
	public string CardName = "Tower Name";

	[SerializeField]
	public string Description = "Tower Description";

	[SerializeField]
	public int Cost = 1;

	[SerializeField]
	public Sprite CardSprite;

	[SerializeField, Tooltip("Associated card banner prefab.")]
	public CardBanner Banner;

	[SerializeField, Tooltip("Card display prefab.")]
	public Card Card;
}
