using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
	#region Type Definitions

	public enum TowerManagerState
	{
		Idle,
		Placing,
		Selling,
	}

	/// <summary>
	/// Placement map sample value. Not the final determiner for whether a surface is suitable for the tower.
	/// </summary>
	private enum PlacementSampleState
	{
		Obstacle,
		Land,
		Water,
		Invalid
	}

	/// <summary>
	/// Any colour not listed for mapping will be mapped to invalid.
	/// </summary>
	private static readonly Dictionary<Color, PlacementSampleState> ColorToStateMap = new Dictionary<Color, PlacementSampleState>()
	{
		{ Color.red, PlacementSampleState.Obstacle },
		{ Color.green, PlacementSampleState.Land },
		{ Color.blue, PlacementSampleState.Water }
	};

	public enum FinalPlacementState
	{
		Valid,
		Invalid
	}

	#endregion

	#region Serializables

	[SerializeField, Tooltip("Texture map that represents the game map surfaces.\n" +
		"Red (255, 0, 0) = Enemy path/obstacle/unplaceable\n" +
		"Green (0, 255, 0) = Land placeable\n" +
		"Blue (0, 0, 255) = Water placeable")]
	public Texture2D placementMap;

	[SerializeField, Tooltip("Actual map SpriteRenderer. Used as positioning reference.")]
	public SpriteRenderer mapSprite;

	[SerializeField, Tooltip("Tower preview object, used for displaying where the tower will be built.")]
	public TowerPreview towerPreview;

	#endregion

	#region Member Declarations

	/// <summary>
	/// The current state of the tower manager.
	/// </summary>
	[ShowNativeProperty]
	public TowerManagerState state { get; private set; } = TowerManagerState.Idle;

	/// <summary>
	/// List of all the built towers in the map.
	/// </summary>
	public List<BaseTower> towers { get; set; } = new List<BaseTower>();

	#endregion

	#region Monobehaviour

	private void Update()
	{
		
	}

	private void OnDrawGizmosSelected()
	{
		if (mapSprite == null) return;
		Gizmos.color = Color.red;

		Bounds bounds = mapSprite.sprite.bounds;

		// Draw map bounds
		Gizmos.DrawWireCube(mapSprite.transform.position, bounds.size);
	}

	#endregion

	public void PlaceTower(BaseTower tower)
	{
		if (state == TowerManagerState.Placing || state == TowerManagerState.Selling)
		{
			Debug.Log("TowerManager is currently in the placing or selling state, thus unable to enter the placing state (again).");
			return;
		}
		if (tower == null)
		{
			Debug.Log("Tower is null, not entering placement state.");
			return;
		}
		state = TowerManagerState.Placing;

		towerPreview.gameObject.SetActive(true);
		towerPreview.SetSprite(tower.TowerSprite.sprite);
	}
}
