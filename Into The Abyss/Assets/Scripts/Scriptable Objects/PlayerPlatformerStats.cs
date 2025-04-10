using UnityEngine;

[CreateAssetMenu(menuName = "Unit Stats/Player Platformer Stats", fileName = "New Platformer Stats")]
public class PlayerPlatformerStats : ScriptableObject
{
	[Header("Layers"), Space]
	public LayerMask playerLayer;

	[Header("Ground Movement"), Space]
	public float groundAcceleration = 30f;
	public float groundDeceleration = 30f;

	[Tooltip("A constant downwards force applied when grounded."), Range(0f, -5f)]
	public float groundingForce = -2f;
	public float surfaceCastDistance = .05f;

	[Header("Air Movement"), Space]
	public float maxFallSpeed = -5f;

	[Tooltip("How fast the player comes to a stop after ceasing input mid-air.")]
	public float airDeceleration = 10f;

	[Tooltip("How fast the player gains fall speed, or in air gravity.")]
	public float fallGravity = 70f;
	public float gravityMultiplierWhenEndJumpEarly = 3f;
	
	[Tooltip("The time period that allows jumping after leaving the edge or cliff.")]
	public float coyoteTime = .15f;

	[Tooltip("The time period that allows jumping even before hitting the ground.")]
	public float jumpBufferTime = .2f;
}
