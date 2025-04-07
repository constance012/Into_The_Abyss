using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform SpawnPoint;

    [Header("Spawn Setting"), Space]
    [SerializeField] private float spawnPerTime = 1f;
    private float timeFromLastPoint = 0f;

    [Header("Arrow Setting"), Space]
    [SerializeField] private bool isArrowFacingRight = true;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private Vector2 knockbackForce = new Vector2(0f, 0f);

    // Update is called once per frame
    void Update()
    {
        if(timeFromLastPoint > 1/spawnPerTime)
        {
            SpawnArrow();
            timeFromLastPoint = 0f;
        }

        timeFromLastPoint++;
    }

    private void SpawnArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, SpawnPoint.position, Quaternion.identity, SpawnPoint);
        ProjectileLauncher arrowLauncher = arrow.GetComponent<ProjectileLauncher>();

        arrowLauncher.SetFacingRight(!isArrowFacingRight ? -1 : 1);
        arrowLauncher.SetMoveSpeed(moveSpeed);
        arrowLauncher.SetDamage(damage);
        arrowLauncher.SetKnockback(knockbackForce);
    }
}
