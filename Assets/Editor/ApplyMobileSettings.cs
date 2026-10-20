using UnityEditor;
using UnityEngine;

/// <summary>
/// Mobil surum ayarlarini tek tikla uygular:
///   * Uygulama ikonu (Assets/AppIcon/AppIcon_512.png)
///   * Paket adi (applicationIdentifier): com.vacantguy.tamamulanasker
///   * Sirket adi: Vacant Guy, surum 1.1.0, Android min API 21
///   * Ekran yonu: yatay (Landscape)
///
/// Kullanim: Unity'de ust menuden
///   Tamam Ulan Asker > Mobil Ayarlari Uygula (Ikon + Paket Adi)
/// Ikon dosyalari zaten ProjectSettings'e elle de atanabilir; bu script
/// o isi otomatik yapar. Build almadan once bir kez calistirman yeterli.
/// </summary>
public static class ApplyMobileSettings
{
	private const string PackageId = "com.vacantguy.tamamulanasker";
	private const string IconPath = "Assets/AppIcon/AppIcon_512.png";

	[MenuItem("Tamam Ulan Asker/Mobil Ayarlari Uygula (Ikon + Paket Adi)")]
	public static void Apply()
	{
		PlayerSettings.companyName = "Vacant Guy";
		PlayerSettings.productName = "TAMAM ULAN ASKER";
		PlayerSettings.bundleVersion = "1.1.0";
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, PackageId);
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, PackageId);
		PlayerSettings.Android.bundleVersionCode = 2;
		PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;

		PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
		PlayerSettings.allowedAutorotateToPortrait = false;
		PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
		PlayerSettings.allowedAutorotateToLandscapeLeft = true;
		PlayerSettings.allowedAutorotateToLandscapeRight = true;

		Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath);
		if (icon != null)
		{
			ApplyIcons(BuildTargetGroup.Android, icon);
			ApplyIcons(BuildTargetGroup.iOS, icon);
			ApplyIcons(BuildTargetGroup.Standalone, icon);
			Debug.Log("[TamamUlanAsker] Ikon uygulandi: " + IconPath);
		}
		else
		{
			Debug.LogWarning("[TamamUlanAsker] Ikon bulunamadi: " + IconPath + " - Player Settings > Icon'dan elle atayin.");
		}

		AssetDatabase.SaveAssets();
		Debug.Log("[TamamUlanAsker] Mobil ayarlar uygulandi. Paket: " + PackageId);
	}

	private static void ApplyIcons(BuildTargetGroup group, Texture2D icon)
	{
		int[] sizes = PlayerSettings.GetIconSizesForTargetGroup(group);
		if (sizes == null || sizes.Length == 0)
		{
			return;
		}
		Texture2D[] textures = new Texture2D[sizes.Length];
		for (int i = 0; i < sizes.Length; i++)
		{
			textures[i] = icon;
		}
		PlayerSettings.SetIconsForTargetGroup(group, textures);
	}
}
