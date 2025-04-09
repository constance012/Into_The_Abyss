using UnityEngine;

public class SurfaceSensor : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform wallCheck;
	[SerializeField] private LayerMask groundLayers;

	[Header("Player Graphic"), Space]
	[SerializeField] private PlayerAnimator playerAnimator;

	[Header("Settings"), Space]
	[SerializeField] private float groundCheckRadius;
	[SerializeField] private float wallCheckRadius;

	public bool Grounded { get; private set; }
	public bool TouchedWalls { get; private set; }

	private void FixedUpdate()
	{
		playerAnimator.SetBool(PlayerAnimatorParameters.WasGrounded, Grounded);
		
		Grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers) != null;
		TouchedWalls = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, groundLayers) != null;

		playerAnimator.SetBool(PlayerAnimatorParameters.IsGrounded, Grounded);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
	}
}
