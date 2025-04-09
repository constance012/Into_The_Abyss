using AYellowpaper.SerializedCollections;
using UnityEngine;

/// <summary>
/// A scriptable object for creating a set of keys use in keybinding.
/// </summary>
[CreateAssetMenu(fileName = "New Keyset", menuName = "Keybinding/Keyset")]
public class Keyset : ScriptableObject
{
	[Header("List of keys"), Space]
	public SerializedDictionary<KeybindingActions, KeyCode> keyTable = new SerializedDictionary<KeybindingActions, KeyCode>();

	public int TotalKeys => keyTable.Count;
	public int LastIndex => TotalKeys - 1;
	
	public KeyCode this[KeybindingActions action]
	{
		get { return keyTable[action]; }
		set { keyTable[action] = value; }
	}
}
