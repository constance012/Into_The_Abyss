using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class EditorScenesHelper
{
	private enum Scene
	{
		MainMenu,
		Gameplay
	}
	
	private static readonly Dictionary<Scene, string> _scenePaths = new Dictionary<Scene, string>()
	{
		[Scene.MainMenu] = "Assets/Scenes/Main Menu.unity",
		[Scene.Gameplay] = "Assets/Scenes/Main Gameplay.unity",
	};

	[MenuItem("CST Utilities/Open Scenes/Main Menu")]
	public static void OpenMainMenuScene()
	{
		EditorSceneManager.OpenScene(_scenePaths[Scene.MainMenu]);
	}

	[MenuItem("CST Utilities/Open Scenes/Gameplay")]
	public static void OpenGameplayScene()
	{
		EditorSceneManager.OpenScene(_scenePaths[Scene.Gameplay]);
	}
}