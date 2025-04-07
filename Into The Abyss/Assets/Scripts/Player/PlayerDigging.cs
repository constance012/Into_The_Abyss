using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDigging : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Stats stats;
	[SerializeField] private Tilemap diggableTilemap;
	[SerializeField] private Transform digPivot;
	[SerializeField] private Transform digPoint;

	[Header("Player Graphic"), Space]
	[SerializeField] private PlayerAnimator playerAnimator;

	[Header("Destructibles"), Space]
	[SerializeField] private float attackRadius;
	[SerializeField] private LayerMask destructibleLayers;

	private Vector3 _lookDirection;
	private float _digInterval;

	private void Update()
	{
		_digInterval -= Time.deltaTime;
		AimAtMouse();
	}

	private void LateUpdate()
	{
		CheckDigging();
	}

	private void AimAtMouse()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_lookDirection = (mousePos - PlayerController.Position).normalized;

		float lookAngle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;

		digPivot.eulerAngles = Vector3.forward * lookAngle;
	}

	private void CheckDigging()
	{
		if(Input.GetMouseButton(0) || LegacyInputManager.Instance.GetKey(KeybindingActions.Dig))
		{
			playerAnimator.SetBool(PlayerAnimatorParameters.IsDigging, true);

			CheckDestructible();

			if (_digInterval <= 0f)
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
		else
		{
			playerAnimator.SetBool(PlayerAnimatorParameters.IsDigging, false);
		}
	}

	private void CheckDestructible()
	{
		Collider2D destructible = Physics2D.OverlapCircle(digPoint.position, attackRadius, destructibleLayers);

		if (destructible != null)
		{
			if (destructible.TryGetComponent<Destructibles>(out var component))
			{
				component.Break();
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(digPoint.position, attackRadius);

		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + _lookDirection);
	}
}