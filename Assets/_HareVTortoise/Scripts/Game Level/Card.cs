using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
	#region Serializables

	[SerializeField, Tooltip("How much the card costs to activate.")]
	public int Cost = 0;

	[SerializeField, Tooltip("Invoked when the card is activated.")]
	public UnityEvent OnActivateEvent = new UnityEvent();

	[SerializeField, Tooltip("Invoked when the activated card is released.")]
	public UnityEvent OnActivateReleaseEvent = new UnityEvent();

	[SerializeField, ReadOnly]
	public bool IsActivated = false;

	#endregion

	public void Activate()
	{
		OnActivateEvent.Invoke();
		OnActivate();
	}

	public void ActivateRelease()
	{
		OnActivateReleaseEvent.Invoke();
		OnActivateRelease();
	}

	protected virtual void OnActivate()
	{

	}

	protected virtual void OnActivateRelease()
	{

	}
}
