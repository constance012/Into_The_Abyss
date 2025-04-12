using UnityEngine;

public class Arrows : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Stats stats;

	[Header("Attributes"), Space]
	[SerializeField] private float timeToDestroy = 12f;
	[SerializeField] private int facingRight = 1;

	public void SetFacingRight(int facingRight)
	{
		this.facingRight = facingRight;
	}

	private void Start()
	{
		Destroy(gameObject, timeToDestroy);
	}

	private void Update()
	{
		transform.localScale = new Vector3(-facingRight, transform.localScale.y, transform.localScale.z);
	}

	private void FixedUpdate()
	{
		rb.linearVelocity = new Vector2(facingRight * stats.GetDynamicStat(Stat.MoveSpeed), rb.linearVelocityY);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		var damageable = collider.GetComponentInParent<IDamageable>();
		damageable?.TakeDamage(stats, transform.position);

		Destroy(gameObject);
	}
}
