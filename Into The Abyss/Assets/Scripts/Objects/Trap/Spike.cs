using UnityEngine;

public class Spike : MonoBehaviour
{
	[Header("Stats"), Space]
	[SerializeField] private Stats stats;
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var damageable = collision.gameObject.GetComponentInParent<IDamageable>();
		damageable?.TakeDamage(stats, transform.position);
		
		if(destroyAfterInteract)
		{
			Destroy(gameObject);
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
