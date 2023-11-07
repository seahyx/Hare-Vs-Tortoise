using UnityEngine;

public class TowerPreview : MonoBehaviour
{
	#region Serializables

	[SerializeField]
	public SpriteRenderer TowerSprite;

	[SerializeField]
	public Transform RangeTransform;

	[SerializeField]
	public Color ValidPlacementColor = new Color(1, 1, 1, 0.75f);

	[SerializeField]
	public Color ObstaclePlacementColor = new Color(1, 0, 0, 0.5f);

	[SerializeField]
	public Color InvalidPlacementColor = new Color(1, 0, 0, 0.5f);

	[SerializeField]
	public bool IsValidPosition = false;

	#endregion

	/// <summary>
	/// Set the preview sprite.
	/// </summary>
	/// <param name="sprite">Body sprite of the tower to preview.</param>
	public void SetSprite(Sprite sprite)
	{
		TowerSprite.sprite = sprite;
	}

	/// <summary>
	/// Set the radius range of the tower preview in Unity units.
	/// </summary>
	/// <param name="range"></param>
	public void SetRange(float range)
	{
		RangeTransform.localScale = new Vector3(range, range, 1.0f);
	}

	/// <summary>
	/// Set the placement state of the tower.
	/// </summary>
	/// <param name="state"></param>
	public void SetState(TowerPlacementManager.FinalPlacementState state)
	{
		IsValidPosition = false;
		switch (state)
		{
			default:
				TowerSprite.color = InvalidPlacementColor; break;
			case TowerPlacementManager.FinalPlacementState.Valid:
				IsValidPosition = true;
				TowerSprite.color = ValidPlacementColor;
				break;
			case TowerPlacementManager.FinalPlacementState.Invalid:
				TowerSprite.color = ObstaclePlacementColor;
				break;
		}
	}
}
