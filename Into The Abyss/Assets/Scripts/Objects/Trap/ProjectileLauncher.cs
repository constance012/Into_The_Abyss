using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes"), Space]
    [SerializeField] private float timeToDestroy = 12f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int facingRight = 1;
    [SerializeField] private int damage = 1;
    [SerializeField] private Vector2 knockbackForce = Vector2.zero;

    public void SetFacingRight(int facingRight)
    {
        this.facingRight = facingRight;
    }
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    public void SetKnockback(Vector2 knockback)
    {
        this.knockbackForce = knockback;
    }

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.localScale = new Vector3(-facingRight, transform.localScale.y, transform.localScale.z);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(facingRight * moveSpeed, rb.linearVelocityY);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPoint hp = collision.GetComponentInParent<HealthPoint>();
        if(hp != null)
        {
            int signKnockBack = collision.transform.position.x >= transform.position.x ? 1 : -1;
            Vector2 knockback = new Vector2(signKnockBack * knockbackForce.x, knockbackForce.y);

            hp.ChangeHealth(-damage, knockback);
        }

        Destroy(gameObject);
    }
}
