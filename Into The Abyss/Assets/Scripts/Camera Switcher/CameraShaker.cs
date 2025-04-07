using UnityEngine;
using Unity.Cinemachine;

[AddComponentMenu("Singletons/Camera Shaker")]
public sealed class CameraShaker : Singleton<CameraShaker>
{
	[Header("References"), Space]
	[SerializeField] private CinemachineCamera virtualCamera;

	[Header("Global Shake Settings"), Space]
	[SerializeField] private float globalInitialAmplitude;
	[SerializeField] private float globalShakeDuration;

	// Private fields.
	private CinemachineBasicMultiChannelPerlin _shaker;
	private float _remainingTime;

	private float _localInitialAmplitude;
	private float _localShakeDuration;
	private bool _isGlobalShake;

	protected override void Awake()
	{
		base.Awake();

		//_shaker = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise);
	}

	private void Update()
	{
		if (_remainingTime > 0f)
		{
			if (_isGlobalShake)
				_shaker.AmplitudeGain = Mathf.Lerp(0f, globalInitialAmplitude, _remainingTime / globalShakeDuration);
			else
				_shaker.AmplitudeGain = Mathf.Lerp(0f, _localInitialAmplitude, _remainingTime / _localShakeDuration);

			_remainingTime -= Time.deltaTime;
		}
		else
		{
			_remainingTime = 0f;
			_shaker.AmplitudeGain = 0f;
		}
	}

	/// <summary>
	/// Shakes the camera using the GLOBAL shake settings.
	/// </summary>
	public void ShakeCamera()
	{
		_shaker.AmplitudeGain = globalInitialAmplitude;
		_remainingTime = globalShakeDuration;
		_isGlobalShake = true;
	}

	/// <summary>
	/// Shakes the camera with the specified amplitude and duration.
	/// </summary>
	/// <param name="amplitude"></param>
	/// <param name="duration"></param>
	public void ShakeCamera(float amplitude, float duration)
	{
		_localInitialAmplitude = amplitude;
		_localShakeDuration = duration;

		_shaker.AmplitudeGain = amplitude;
		_remainingTime = duration;
		_isGlobalShake = false;
	}
}
