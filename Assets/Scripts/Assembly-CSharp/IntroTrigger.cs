using ControlFreak2;
using UnityEngine;

public class IntroTrigger : MonoBehaviour
{
	public IntroController introController;

	public GameControllerScript gc;

	private void Start()
	{
	}

	private void Update()
	{
		if (CF2Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(CF2Input.mousePosition), out var hitInfo) && hitInfo.transform.name == "Notebook" && gc.notebooks >= 1 && gc.notebooks < 3)
		{
			introController.StartCoroutine(introController.Intro());
		}
	}
}
