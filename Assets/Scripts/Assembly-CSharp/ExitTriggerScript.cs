using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTriggerScript : MonoBehaviour
{
	public GameControllerScript gc;

	private void OnTriggerEnter(Collider other)
	{
		if ((gc.notebooks >= 20) & (other.tag == "Player"))
		{
			if (gc.failedNotebooks >= 20)
			{
				SceneManager.LoadScene("Results");
			}
			else
			{
				SceneManager.LoadScene("Results");
			}
		}
	}
}
