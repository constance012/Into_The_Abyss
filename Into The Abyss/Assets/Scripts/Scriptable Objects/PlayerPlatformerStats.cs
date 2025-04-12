using UnityEngine;

[CreateAssetMenu(menuName = "Unit Stats/Player Platformer Stats", fileName = "New Platformer Stats")]
public class PlayerPlatformerStats : ScriptableObject
{
	[Header("Layers"), Space]
	public LayerMask playerLayer;

	[Min(1f), Header("Ground Movement"), Space]
	public float maxGroundSpeed = 14f;

	[Min(5f), Space]
	public float groundAcceleration = 30f;

	[Min(5f)]
	public float groundDeceleration = 30f;

	/// <summary>
	/// A constant downwards force applied when grounded.
	/// </summary>
	[Min(1f), Tooltip("A constant downwards force applied when grounded."), Range(0f, -5f), Space]
	public float groundingForce = 2f;

	[Range(.01f, .2f)]
	public float surfaceCastDistance = .05f;

	[Header("Air Movement"), Space]
	public float jumpForce = 36f;

	[Min(5f), Space]
	public float maxFallSpeed = 10f;

	/// <summary>
	/// How fast the player comes to a stop after ceasing input mid-air.
	/// </summary>
	[Min(5f), Tooltip("How fast the player comes to a stop after ceasing input mid-air.")]
	public float airDeceleration = 25f;

	/// <summary>
	/// How fast the player gains fall speed, or in air gravity.
	/// </summary>
	[Min(0f), Tooltip("How fast the player gains fall speed, or in air gravity."), Space]
	public float fallGravity = 70f;

	[Min(1f)]
	public float gravityMultiplierWhenEndJumpEarly = 3f;
	
	/// <summary>
	/// The time period that allows jumping after leaving the edge or cliff.
	/// </summary>
	[Min(0f), Tooltip("The time period that allows jumping after leaving the edge or cliff."), Space]
	public float coyoteTime = .15f;

	/// <summary>
	/// The time period that allows jumping even before hitting the ground.
	/// </summary>
	[Min(0f), Tooltip("The time period that allows jumping even before hitting the ground.")]
	public float jumpBufferTime = .2f;
}
