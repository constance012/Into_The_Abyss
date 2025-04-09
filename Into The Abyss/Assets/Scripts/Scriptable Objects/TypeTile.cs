using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile")]
public class TypeTile : ScriptableObject
{
    public TileBase[] Tiles;

    public bool Destructible;
}
