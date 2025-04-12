using UnityEditor;
using UnityEngine;

public class PlayerPrefsHelper
{
	[MenuItem("CST Utilities/Clear Player Prefs")]
	public static void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}