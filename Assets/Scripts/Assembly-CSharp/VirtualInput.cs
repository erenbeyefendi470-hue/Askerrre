using UnityEngine;

/// <summary>
/// Mobil (dokunmatik) kontroller ile PC kontrolleri arasindaki kopru.
/// MobileControls bu statik degerleri doldurur, oyunun mevcut scriptleri
/// (PlayerScript, GameControllerScript, CameraScript, kapilar, defterler...)
/// de Input.* cagrilarinin yaninda bunlari okur. Boylece klavye/fare
/// destegi hic bozulmadan oyuna dokunmatik destegi eklenir.
///
/// Buton olaylari (Interact/UseItem/Pause/Jump/ItemNext/ItemPrev) kasitli
/// olarak "bir sonraki frame"de gecerli olur: UI dokunmasi hangi sirayla
/// islenirse islensin, oyunun butun scriptleri olayi ayni frame icinde
/// ve yalnizca bir frame boyunca gorur. (Ornegin defterin ayni frame'de
/// iki kez toplanmasi gibi sorunlar yasanmaz.)
/// </summary>
public static class VirtualInput
{
	/// <summary>Sanal joystick: -1..1. X = saga/sola (Strafe), Y = ileri/geri (Forward).</summary>
	public static float MoveX;
	public static float MoveY;

	/// <summary>KOŞ butonu basili tutuluyor mu (PC: Left Shift).</summary>
	public static bool RunHeld;

	/// <summary>ARKAYA BAK butonu basili tutuluyor mu (PC: Space - basili tutma).</summary>
	public static bool LookBehindHeld;

	private static float lookDeltaX;

	private static int interactFrame = -100;
	private static int useItemFrame = -100;
	private static int pauseFrame = -100;
	private static int jumpFrame = -100;
	private static int nextItemFrame = -100;
	private static int prevItemFrame = -100;

	// ---- Dokunmatik bakis (sag yari surukleme) ----

	public static void AddLookDelta(float pixels)
	{
		lookDeltaX += pixels;
	}

	/// <summary>Birikmis bakis hareketini okur ve sifirlar. Her frame en fazla bir kez cagrilmali.</summary>
	public static float ConsumeLookDeltaX()
	{
		float value = lookDeltaX;
		lookDeltaX = 0f;
		return value;
	}

	// ---- Buton olaylari (MobileControls cagirir) ----

	public static void PressInteract() { interactFrame = Time.frameCount + 1; }
	public static void PressUseItem() { useItemFrame = Time.frameCount + 1; }
	public static void PressPause() { pauseFrame = Time.frameCount + 1; }
	public static void PressJump() { jumpFrame = Time.frameCount + 1; }
	public static void PressNextItem() { nextItemFrame = Time.frameCount + 1; }
	public static void PressPrevItem() { prevItemFrame = Time.frameCount + 1; }

	// ---- Oyun scriptleri okur ----

	/// <summary>ETKILES butonu bu frame'de devreye girdi mi (PC: Sol fare tiki - ekranin ortasina raycast).</summary>
	public static bool InteractPressedThisFrame { get { return interactFrame == Time.frameCount; } }

	/// <summary>ESYA KULLAN butonu (PC: Sag fare tiki).</summary>
	public static bool UseItemPressedThisFrame { get { return useItemFrame == Time.frameCount; } }

	/// <summary>DURAKLAT butonu (PC: Esc / P).</summary>
	public static bool PausePressedThisFrame { get { return pauseFrame == Time.frameCount; } }

	/// <summary>ZIPLA butonu (PC: Space - ip atlama mini oyunu).</summary>
	public static bool JumpPressedThisFrame { get { return jumpFrame == Time.frameCount; } }

	public static bool NextItemPressedThisFrame { get { return nextItemFrame == Time.frameCount; } }
	public static bool PrevItemPressedThisFrame { get { return prevItemFrame == Time.frameCount; } }

	/// <summary>Sahne degisiminde / oyuncu olumunde yarim kalmis girdileri temizler.</summary>
	public static void ResetAll()
	{
		MoveX = 0f;
		MoveY = 0f;
		RunHeld = false;
		LookBehindHeld = false;
		lookDeltaX = 0f;
		interactFrame = -100;
		useItemFrame = -100;
		pauseFrame = -100;
		jumpFrame = -100;
		nextItemFrame = -100;
		prevItemFrame = -100;
	}
}
