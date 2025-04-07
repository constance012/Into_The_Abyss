using Unity.Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private CinemachineCamera followCamera;
	[SerializeField] private Transform bottomPoint;

	[Header("Camera Follow Settings"), Space]
	[SerializeField] private float moveSpeed = 3f;
	[SerializeField] private float followSpeed = 2f;
	[SerializeField] private float gameOverDelay;

	private Transform _cameraTransform;
	private Vector3 _targetPos;
	private float _gameOverDelay;
	private float _distanceY;

	private void Start()
	{
		_cameraTransform = followCamera.transform;
		_targetPos = _cameraTransform.position;
		_gameOverDelay = gameOverDelay;
	}

	void Update()
	{
		if (GameManager.Instance.GameStarted)
		{
			MoveDown();
			FollowPlayer();
		}
	}

	private void MoveDown()
	{
		if (_cameraTransform.position.y > bottomPoint.position.y)
		{
			transform.position += moveSpeed * Time.deltaTime * Vector3.down;
		}

		_distanceY = transform.position.y - PlayerController.Position.y;

		if (_distanceY > followCamera.Lens.OrthographicSize)
		{
			_targetPos = new Vector3(transform.position.x, PlayerController.Position.y, transform.position.z);
		}
		
		if (_distanceY < -followCamera.Lens.OrthographicSize)
		{
			_gameOverDelay -= Time.deltaTime;
			
			if (_gameOverDelay <= 0f)
			{
				GameManager.Instance.GameOver();
				_gameOverDelay = gameOverDelay;
			}
		}
	}

	private void FollowPlayer()
	{
		if(_distanceY > followCamera.Lens.OrthographicSize)
		{
			transform.position = Vector3.Slerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
		}
	}
}
