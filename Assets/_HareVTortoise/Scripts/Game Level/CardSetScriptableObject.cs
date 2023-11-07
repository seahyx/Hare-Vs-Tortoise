using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Set", menuName = "Hare V Tortoise/Card Set", order = 2)]
public class CardSetScriptableObject : ScriptableObject
{
	#region Serializables

	[SerializeField, Tooltip("Main card object.")]
	public Card card;

	[SerializeField, Tooltip("Associated card banner object.")]
	public CardBanner banner;

	#endregion
}
