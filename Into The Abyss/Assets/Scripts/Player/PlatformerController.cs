using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerController : MonoBehaviour
{
	[Header("Platformer Stats"), Space]
	[SerializeField] private PlayerPlatformerStats stats;

	[Header("Physic"), Space]
	[SerializeField] private Rigidbody2D rb2D;
	[SerializeField] private CapsuleCollider2D col;
	[SerializeField] private float gravityScaleOnDisabled = 1f;
	
	[Header("Graphic"), Space]
	[SerializeField] private Transform playerGraphic;
	[SerializeField] private PlayerAnimator animator;

	public static Vector2 Position { get; private set; }

	private FrameInputData _frameInput;
	private Vector2 _frameVelocity;
	private bool _raycastStartsInColliders;
	private float _time;

	private void Awake()
	{
		_frameInput = new FrameInputData();

		_raycastStartsInColliders = Physics2D.queriesStartInColliders;
		
		_pressedJumpTime = float.MinValue;
		_leftGroundTime = float.MinValue;

		Position = rb2D.position;
	}

	private void Update()
	{
		if (GameManager.Instance.GameDone)
		{
			return;
		}

		_time += Time.deltaTime;

		CheckForInput();
		CheckFlip();
	}

	private void FixedUpdate()
	{
		CheckForGroundAndCeiling();

		HandleJumping();
		HandleGroundMovement();
		HandleGravity();

		ApplyFrameMovement();
	}

	public void SetEnable(bool state)
	{
		_frameInput.Reset();
		_frameVelocity = Vector2.zero;

		rb2D.gravityScale = state ? 0f : gravityScaleOnDisabled;

		this.enabled = state;
	}

	private void CheckForInput()
	{
		_frameInput.jumpPressedDown = LegacyInputManager.Instance.GetKeyDown(KeybindingActions.Jump) || Input.GetKeyDown(KeyCode.W);
		_frameInput.jumpHeld = LegacyInputManager.Instance.GetKey(KeybindingActions.Jump) || Input.GetKey(KeyCode.W);
		_frameInput.direction = new Vector2(LegacyInputManager.Instance.GetAxisRaw("Horizontal"),
											LegacyInputManager.Instance.GetAxisRaw("Vertical"));

		if (_frameInput.jumpPressedDown)
		{
			_needToJump = true;
			_pressedJumpTime = _time;
		}
	}


	#region Collision
	private bool _isGrounded;
	private float _leftGroundTime;

	private void CheckForGroundAndCeiling()
	{
		Physics2D.queriesStartInColliders = false;
		animator.SetBool(PlayerAnimatorParameters.WasGrounded, _isGrounded);

		bool groundHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0f, Vector2.down, stats.surfaceCastDistance, ~stats.playerLayer);
		bool ceilingHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0f, Vector2.up, stats.surfaceCastDistance, ~stats.playerLayer);

		if (ceilingHit)
		{
			_frameVelocity.y = Mathf.Min(0f, _frameVelocity.y);
		}

		if (groundHit && !_isGrounded)
		{
			// Landing on the ground.
			_isGrounded = true;
			_bufferedJumpAvailable = true;
			_coyoteAvailable = true;
		}
		else if (!groundHit && _isGrounded)
		{
			// Left the ground.
			_isGrounded = false;
			_leftGroundTime = _time;
		}

		Physics2D.queriesStartInColliders = _raycastStartsInColliders;
		animator.SetBool(PlayerAnimatorParameters.IsGrounded, _isGrounded);
	}
	#endregion


	#region Jumping
	private bool _needToJump;
	private bool _bufferedJumpAvailable;
	private bool _coyoteAvailable;
	private float _pressedJumpTime;

	private bool HasBufferedJump => _bufferedJumpAvailable && _pressedJumpTime + stats.jumpBufferTime > _time;
	private bool CanPerformCoyote => _coyoteAvailable && !_isGrounded && _leftGroundTime + stats.coyoteTime > _time;

	private void HandleJumping()
	{
		if (!_needToJump && !HasBufferedJump)
		{
			return;
		}

		if (_isGrounded || CanPerformCoyote)
		{
			PerformJump();
		}
		
		_needToJump = false;
	}

	private void PerformJump()
	{
		_frameVelocity.y = stats.jumpForce;

		_bufferedJumpAvailable = false;
		_coyoteAvailable = false;
		_pressedJumpTime = 0f;
		_leftGroundTime = 0f;
	}
	#endregion


	#region Ground Movement
	private bool _facingRight = true;

	private void HandleGroundMovement()
	{
		if (_frameInput.direction.x == 0f)
		{
			float deceleration = _isGrounded ? stats.groundDeceleration : stats.airDeceleration;
			_frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0f, deceleration * Time.deltaTime);
		}
		else
		{
			float targetSpeed = _frameInput.direction.x * stats.maxGroundSpeed;
			_frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, targetSpeed, stats.groundAcceleration * Time.deltaTime);
		}
	}

	private void CheckFlip()
	{
		bool mustFlip = (_facingRight && _frameInput.direction.x < 0f) || (!_facingRight && _frameInput.direction.x > 0f);

		if (mustFlip)
		{
			playerGraphic.Rotate(0f, -180f, 0f);
			_facingRight = !_facingRight;
		}
	}
	#endregion


	#region Gravity
	private void HandleGravity()
	{
		if (_isGrounded && _frameVelocity.y <= 0f)
		{
			_frameVelocity.y = -stats.groundingForce;
		}
		else
		{
			float gravity = stats.fallGravity;
			_frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -stats.maxFallSpeed, gravity * Time.deltaTime);
		}
	}
	#endregion

	private void ApplyFrameMovement()
	{
		rb2D.linearVelocity = _frameVelocity;
		Position = rb2D.position;

		animator.SetFloat(PlayerAnimatorParameters.VelocityX, Mathf.Abs(_frameVelocity.x));
		animator.SetFloat(PlayerAnimatorParameters.VelocityY, _frameVelocity.y);
	}
}

public struct FrameInputData
{
	public bool jumpPressedDown;
	public bool jumpHeld;
	public Vector2 direction;

	public void Reset()
	{
		jumpPressedDown = false;
		jumpHeld = false;
		direction = Vector2.zero;
	}
}