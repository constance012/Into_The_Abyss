using UnityEngine;

public class DamageObject : MonoBehaviour
{
	[Header("Damage"), Space]
	[SerializeField] private int damage = 1;
	[SerializeField] private Vector2 knockbackForce = new Vector2(0f, 0f);
	[SerializeField] private bool destroyAfterInteract;

	[SerializeField] private float cameraCheckTimer;

	private Camera _mainCamera;
	private float _cameraCheckTimer;
	private bool _insideCameraView;

	private void Awake()
	{
		_mainCamera = Camera.main;
	}

	private void LateUpdate()
	{
		_cameraCheckTimer -= Time.deltaTime;

		if (_cameraCheckTimer <= 0f)
		{
			Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(transform.position);
			_insideCameraView = viewportPoint.x >= 0f && viewportPoint.x <= 1f &&
								viewportPoint.y >= 0f && viewportPoint.y <= 1f &&
								viewportPoint.z > 0f;
			
			_cameraCheckTimer = cameraCheckTimer;
		}
	}

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

	public void PlaySpikeSound()
	{
		if (_insideCameraView)
		{
			AudioManager.Instance.PlayWithRandomPitch("Spike", .7f, 1.2f);
		}
	}
}
