using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDigging : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Stats stats;
	[SerializeField] private Tilemap diggableTilemap;
	[SerializeField] private Transform digPoint;

	private float _digInterval;

	private void Update()
	{
		_digInterval -= Time.deltaTime;
		CheckDigging();
	}

	private void CheckDigging()
	{
		if((Input.GetMouseButton(0) || LegacyInputManager.Instance.GetKey(KeybindingActions.Dig)) && _digInterval <= 0f)
		{
			Vector3Int gridPosition = diggableTilemap.WorldToCell(digPoint.position);

			TileBase currentTile = diggableTilemap.GetTile(gridPosition);

			if(currentTile != null)
			{
				diggableTilemap.SetTile(gridPosition, null);
			}

			_digInterval = stats.GetDynamicStat(Stat.DigInterval);
		}
	}
}