using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger), typeof(Image))]
public class CardBanner : MonoBehaviour
{
	#region Serializables

	[SerializeField, ReadOnly, Tooltip("Card reference.")]
	public Card Card;

	[SerializeField, ReadOnly, Expandable]
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
	/// Initialized by the <see cref="CardManager"/> that instantiated this <see cref="CardBanner"/> object, providing a reference to itself.
	/// </summary>
	public CardManager CardManager;

	public Image Image { get; private set; }

	#endregion

	#region Monobehaviour

	private void Start()
	{
		Image = GetComponent<Image>();
		Image.sprite = CardData.CardBannerSprite;

		trigger = GetComponent<EventTrigger>();
		EventTrigger.Entry clickEntry = new EventTrigger.Entry();
		clickEntry.eventID = EventTriggerType.PointerClick;
		clickEntry.callback.AddListener((data) => Click());
		trigger.triggers.Add(clickEntry);

		EventTrigger.Entry deClickEntry = new EventTrigger.Entry();
		deClickEntry.eventID = EventTriggerType.Deselect;
		deClickEntry.callback.AddListener((data) => Deselect());
		trigger.triggers.Add(deClickEntry);

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
		trigger.triggers.Add(beginHoverEntry);

		EventTrigger.Entry endHoverEntry = new EventTrigger.Entry();
		endHoverEntry.eventID = EventTriggerType.PointerExit;
		endHoverEntry.callback.AddListener((data) => HoverExit((PointerEventData)data));
		trigger.triggers.Add(endHoverEntry);
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
			//Debug.Log("Card clicked; cancel activation.");
			ActivateCancel();
		}
		else
		{
			//Debug.Log("Card clicked; activating.");
			Activate();
		}
	}

	public void Deselect()
	{
		//Debug.Log("Card declicked; cancel activation.");
		ActivateRelease();
	}

	public void BeginDrag(PointerEventData eventData)
	{
		//Debug.Log("Card dragged; activating.");
		Activate();
	}

	public void EndDrag(PointerEventData eventData)
	{
		//Debug.Log("Card released; activation release.");
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

	protected virtual void OnActivate()
	{
		CardManager.ActivateCard(this);
	}

	public void ActivateRelease()
	{
		if (!IsActivated) return;
		IsActivated = false;
		OnActivateReleaseEvent.Invoke();
		OnActivateRelease();
	}

	protected virtual void OnActivateRelease()
	{
		CardManager.DeactivateCard(this);
	}

	public void ActivateCancel()
	{
		if (!IsActivated) return;
		IsActivated = false;
		OnActivateCancelEvent.Invoke();
		OnActivateCancel();
	}

	protected virtual void OnActivateCancel()
	{
		CardManager.DeactivateCard(this);
	}

	public void ConsumeCard()
	{
		OnConsumeEvent.Invoke();
		OnConsumeCard();
	}

	protected virtual void OnConsumeCard()
	{
		// Call the card manager for clean up
		CardManager.ConsumeCard(this);
	}

	#endregion
}
