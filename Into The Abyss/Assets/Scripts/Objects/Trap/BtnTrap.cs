using UnityEngine;

public class BtnTrap : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform SpawnPoint;

    [Header("Arrow Setting"), Space]
    [SerializeField] private bool isArrowFacingRight = true;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private Vector2 knockbackForce = new Vector2(0f, 0f);


    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject arrow = Instantiate(arrowPrefab, SpawnPoint.position, Quaternion.identity, SpawnPoint);
        ProjectileLauncher arrowLauncher = arrow.GetComponent<ProjectileLauncher>();

        arrowLauncher.SetFacingRight(!isArrowFacingRight ? -1 : 1);
        arrowLauncher.SetMoveSpeed(moveSpeed);
        arrowLauncher.SetDamage(damage);
        arrowLauncher.SetKnockback(knockbackForce);

        Destroy(gameObject);
    }
}
