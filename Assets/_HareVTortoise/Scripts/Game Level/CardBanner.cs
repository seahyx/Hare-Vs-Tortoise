using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBanner : MonoBehaviour
{
	#region Serializables

	[SerializeField, ReadOnly, Tooltip("Card reference.")]
	public Card card;

	public EventTrigger trigger;

	#endregion

	private void Start()
	{
		trigger = GetComponent<EventTrigger>();
		EventTrigger.Entry clickEntry = new EventTrigger.Entry();
		clickEntry.eventID = EventTriggerType.PointerClick;
		clickEntry.callback.AddListener((data) => Click());
		trigger.triggers.Add(clickEntry);

		EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
		beginDragEntry.eventID = EventTriggerType.BeginDrag;
		beginDragEntry.callback.AddListener((data) => BeginDrag((PointerEventData)data));
		trigger.triggers.Add(beginDragEntry);

		EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
		endDragEntry.eventID = EventTriggerType.EndDrag;
		endDragEntry.callback.AddListener((data) => EndDrag((PointerEventData)data));
		trigger.triggers.Add(endDragEntry);
	}

	public void ToggleCard(bool show)
	{
		if (card == null) return;
		card.gameObject.SetActive(show);
	}

	public void ShowCard()
	{
		ToggleCard(true);
	}

	public void HideCard()
	{
		ToggleCard(false);
	}

	public void Click()
	{
		card.Activate();
	}

	public void BeginDrag(PointerEventData eventData)
	{
		//Debug.Log($"Beginning drag at {eventData.position}");
		card.Activate();
	}

	public void EndDrag(PointerEventData eventData)
	{
		//Debug.Log($"End drag at {eventData.position}");
		card.ActivateRelease();
	}
}
