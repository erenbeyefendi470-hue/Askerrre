using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Tek tikla / tek komutla Android APK uretir.
///
/// Editor icinden:  Tamam Ulan Asker > APK Build Al
/// Komut satirindan (PC'de BUILD_APK.bat bunu cagirir):
///   Unity.exe -quit -batchmode -projectPath &lt;proje&gt; -executeMethod BuildAndroid.BuildAPKCommandLine
///
/// Cikti: &lt;proje&gt;/Builds/TamamUlanAsker.apk
/// On kosul: Unity Hub'dan 2019.4.15f1 icin Android Build Support +
/// Android SDK &amp; NDK Tools + OpenJDK modullerinin kurulu olmasi.
/// API key / lisans anahtari gerekmez; Unity Personal (ucretsiz) yeterlidir.
/// </summary>
public static class BuildAndroid
{
	private const string ApkName = "TamamUlanAsker.apk";

	[MenuItem("Tamam Ulan Asker/APK Build Al")]
	public static void BuildAPKMenu()
	{
		string output = BuildAPK();
		if (!string.IsNullOrEmpty(output))
		{
			EditorUtility.RevealInFinder(output);
		}
	}

	public static void BuildAPKCommandLine()
	{
		string output = BuildAPK();
		if (string.IsNullOrEmpty(output))
		{
			EditorApplication.Exit(1);
		}
	}

	/// <summary>GitHub Actions (GameCI) icin: cikis kodunu GameCI yonetir.</summary>
	public static void BuildForCI()
	{
		string output = BuildAPK();
		if (string.IsNullOrEmpty(output))
		{
			throw new System.Exception("[TamamUlanAsker] CI APK build basarisiz oldu.");
		}
	}

	private static string BuildAPK()
	{
		// Ikon + paket adi + yatay ekran ayarlari
		ApplyMobileSettings.Apply();

		if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
		{
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
		}

		string[] scenes = GetEnabledScenes();
		if (scenes.Length == 0)
		{
			Debug.LogError("[TamamUlanAsker] Build Settings'te aktif sahne yok!");
			return null;
		}

		string projectRoot = Directory.GetParent(Application.dataPath).FullName;
		string outputDir = Path.Combine(projectRoot, "Builds");
		Directory.CreateDirectory(outputDir);
		string outputPath = Path.Combine(outputDir, ApkName);

		PlayerSettings.Android.useAPKExpansionFiles = false;
		EditorUserBuildSettings.buildAppBundle = false; // Google Play icin true yap (AAB)

		BuildPlayerOptions options = new BuildPlayerOptions
		{
			scenes = scenes,
			locationPathName = outputPath,
			target = BuildTarget.Android,
			targetGroup = BuildTargetGroup.Android,
			options = BuildOptions.None
		};

		Debug.Log("[TamamUlanAsker] APK build basliyor: " + outputPath);
		UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(options);
		if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
		{
			Debug.Log("[TamamUlanAsker] APK hazir: " + outputPath + " (" + report.summary.totalSize + " byte)");
			return outputPath;
		}

		Debug.LogError("[TamamUlanAsker] Build basarisiz: " + report.summary.result);
		return null;
	}

	private static string[] GetEnabledScenes()
	{
		System.Collections.Generic.List<string> scenes = new System.Collections.Generic.List<string>();
		foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if (scene.enabled && !string.IsNullOrEmpty(scene.path))
			{
				scenes.Add(scene.path);
			}
		}
		return scenes.ToArray();
	}
}
