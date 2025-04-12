using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Stats stats;
	[SerializeField] private Rigidbody2D rb2D;
	[SerializeField] private SurfaceSensor surfaceSensor;

	[Header("Player Graphic"), Space]
	[SerializeField] private Transform playerGraphic;
	[SerializeField] private PlayerAnimator playerAnimator;

	[Header("Movement"), Space]
	[SerializeField] private float acceleration = 0.29f;
	[SerializeField] private float deceleration = 0.29f;

	[Header("Jumping"), Space]
	[SerializeField] private float gravity = 9.81f;

	public static Vector2 Position { get; private set; }

	private float _inputX;
	private float _previousInputX;
	private float _speedX;
	private bool _needToJump;
	private bool _facingRight = true;

	private void Awake()
	{
		Position = rb2D.position;
	}

	private void Update()
	{
		if (GameManager.Instance.GameDone)
		{
			return;
		}

		CheckInput();
		CheckFlip();
	}

	private void FixedUpdate()
	{
		if (GameManager.Instance.GameDone)
		{
			return;
		}

		HandleJumping();		
		HandleMovement();

		Position = rb2D.position;
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
		bool mustFlip;

		if (surfaceSensor.TouchedWalls)
		{
			mustFlip = (_facingRight && _inputX < 0f) || (!_facingRight && _inputX > 0f);
		}
		else
		{
			mustFlip = (_facingRight && _speedX < 0f) || (!_facingRight && _speedX > 0f);
		}

		if (mustFlip)
		{
			playerGraphic.Rotate(0f, -180f, 0f);
			_facingRight = !_facingRight;
		}
	}

	private void HandleMovement()
	{
		if (surfaceSensor.TouchedWalls)
		{
			if (Mathf.Abs(_speedX) > 0f)
			{
				_speedX = 0f;
			}
			// Accelerate.
			if (_inputX >= 0f && !_facingRight)
			{
				_speedX += acceleration * _inputX * Time.deltaTime;
				_speedX = Mathf.Min(stats.GetDynamicStat(Stat.MoveSpeed), _speedX);
			}
			else if (_inputX < 0f && _facingRight)
			{
				_speedX += acceleration * _inputX * Time.deltaTime;
				_speedX = Mathf.Max(-stats.GetDynamicStat(Stat.MoveSpeed), _speedX);
			}
		}
		else
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
		}
		
		rb2D.linearVelocityX = _speedX;
		playerAnimator.SetFloat(PlayerAnimatorParameters.VelocityX, Mathf.Abs(_speedX));
	}

	private void HandleJumping()
	{
		if(_needToJump)
		{
			AudioManager.Instance.PlayWithRandomPitch("Jump", .7f, 1.2f);
			rb2D.AddForce(Vector2.up * stats.GetDynamicStat(Stat.JumpForce), ForceMode2D.Impulse);
		}
		
		if(!surfaceSensor.Grounded)
		{
			rb2D.linearVelocityY -= gravity * Time.deltaTime;
			rb2D.linearVelocityY = Mathf.Max(rb2D.linearVelocityY, -stats.GetStaticStat(Stat.FallSpeed));
		}
		else
		{
			_needToJump = false;
		}

		playerAnimator.SetFloat(PlayerAnimatorParameters.VelocityY, rb2D.linearVelocityY);
	}
}
