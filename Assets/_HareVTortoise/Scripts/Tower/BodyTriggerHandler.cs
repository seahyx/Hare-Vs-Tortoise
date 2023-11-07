using UnityEngine;

public class BodyTriggerHandler : MonoBehaviour
{
	[HideInInspector]
	private TowerPreview towerPreview;

	private int totalOverlaps = 0;

	public bool IsOverlapping => totalOverlaps > 0;

	#region Monobehaviour

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.tag.Equals("Tower")) return;
		totalOverlaps++;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.gameObject.tag.Equals("Tower")) return;
		totalOverlaps--;
	}

	#endregion
}
