using ControlFreak2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// TAMAM ULAN ASKER - Baldi's Basics mobil kontrol paketi (GameBanana
/// tools/19220, Control Freak 2 tabanli) yoneticisi.
///
/// Artik arayuzu bu script cizmiyor. Paketin CF2-Rig prefab'i
/// (Assets/Resources/BaldiMobile/CF2-Rig.prefab) cagriliyor:
///   * Joystick  -> "Forward" / "Strafe" eksenleri
///   * TrackPad  -> "Mouse X" / "Mouse Y" (bakis)
///   * Run tusu (bacak ikonu) -> "Run" (koyu kizil yapildi)
///   * Interaction (el ikonu) -> Fire1 / sol tik (kapi, defter, esya)
///   * Mirror -> "Look Behind" (ayni tus ip atlamada ziplatir)
///   * Pause -> "Pause", Item0/1/2 -> Alpha1/2/3 (esya secimi)
/// Oyunun scriptleri (PlayerScript, GameControllerScript, kapilar...)
/// Input yerine ControlFreak2.CF2Input okudugu icin bu tuslar oyunun
/// orijinal mekaniklerine (stamina, guilt, kapi sesi vb.) dogrudan bagli.
///
/// Bu scriptin isi:
///   1) Rig'i sahneye otomatik getirmek (elle suruklemek gerekmez,
///      PlayerScript olan her sahnede gecerli, menulerde gizli),
///   2) Matematik ekraninda / duraklatmada / olumde rig'i gizlemek,
///   3) Pakette eksik olan islevler icin kucuk ek butonlar gostermek:
///      "KULLAN" (esya kullan - pakette tusu yoktu), esya ileri/geri
///      oklari (oyunda 5 esya yuvasi var, pakette 3) ve oyun durmusken
///      gorunen bir "II" devam butonu.
///
/// PC'de test: PlayerPrefs "ForceMobileControls" = 1 ise veya dokunmatik
/// kullanilirsa aktiflesir. Android/iOS'ta otomatiktir.
/// </summary>
public class MobileControls : MonoBehaviour
{
	private static MobileControls instance;

	private GameObject rigInstance;
	private Canvas extraCanvas;
	private GameObject extraPanel;
	private GameObject pausedResumeButtonObject;

	private PlayerScript player;
	private GameControllerScript gameController;
	private float nextSearchTime;
	private Sprite circleSprite;
	private Font uiFont;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Bootstrap()
	{
		if (instance != null)
		{
			return;
		}
		GameObject obj = new GameObject("MobileControls");
		instance = obj.AddComponent<MobileControls>();
		DontDestroyOnLoad(obj);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		VirtualInput.ResetAll();
		if (instance != null)
		{
			instance.player = null;
			instance.gameController = null;
			instance.nextSearchTime = 0f;
		}
	}

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if UNITY_ANDROID || UNITY_IOS
		// Baldi tarzi oyun yatay ekranda oynanir.
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
#endif
		if (PlayerPrefs.GetInt("ForceMobileControls", 0) == 1)
		{
			CF2Input.SetMobileMode(CF2Input.MobileMode.Enabled);
		}
		SpawnRig();
		BuildExtraUI();
	}

	private void SpawnRig()
	{
		if (rigInstance != null)
		{
			return;
		}
		GameObject prefab = Resources.Load<GameObject>("BaldiMobile/CF2-Rig");
		if (prefab == null)
		{
			Debug.LogWarning("[MobileControls] CF2-Rig prefab'i bulunamadi: Assets/Resources/BaldiMobile/CF2-Rig.prefab");
			return;
		}
		rigInstance = Instantiate(prefab);
		rigInstance.name = "CF2-Rig (MobileControls)";
		DontDestroyOnLoad(rigInstance);
		rigInstance.SetActive(false);
	}

	private bool ControlsShouldBeVisible()
	{
		if (Application.isMobilePlatform)
		{
			return true;
		}
		if (InputTypeManager.usingTouch)
		{
			return true;
		}
		return PlayerPrefs.GetInt("ForceMobileControls", 0) == 1;
	}

	private void Update()
	{
		bool show = ControlsShouldBeVisible();

		if (Time.unscaledTime >= nextSearchTime)
		{
			nextSearchTime = Time.unscaledTime + 0.25f;
			if (player == null)
			{
				player = FindObjectOfType<PlayerScript>();
			}
			if (gameController == null)
			{
				gameController = FindObjectOfType<GameControllerScript>();
			}
			EnsureEventSystem();
			if (rigInstance == null)
			{
				SpawnRig();
			}
		}

		bool inGameplay = (player != null);
		bool learning = (gameController != null && gameController.LearningActive);
		bool paused = (gameController != null && gameController.GamePaused);
		bool gameOver = (player != null && player.gameOver);

		bool showRig = show && inGameplay && !learning && !paused && !gameOver;
		if (rigInstance != null && rigInstance.activeSelf != showRig)
		{
			rigInstance.SetActive(showRig);
			if (!showRig)
			{
				VirtualInput.ResetAll();
			}
		}

		// Ek butonlar (KULLAN + esya oklari) rig ile ayni anda gorunur.
		if (extraPanel != null && extraPanel.activeSelf != showRig)
		{
			extraPanel.SetActive(showRig);
		}
		// Oyun durmusken rig gizli oldugu icin devam edebilmek uzere ayri "II" butonu:
		bool showResume = show && inGameplay && paused;
		if (pausedResumeButtonObject != null && pausedResumeButtonObject.activeSelf != showResume)
		{
			pausedResumeButtonObject.SetActive(showResume);
		}

		if (showRig)
		{
			InputTypeManager.usingTouch = true;
		}
	}

	private void LateUpdate()
	{
		if (ControlsShouldBeVisible() && player != null)
		{
			if (gameController == null || !gameController.LearningActive)
			{
				if (Cursor.lockState != CursorLockMode.None)
				{
					Cursor.lockState = CursorLockMode.None;
				}
			}
		}
	}

	private void EnsureEventSystem()
	{
		if (FindObjectOfType<EventSystem>() == null)
		{
			GameObject es = new GameObject("EventSystem");
			es.AddComponent<EventSystem>();
			es.AddComponent<StandaloneInputModule>();
		}
	}

	// ---------------------------------------------------------- ek butonlar

	private Sprite GetCircleSprite()
	{
		if (circleSprite != null)
		{
			return circleSprite;
		}
		int size = 128;
		Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
		Color[] pixels = new Color[size * size];
		Vector2 center = new Vector2(size / 2f, size / 2f);
		for (int y = 0; y < size; y++)
		{
			for (int x = 0; x < size; x++)
			{
				float distance = Vector2.Distance(new Vector2(x, y), center);
				float alpha = Mathf.Clamp01((size / 2f - distance) / 4f);
				pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
			}
		}
		texture.SetPixels(pixels);
		texture.Apply();
		circleSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
		return circleSprite;
	}

	private Font GetFont()
	{
		if (uiFont == null)
		{
			uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}
		return uiFont;
	}

	private GameObject CreateCircleButton(Transform parent, string label, Vector2 anchor, Vector2 anchoredPos, float size, Color color, out MobileButton button)
	{
		GameObject obj = new GameObject(label + "Button");
		obj.transform.SetParent(parent, false);
		RectTransform rect = obj.AddComponent<RectTransform>();
		rect.anchorMin = anchor;
		rect.anchorMax = anchor;
		rect.pivot = new Vector2(0.5f, 0.5f);
		rect.anchoredPosition = anchoredPos;
		rect.sizeDelta = new Vector2(size, size);

		Image image = obj.AddComponent<Image>();
		image.sprite = GetCircleSprite();
		image.color = color;
		image.raycastTarget = true;

		button = obj.AddComponent<MobileButton>();

		GameObject textObj = new GameObject("Label");
		textObj.transform.SetParent(obj.transform, false);
		RectTransform textRect = textObj.AddComponent<RectTransform>();
		textRect.anchorMin = Vector2.zero;
		textRect.anchorMax = Vector2.one;
		textRect.offsetMin = Vector2.zero;
		textRect.offsetMax = Vector2.zero;
		Text text = textObj.AddComponent<Text>();
		text.text = label;
		text.font = GetFont();
		text.fontSize = Mathf.RoundToInt(size * 0.20f);
		text.fontStyle = FontStyle.Bold;
		text.alignment = TextAnchor.MiddleCenter;
		text.color = Color.white;
		text.raycastTarget = false;
		return obj;
	}

	private void BuildExtraUI()
	{
		GameObject canvasObj = new GameObject("MobileExtraCanvas");
		canvasObj.transform.SetParent(transform, false);
		extraCanvas = canvasObj.AddComponent<Canvas>();
		extraCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		extraCanvas.sortingOrder = 101; // CF2-Rig canvas'i 100
		CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		scaler.referenceResolution = new Vector2(1280f, 720f);
		scaler.matchWidthOrHeight = 0.5f;
		canvasObj.AddComponent<GraphicRaycaster>();

		extraPanel = new GameObject("ExtraControls");
		extraPanel.transform.SetParent(canvasObj.transform, false);
		RectTransform panelRect = extraPanel.AddComponent<RectTransform>();
		panelRect.anchorMin = Vector2.zero;
		panelRect.anchorMax = Vector2.one;
		panelRect.offsetMin = Vector2.zero;
		panelRect.offsetMax = Vector2.zero;

		Color dark = new Color(0f, 0f, 0f, 0.45f);

		// Pakette olmayan: esya kullanma
		MobileButton useButton;
		CreateCircleButton(extraPanel.transform, "KULLAN", new Vector2(0.5f, 0f), new Vector2(290, 70), 105f, dark, out useButton);
		useButton.OnPress = delegate { VirtualInput.PressUseItem(); };

		// Oyunda 5 esya yuvasi var, pakette 3 tus var: ileri/geri oklari
		MobileButton prevButton;
		CreateCircleButton(extraPanel.transform, "<", new Vector2(0.5f, 0f), new Vector2(140, 70), 85f, dark, out prevButton);
		prevButton.OnPress = delegate { VirtualInput.PressPrevItem(); };

		MobileButton nextButton;
		CreateCircleButton(extraPanel.transform, ">", new Vector2(0.5f, 0f), new Vector2(440, 70), 85f, dark, out nextButton);
		nextButton.OnPress = delegate { VirtualInput.PressNextItem(); };

		// Duraklatildiginda gorunen devam (duraklat) butonu
		MobileButton resumeButton;
		pausedResumeButtonObject = CreateCircleButton(canvasObj.transform, "II", new Vector2(1f, 1f), new Vector2(-70, -70), 90f, dark, out resumeButton);
		resumeButton.OnPress = delegate { VirtualInput.PressPause(); };

		extraPanel.SetActive(false);
		pausedResumeButtonObject.SetActive(false);
	}
}
