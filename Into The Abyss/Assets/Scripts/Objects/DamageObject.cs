using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [Header("Damage"), Space]
    [SerializeField] private int damage = 1;
    [SerializeField] private Vector2 knockbackForce = new Vector2(0f, 0f);
    [SerializeField] private bool destroyAfterInteract;

    void OnCollisionEnter2D(Collision2D collision)
    {
        HealthPoint hp = collision.gameObject.GetComponent<HealthPoint>();
        if(hp != null)
        {
            int signKnockBack = collision.transform.position.x >= transform.position.x ? 1 : -1;
            Vector2 knockback = new Vector2(signKnockBack * knockbackForce.x, knockbackForce.y);

            hp.ChangeHealth(-damage, knockback);
            
            if(destroyAfterInteract)
            {
                Destroy(gameObject);
            }
        }
    }
}
