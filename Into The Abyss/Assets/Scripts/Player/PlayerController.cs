using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
	[Header("Refernces"), Space]
	[SerializeField] private Stats stats;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private SurfaceSensor surfaceSensor;
	[SerializeField] private Transform playerGraphic;

	[Header("Movement"), Space]
	[SerializeField] private float acceleration = 0.29f;
	[SerializeField] private float deceleration = 0.29f;

	[Header("Jumping"), Space]
	[SerializeField] private float gravity = 9.81f;

	[Header("Digging")]
	[SerializeField] private Tilemap ground;
	[SerializeField] private Transform digPoint;

	[Header("Tile Table"), Space]
	[SerializeField]private SerializedDictionary<TileBase, TypeTile> tileTable;

	private float _inputX;
	private float _previousInputX;
	private float _digInterval;
	private float _speedX;
	private bool _needToJump;
	private bool _facingRight = true;

	private void Update()
	{
		CheckInput();
		CheckFlip();
		CheckDigging();

		_digInterval -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		HandleJumping();
		HandleMovement();
	}

	private void CheckInput()
	{
		if (LegacyInputManager.Instance.GetKeyDown(KeybindingActions.Jump) && surfaceSensor.Grounded)
		{
			_needToJump = true;
		}

		_inputX = LegacyInputManager.Instance.GetAxisRaw("Horizontal");
		_previousInputX = Mathf.Sign(_speedX);
	}

	private void CheckFlip()
	{
		bool mustFlip = (_facingRight && _speedX < 0f) || (!_facingRight && _speedX > 0f);

		if (mustFlip)
		{
			playerGraphic.Rotate(0f, -180f, 0f);
			_facingRight = !_facingRight;
		}
	}

	private void CheckDigging()
	{
		if((Input.GetMouseButton(0) || LegacyInputManager.Instance.GetKey(KeybindingActions.Dig)) && _digInterval <= 0f)
		{
			Vector3Int gridPosition = ground.WorldToCell(digPoint.position);

			TileBase currentTile = ground.GetTile(gridPosition);

			if(currentTile != null && tileTable.TryGetValue(currentTile, out TypeTile typeTile))
			{
				if (typeTile.Destructible)
				{
					ground.SetTile(gridPosition, null);
				}
			}

			_digInterval = stats.GetDynamicStat(Stat.DigInterval);
		}
	}

	private void HandleMovement()
	{		
		if (Mathf.Abs(_inputX) > 0f)
		{
			// Accelerate.
			if (_inputX >= 0f)
			{
				_speedX += acceleration * _inputX * Time.deltaTime;
				_speedX = Mathf.Min(stats.GetDynamicStat(Stat.MoveSpeed), _speedX);
			}
			else
			{
				_speedX += acceleration * _inputX * Time.deltaTime;
				_speedX = Mathf.Max(-stats.GetDynamicStat(Stat.MoveSpeed), _speedX);
			}
		}
		else if (Mathf.Abs(_speedX) > 0f)
		{
			// Decelerate.
			if (_speedX >= 0f)
			{
				_speedX -= deceleration * _previousInputX * Time.deltaTime;
				_speedX = Mathf.Max(0f, _speedX);
			}
			else
			{
				_speedX -= deceleration * _previousInputX * Time.deltaTime;
				_speedX = Mathf.Min(0f, _speedX);
			}
		}
		
		rb.linearVelocityX = _speedX;
	}

	private void HandleJumping()
	{
		if(_needToJump)
		{
			rb.AddForce(Vector2.up * stats.GetDynamicStat(Stat.JumpForce), ForceMode2D.Impulse);
		}
		
		if(!surfaceSensor.Grounded)
		{
			rb.linearVelocityY -= gravity * Time.deltaTime;
			rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -stats.GetStaticStat(Stat.FallSpeed));
		}
		else
		{
			_needToJump = false;
		}
	}
}
