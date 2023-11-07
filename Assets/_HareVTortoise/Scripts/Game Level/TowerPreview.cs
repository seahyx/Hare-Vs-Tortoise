using System.Collections;
using System.Collections.Generic;
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

	#endregion

	public void SetSprite(Sprite sprite)
	{
		TowerSprite.sprite = sprite;
	}

	public void SetRange(float range)
	{
		RangeTransform.localScale = new Vector3(range, range, 1.0f);
	}

	public void SetState(TowerPlacementManager.FinalPlacementState state)
	{
		switch (state)
		{
			default:
				TowerSprite.color = InvalidPlacementColor; break;
			case TowerPlacementManager.FinalPlacementState.Valid:
				TowerSprite.color = ValidPlacementColor;
				break;
			case TowerPlacementManager.FinalPlacementState.Invalid:
				TowerSprite.color = ObstaclePlacementColor;
				break;
		}
	}
}
