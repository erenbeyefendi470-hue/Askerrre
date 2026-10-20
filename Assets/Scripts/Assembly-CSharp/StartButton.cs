using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
	public enum Mode
	{
		Story = 0,
		Endless = 1
	}

	public string TargetScene;

	public Mode currentMode;

	public void StartGame()
	{
		PlayerPrefs.SetString("CurrentMode", "story");
		SceneManager.LoadSceneAsync(TargetScene);
	}
}
