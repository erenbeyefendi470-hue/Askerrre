using TMPro;
using UnityEngine;

public class DistanceScript : MonoBehaviour
{
	public TMP_Text distanceText;

	public Transform playerTransform;

	public Transform baldiTransform;

	private GameControllerScript gc;

	private void Start()
	{
		gc = Object.FindObjectOfType<GameControllerScript>();
	}

	private void Update()
	{
		float num = Vector3.Distance(playerTransform.position, baldiTransform.position);
		if (distanceText != null && playerTransform != null && baldiTransform != null && gc.spoopMode)
		{
			if (num > 100f)
			{
				distanceText.color = Color.green;
			}
			else if (num > 50f && num <= 100f)
			{
				distanceText.color = Color.yellow;
			}
			else if (num < 50f)
			{
				distanceText.color = Color.red;
			}
			distanceText.text = "KELBACAK SENDEN " + num.ToString("F0") + " METRE UZAK";
		}
	}
}
