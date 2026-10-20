using ControlFreak2;
using UnityEngine;

public class ClickableTest : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (CF2Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(CF2Input.mousePosition), out var hitInfo) && hitInfo.transform.name == "MathNotebook")
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
