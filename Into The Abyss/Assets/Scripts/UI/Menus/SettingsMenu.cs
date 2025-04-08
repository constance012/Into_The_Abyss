using System;
using UnityEngine;
using UnityEngine.Audio;

public sealed class SettingsMenu : MonoBehaviour
{
	[Header("Menu References"), Space]
	[SerializeField] private TweenableUIMaster settingsMenu;
	[SerializeField] private TweenableUIMaster mainMenu;

	[Header("Audio Mixer"), Space]
	[SerializeField] private AudioMixer mixer;

	[Header("Slider Groups"), Space]
	[SerializeField] private SliderGroup masterSlider;
	[SerializeField] private SliderGroup musicSlider;
	[SerializeField] private SliderGroup soundSlider;
	[SerializeField] private SliderGroup ambienceSlider;

	[Header("Directional Selectors"), Space]
	[SerializeField] private StringSelector qualitySelector;
	[SerializeField] private StringSelector framerateSelector;
	[SerializeField] private StringSelector vsyncSelector;
	[SerializeField] private StringSelector measurementUnitSelector;

	private void OnEnable()
	{
		ReloadUI();
	}

	#region Callback Methods for UI.
	public async void OpenMainMenu()
	{
		await settingsMenu.SetActive(false);
		await mainMenu.SetActive(true);
	}

	public void SetMasterVolume(float amount)
	{
		mixer.SetFloat("masterVol", masterSlider.ValueAsMixerDecibel);

		masterSlider.DisplayText = ConvertDecibelToText(amount);
		UserSettings.MasterVolume = amount;
	}
	
	public void SetMusicVolume(float amount)
	{
		mixer.SetFloat("musicVol", musicSlider.ValueAsMixerDecibel);

		musicSlider.DisplayText = ConvertDecibelToText(amount);
		UserSettings.MusicVolume = amount;
	}

	public void SetSoundVolume(float amount)
	{
		mixer.SetFloat("soundVol", soundSlider.ValueAsMixerDecibel);

		soundSlider.DisplayText = ConvertDecibelToText(amount);
		UserSettings.SoundVolume = amount;
	}

	public void SetAmbienceVolume(float amount)
	{
		mixer.SetFloat("ambienceVol", ambienceSlider.ValueAsMixerDecibel);

		ambienceSlider.DisplayText = ConvertDecibelToText(amount);
		UserSettings.AmbienceVolume = amount;
	}

	public void SetQualityLevel(int index)
	{
		QualitySettings.SetQualityLevel(index);
		UserSettings.QualityLevel = index;
	}

	public void SetTargetFramerate(string value)
	{
		try
		{
			int fps = Convert.ToInt32(value);
			Application.targetFrameRate = Convert.ToInt32(fps);
			UserSettings.TargetFramerate = fps;
		}
		catch (FormatException)
		{
			Application.targetFrameRate = 60;
			UserSettings.TargetFramerate = 60;
		}
	}

	public void SetVsync(int useVsync)
	{
		Debug.Log($"Use Vsync: {Convert.ToBoolean(useVsync)}.");
		QualitySettings.vSyncCount = useVsync;
		UserSettings.UseVsync = useVsync;
	}

	public void SetMetricType(int index)
	{
		UserSettings.MetricType = index;
	}

	public void ResetToDefault()
	{
		UserSettings.ResetToDefault(UserSettings.SettingSection.All);
		ReloadUI();
	}
	#endregion

	#region Utility Functions.
	private string ConvertDecibelToText(float amount)
	{
		return (amount * 100f).ToString("0");
	}
	#endregion

	private void ReloadUI()
	{
		float masterVol = UserSettings.MasterVolume;
		float musicVol = UserSettings.MusicVolume;
		float soundVol = UserSettings.SoundVolume;
		float ambienceVol = UserSettings.AmbienceVolume;

		masterSlider.Value = masterVol;
		musicSlider.Value = musicVol;
		soundSlider.Value = soundVol;
		ambienceSlider.Value = ambienceVol;

		masterSlider.DisplayText = ConvertDecibelToText(masterVol);
		musicSlider.DisplayText = ConvertDecibelToText(musicVol);
		soundSlider.DisplayText = ConvertDecibelToText(soundVol);
		ambienceSlider.DisplayText = ConvertDecibelToText(ambienceVol);

		qualitySelector.Index = UserSettings.QualityLevel;
		framerateSelector.Value = UserSettings.TargetFramerate.ToString();
		vsyncSelector.Index = UserSettings.UseVsync;
		measurementUnitSelector.Index = UserSettings.MetricType;
	}
}
