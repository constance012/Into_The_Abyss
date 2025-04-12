using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private GameObject arrowPrefab;
	[SerializeField] private Transform SpawnPoint;

	[Header("Spawn Setting"), Space]
	[SerializeField] private float spawnPerTime = 1f;
	private float timeFromLastPoint = 0f;

	[Header("Arrow Setting"), Space]
	[SerializeField] private bool isArrowFacingRight = true;
	[SerializeField] private float cameraCheckTimer;

	private Camera _mainCamera;
	private float _cameraCheckTimer;
	private bool _insideCameraView;

	private void Awake()
	{
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		if(timeFromLastPoint > 1/spawnPerTime)
		{
			SpawnArrow();
			timeFromLastPoint = 0f;
		}

		timeFromLastPoint++;
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

	private void SpawnArrow()
	{
		if (_insideCameraView)
		{
			AudioManager.Instance.PlayWithRandomPitch("Arrow Shot", .7f, 1.2f);
		}

		GameObject arrow = Instantiate(arrowPrefab, SpawnPoint.position, Quaternion.identity, SpawnPoint);
		Arrows arrowLauncher = arrow.GetComponent<Arrows>();

		arrowLauncher.SetFacingRight(!isArrowFacingRight ? -1 : 1);
	}
}
