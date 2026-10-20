using ControlFreak2;
using UnityEngine;

public class NotebookScript : MonoBehaviour
{
	public float openingDistance;

	public GameControllerScript gc;

	public BaldiScript bsc;

	public float respawnTime;

	public bool up;

	public Transform player;

	public GameObject learningGame;

	public AudioSource audioDevice;

	public AudioSource textSource;

	public TextFadeOut fadeOut;

	public IntroController introController;

	private void Start()
	{
		up = true;
	}

	private void Update()
	{
		if (gc.mode == "endless")
		{
			if (respawnTime > 0f)
			{
				if ((base.transform.position - player.position).magnitude > 60f)
				{
					respawnTime -= Time.deltaTime;
				}
			}
			else if (!up)
			{
				base.transform.position = new Vector3(base.transform.position.x, 4f, base.transform.position.z);
				up = true;
				audioDevice.Play();
			}
		}
		if ((CF2Input.GetMouseButtonDown(0) || VirtualInput.InteractPressedThisFrame) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.transform.tag == "Notebook") & (Vector3.Distance(player.position, base.transform.position) < openingDistance)))
		{
			base.transform.position = new Vector3(base.transform.position.x, -20f, base.transform.position.z);
			up = false;
			respawnTime = 120f;
			if (gc.notebooks >= 1 && gc.notebooks < 2)
			{
				introController.StartCoroutine(introController.Intro());
			}
			gc.CollectNotebook();
			GameObject obj = Object.Instantiate(learningGame);
			obj.GetComponent<MathGameScript>().gc = gc;
			obj.GetComponent<MathGameScript>().baldiScript = bsc;
			obj.GetComponent<MathGameScript>().playerPosition = player.position;
			textSource.Play();
			fadeOut.StartCoroutine(fadeOut.FadeOut());
		}
	}
}
