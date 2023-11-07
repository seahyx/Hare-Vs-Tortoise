using NaughtyAttributes;
using System;
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
	/// Placement map surfaces. Not the final determiner for whether a surface is suitable for the tower.
	/// </summary>
	[Flags]
	public enum Surfaces
	{
		Invalid = 0,
		Road = 1 << 0,
		Land = 1 << 1,
		Water = 1 << 2,
		Obstacle = 1 << 3,
	}

	/// <summary>
	/// Any colour not listed for mapping will be mapped to invalid.
	/// </summary>
	private static readonly Dictionary<Color, Surfaces> ColorToSurfaceMap = new Dictionary<Color, Surfaces>()
	{
		{ Color.black, Surfaces.Road },
		{ Color.green, Surfaces.Land },
		{ Color.blue, Surfaces.Water }
	};

	public enum FinalPlacementState
	{
		Valid,
		Invalid
	}

	#endregion

	#region Serializables

	[SerializeField, Tooltip("Texture map that represents the game map surfaces.\n" +
		"Red (0, 0, 0) = Enemy path/obstacle/unplaceable\n" +
		"Green (0, 255, 0) = Land placeable\n" +
		"Blue (0, 0, 255) = Water placeable")]
	private Texture2D surfaceMap;

	[SerializeField, Tooltip("Actual map SpriteRenderer. Used as positioning reference.")]
	private SpriteRenderer mapSprite;

	[SerializeField, Tooltip("Tower preview object, used for displaying where the tower will be built.")]
	private TowerPreview towerPreview;

	[SerializeField, Tooltip("Transform parent to group the towers under.")]
	private Transform towersContainer;

	#endregion

	#region Member Declarations

	/// <summary>
	/// The current state of the tower manager.
	/// </summary>
	[ShowNativeProperty]
	public TowerManagerState state { get; private set; } = TowerManagerState.Idle;

	/// <summary>
	/// The current tower being built.
	/// </summary>
	[ShowNativeProperty]
	private BaseTower currentBuildTower { get; set; }

	/// <summary>
	/// List of all the built towers in the map.
	/// </summary>
	[ShowNativeProperty]
	public List<BaseTower> towers { get; set; } = new List<BaseTower>();

	#endregion

	#region Monobehaviour

	private void Update()
	{
		if (currentBuildTower == null || state == TowerManagerState.Idle || state == TowerManagerState.Selling) return;

		towerPreview.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		towerPreview.SetState(GetPlacementState(currentBuildTower));
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

	#region Tower Placement

	/// <summary>
	/// Starts the tower placement process.
	/// </summary>
	/// <param name="tower"></param>
	public void StartPlaceTower(BaseTower tower)
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
		towerPreview.SetRange(tower.RangeUnityUnit);
		
	}

	/// <summary>
	/// Finish the tower placement process.
	/// </summary>
	/// <returns>Whether the tower is successfully placed in the map.</returns>
	public bool FinishPlaceTower()
	{
		if (!towerPreview.IsValidPosition && state != TowerManagerState.Placing) return false;

		// Valid position, instantiate tower here
		BaseTower builtTower = Instantiate(currentBuildTower, towerPreview.transform.position, towerPreview.transform.rotation, towersContainer);
		towers.Add(builtTower);

		// Hide preview, clear the currently building tower
		towerPreview.gameObject.SetActive(false);
		currentBuildTower = null;

		// Update state
		state = TowerManagerState.Idle;

		return true;
	}

	/// <summary>
	/// Cancel the tower placement process. Nothing will be consumed.
	/// </summary>
	public void CancelPlaceTower()
	{
		if (state != TowerManagerState.Placing) return;

		// Hide preview, clear the currently building tower
		towerPreview.gameObject.SetActive(false);
		currentBuildTower = null;

		// Update state
		state = TowerManagerState.Idle;
	}

	#endregion

	#region Helper Functions

	/// <summary>
	/// Get the current <see cref="Surfaces"/> under the mouse cursor.
	/// </summary>
	/// <returns>The current <see cref="Surfaces"/> under the mouse cursor.</returns>
	private Surfaces GetSurfaceAtPointer() => GetSurfaceFromUV(PointerToMapUV());

	/// <summary>
	/// Converts the mouse position to (u, v) coordinates on the surface map, where u and v are floats in range [0..1].
	/// Values are not clamped.
	/// </summary>
	/// <returns>(u, v) coordinates on the surface map.</returns>
	private Vector2 PointerToMapUV()
	{
		Vector2 pointInMapCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mapSprite.bounds.min;
		pointInMapCoords.x = pointInMapCoords.x / mapSprite.size.x;
		pointInMapCoords.y = pointInMapCoords.y / mapSprite.size.y;
		return pointInMapCoords;
	}

	/// <summary>
	/// Samples the placement map at position (u, v), where u and v are floats in the range [0..1].
	/// (0, 0) is the bottom left of the map.
	/// Samples the nearest pixel without filtering.
	/// </summary>
	/// <param name="position">Vector2 value in the form of (u, v), where u and v are floats in the range [0..1].</param>
	private Surfaces GetSurfaceFromUV(Vector2 position)
	{
		// Check OOB pointer position
		if (position.x < 0 || position.x > 1 || position.y < 0 || position.y > 1)
			return Surfaces.Invalid;

		// Get x, y pixel coordinates
		int x = (int)(position.x * surfaceMap.width);
		int y = (int)(position.y * surfaceMap.height);

		// Try to get placement value from color of texture at (x, y)
		Surfaces sampleState;
		if (ColorToSurfaceMap.TryGetValue(surfaceMap.GetPixel(x, y), out sampleState))
			return sampleState;

		// No color matches dict
		return Surfaces.Invalid;
	}

	/// <summary>
	/// Gets the final placement state of a tower at the current mouse position.
	/// </summary>
	/// <param name="tower">Tower to be placed.</param>
	/// <returns></returns>
	private FinalPlacementState GetPlacementState(BaseTower tower)
	{
		FinalPlacementState finalPlacementState = tower.PlaceableSurfaces.HasFlag(GetSurfaceAtPointer())
			? FinalPlacementState.Valid : FinalPlacementState.Invalid;
		return finalPlacementState;
	}

	#endregion
}
