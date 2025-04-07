using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.Cinemachine;

public sealed class CameraSwitcher : Singleton<CameraSwitcher>
{
	[Header("Subscribed Cameras"), Space]
	[SerializeField] private SerializedDictionary<CameraType, CinemachineVirtualCameraBase> cameras;

	public CinemachineVirtualCameraBase Active => _activeCamera;

	// Private fields.
	private CinemachineVirtualCameraBase _activeCamera;

	public void Switch(CameraType type)
	{
		if (_activeCamera != cameras[type])
		{
			foreach (var camera in cameras)
			{
				if (camera.Key == type)
				{
					camera.Value.Priority = 20;
					_activeCamera = camera.Value;
				}
				else
					camera.Value.Priority = 0;
			}
		}
	}
}

public enum CameraType
{
	SurfaceCamera,
	DigCamera
}