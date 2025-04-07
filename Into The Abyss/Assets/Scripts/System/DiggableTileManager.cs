using UnityEngine;
using UnityEngine.Tilemaps;
using AYellowpaper.SerializedCollections;

public class DiggableTileManager : Singleton<DiggableTileManager>
{
	[Header("Tile Table"), Space]
	[SerializeField] private SerializedDictionary<TileBase, bool> tileTable = new SerializedDictionary<TileBase, bool>();

	public bool IsDiggable(TileBase tile)
	{
		if (tileTable.ContainsKey(tile))
		{
			return tileTable[tile];
		}
		else
		{
			Debug.LogWarning($"Tile {tile.name} is not in the tile table.");
			return false;
		}
	}
}