using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
	private AudioSource audioDevice;

	public AudioClip aud_Intro;

	public Image chaser;

	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
		chaser.color = new Color(1f, 1f, 1f, 0f);
	}

	public IEnumerator Intro()
	{
		Color quarter = new Color(1f, 1f, 1f, 0.25f);
		Color half = new Color(1f, 1f, 1f, 0.5f);
		Color almost = new Color(1f, 1f, 1f, 0.75f);
		audioDevice.PlayOneShot(aud_Intro);
		chaser.color = quarter;
		yield return new WaitForSeconds(0.1f);
		chaser.color = half;
		yield return new WaitForSeconds(0.1f);
		chaser.color = almost;
		yield return new WaitForSeconds(0.1f);
		chaser.color = Color.white;
		yield return new WaitForSeconds(1.5f);
		chaser.color = almost;
		yield return new WaitForSeconds(0.1f);
		chaser.color = half;
		yield return new WaitForSeconds(0.1f);
		chaser.color = quarter;
		yield return new WaitForSeconds(0.1f);
		chaser.color = new Color(1f, 1f, 1f, 0f);
	}
}
