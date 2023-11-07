using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class CardBanner : MonoBehaviour
{
	#region Serializables

	[SerializeField, ReadOnly, Tooltip("Card reference.")]
	public Card Card;

	[SerializeField, Expandable]
	public CardDataScriptableObject CardData;

	[SerializeField, Tooltip("Invoked when the card is activated.")]
	public UnityEvent OnActivateEvent = new UnityEvent();

	[SerializeField, Tooltip("Invoked when the activated card is released.")]
	public UnityEvent OnActivateReleaseEvent = new UnityEvent();

	[SerializeField, Tooltip("Invoked when the activated card is cancelled.")]
	public UnityEvent OnActivateCancelEvent = new UnityEvent();

	[SerializeField, Tooltip("Invoked when the card is consumed.")]
	public UnityEvent OnConsumeEvent = new UnityEvent();

	[SerializeField, ReadOnly]
	public bool IsActivated = false;

	#endregion

	#region Member Declarations

	private EventTrigger trigger;

	/// <summary>
	/// Initialized by the <see cref="cardManager"/> that instantiated this <see cref="CardBanner"/> object, providing a reference to itself.
	/// </summary>
	protected CardManager cardManager;

	#endregion

	#region Monobehaviour

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

		EventTrigger.Entry beginHoverEntry = new EventTrigger.Entry();
		beginHoverEntry.eventID = EventTriggerType.PointerEnter;
		beginHoverEntry.callback.AddListener((data) => HoverEnter((PointerEventData)data));

		EventTrigger.Entry endHoverEntry = new EventTrigger.Entry();
		endHoverEntry.eventID = EventTriggerType.PointerExit;
		endHoverEntry.callback.AddListener((data) => HoverExit((PointerEventData)data));
	}

	#endregion

	#region Helper Functions

	public void ToggleCard(bool show)
	{
		if (Card == null) return;
		Card.gameObject.SetActive(show);
	}

	public void ShowCard()
	{
		ToggleCard(true);
	}

	public void HideCard()
	{
		ToggleCard(false);
	}

	#endregion

	#region Event Triggers

	public void Click()
	{
		if (IsActivated)
		{
			ActivateCancel();
		}
		else
		{
			Activate();
		}
	}

	public void BeginDrag(PointerEventData eventData)
	{
		//Debug.Log($"Beginning drag at {eventData.position}");
		Activate();
	}

	public void EndDrag(PointerEventData eventData)
	{
		//Debug.Log($"End drag at {eventData.position}");
		ActivateRelease();
	}

	public void HoverEnter(PointerEventData eventData)
	{
		ShowCard();
	}

	public void HoverExit(PointerEventData eventData)
	{
		HideCard();
	}

	#endregion

	#region Card Banner Events

	public void Activate()
	{
		if (IsActivated) return;
		IsActivated = true;
		OnActivateEvent.Invoke();
		OnActivate();
	}

	protected virtual void OnActivate() { }

	public void ActivateRelease()
	{
		if (!IsActivated) return;
		IsActivated = false;
		OnActivateReleaseEvent.Invoke();
		OnActivateRelease();
	}

	protected virtual void OnActivateRelease() { }

	public void ActivateCancel()
	{
		if (!IsActivated) return;
		IsActivated = false;
		OnActivateCancelEvent.Invoke();
		OnActivateCancel();
	}

	protected virtual void OnActivateCancel() { }

	public void ConsumeCard()
	{
		OnConsumeEvent.Invoke();
		OnConsumeCard();
	}

	protected virtual void OnConsumeCard()
	{
		// Call the card manager for clean up
		cardManager.ConsumeCard(this);
	}

	#endregion
}
