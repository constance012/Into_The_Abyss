using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DepthMeterUI : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private TextMeshProUGUI meterText;
	[SerializeField] private float depthPerTile;

	private readonly Dictionary<MetricType, string> _unitTable = new Dictionary<MetricType, string>()
	{
		[MetricType.Imperial] = "ft",
		[MetricType.Metric] = "m"
	};

	private string _currentUnit;

	private void Start()
	{
		_currentUnit = _unitTable[(MetricType)UserSettings.MetricType];
		meterText.text = $"{0}<color=#B5B5B5><size=35>{_currentUnit}</color></size>";
	}

	private void Update()
	{
		if (PlayerController.Position.y < 0f)
		{
			UpdateUI();
		}
	}

	private void UpdateUI()
	{
		float depthFloat = Mathf.Abs(GameManager.Instance.CurrentDepthInMeter * depthPerTile);
		float depthConverted = (MetricType)UserSettings.MetricType == MetricType.Metric ? depthFloat : depthFloat * 3.281f;

		int depthInt = Mathf.CeilToInt(depthConverted);
		meterText.text = $"{depthInt}<color=#B5B5B5><size=35>{_currentUnit}</color></size>";
	}
}

public enum MetricType
{
	Imperial = 0,
	Metric = 1
}