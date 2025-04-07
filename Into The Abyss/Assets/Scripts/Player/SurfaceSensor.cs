using UnityEngine;

public class SurfaceSensor : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform wallCheck;
	[SerializeField] private LayerMask groundLayers;

	[Header("Settings"), Space]
	[SerializeField] private float groundCheckRadius;
	[SerializeField] private float wallCheckRadius;

	public bool Grounded { get; private set; }
	public bool TouchedWalls { get; private set; }

	private void FixedUpdate()
	{
		Grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers) != null;
		TouchedWalls = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, groundLayers) != null;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
	}
}
