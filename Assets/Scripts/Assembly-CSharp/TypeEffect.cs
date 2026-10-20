using System.Collections;
using TMPro;
using UnityEngine;

public class TypeEffect : MonoBehaviour
{
	[SerializeField]
	private TMP_Text textComponent;

	[SerializeField]
	private float timeBetweenChars = 0.04f;

	[TextArea(5, 10)]
	[SerializeField]
	private string[] errorTexts;

	private void Start()
	{
		if (textComponent == null)
		{
			textComponent = GetComponent<TMP_Text>();
		}
		StartCoroutine(TypeAllTextsRoutine());
	}

	private IEnumerator TypeAllTextsRoutine()
	{
		for (int textIndex = 0; textIndex < errorTexts.Length; textIndex++)
		{
			string currentText = errorTexts[textIndex];
			textComponent.text = currentText;
			textComponent.maxVisibleCharacters = 0;
			int totalVisibleCharacters = currentText.Length;
			for (int i = 0; i <= totalVisibleCharacters; i++)
			{
				textComponent.maxVisibleCharacters = i;
				if (i < currentText.Length && !char.IsWhiteSpace(currentText[i]))
				{
					yield return new WaitForSeconds(timeBetweenChars);
				}
			}
			if (textIndex < errorTexts.Length - 1)
			{
				yield return new WaitForSeconds(3f);
			}
		}
		yield return new WaitForSeconds(1f);
		Application.Quit();
	}
}
