using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Ekrana kod ile cizilen sanal joystick. Parmak basip suruklenince
/// VirtualInput.MoveX / MoveY degerlerini -1..1 araliginda gunceller.
/// MobileControls tarafindan olusturulur; sahneye elle eklemen gerekmez.
/// </summary>
public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	public RectTransform knob;

	/// <summary>Joystick cubugunun merkezden en fazla kac piksel (referans cozunurlukte) sapabilecegi.</summary>
	public float radius = 90f;

	private RectTransform baseRect;
	private int activePointerId = -999;
	private bool dragging;

	private void Awake()
	{
		baseRect = GetComponent<RectTransform>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		activePointerId = eventData.pointerId;
		dragging = true;
		UpdateJoystick(eventData.position);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (dragging && eventData.pointerId == activePointerId)
		{
			UpdateJoystick(eventData.position);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.pointerId == activePointerId)
		{
			dragging = false;
			activePointerId = -999;
			VirtualInput.MoveX = 0f;
			VirtualInput.MoveY = 0f;
			if (knob != null)
			{
				knob.anchoredPosition = Vector2.zero;
			}
		}
	}

	private void UpdateJoystick(Vector2 screenPosition)
	{
		Vector2 localPoint;
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, null, out localPoint))
		{
			return;
		}
		// CanvasScaler referans cozunurlugune gore olceklenmis yerel koordinat gelir.
		Vector2 delta = localPoint;
		if (delta.magnitude > radius)
		{
			delta = delta.normalized * radius;
		}
		if (knob != null)
		{
			knob.anchoredPosition = delta;
		}
		VirtualInput.MoveX = Mathf.Clamp(delta.x / radius, -1f, 1f);
		VirtualInput.MoveY = Mathf.Clamp(delta.y / radius, -1f, 1f);
	}

	private void OnDisable()
	{
		dragging = false;
		activePointerId = -999;
		VirtualInput.MoveX = 0f;
		VirtualInput.MoveY = 0f;
		if (knob != null)
		{
			knob.anchoredPosition = Vector2.zero;
		}
	}
}
