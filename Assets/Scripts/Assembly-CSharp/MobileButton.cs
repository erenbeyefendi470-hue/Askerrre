using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Mobil kontrol butonlarinin ortak davranisi. MobileControls tarafindan
/// kod ile olusturulur. Iki tur kullanim destekler:
///  - OnPress : parmak basinca bir kez cagrilir (ETKILES, KULLAN, DURAKLAT, ZIPLA, esya degistir)
///  - OnHold  : basili tutuldugu surece true doner (KOS, ARKAYA BAK)
/// </summary>
public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	public Action OnPress;
	public Action<bool> OnHoldChanged;

	private bool held;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (OnPress != null)
		{
			OnPress();
		}
		if (!held)
		{
			held = true;
			if (OnHoldChanged != null)
			{
				OnHoldChanged(true);
			}
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Release();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Release();
	}

	private void OnDisable()
	{
		Release();
	}

	private void Release()
	{
		if (held)
		{
			held = false;
			if (OnHoldChanged != null)
			{
				OnHoldChanged(false);
			}
		}
	}
}
