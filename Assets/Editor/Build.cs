// C# example.
using UnityEditor;
using System.Diagnostics;

public class BuildScript
{
	[MenuItem( "Build Tools/Build OSX" )]
	public static void Osx()
	{
		PlayerSettings.SetScriptingBackend( BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x );

		// Get filename.
		string[] levels = new string[] { "Assets/Main.unity" };

		// Build player.
		BuildPipeline.BuildPlayer( levels, "M:/Desktop/SteamworksTest.app", BuildTarget.StandaloneOSX, BuildOptions.Development | BuildOptions.UncompressedAssetBundle );
	}

	[MenuItem( "Build Tools/Build Linux" )]
	public static void Linux()
	{
		PlayerSettings.SetScriptingBackend( BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x );

		// Get filename.
		string[] levels = new string[] { "Assets/Main.unity" };

		// Build player.
		BuildPipeline.BuildPlayer( levels, "L:/SteamworksTest64", BuildTarget.StandaloneLinux64, BuildOptions.Development | BuildOptions.UncompressedAssetBundle );
	}

	[MenuItem( "Build Tools/Build Linux 32" )]
	public static void Linux32()
	{
		PlayerSettings.SetScriptingBackend( BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x );

		// Get filename.
		string[] levels = new string[] { "Assets/Main.unity" };

		// Build player.
		BuildPipeline.BuildPlayer( levels, "L:/SteamworksTest32", BuildTarget.StandaloneLinux, BuildOptions.Development | BuildOptions.UncompressedAssetBundle );
	}

	[MenuItem( "Build Tools/Build Windows" )]
	public static void Windows()
	{
		PlayerSettings.SetScriptingBackend( BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x );

		// Get filename.
		string[] levels = new string[] { "Assets/Main.unity" };

		// Build player.
		BuildPipeline.BuildPlayer( levels, "C:/temp/SteamworksTest64/SteamworksTest64.exe", BuildTarget.StandaloneWindows64, BuildOptions.Development | BuildOptions.UncompressedAssetBundle );
	}

	[MenuItem( "Build Tools/Build Windows 32" )]
	public static void Windows32()
	{
		PlayerSettings.SetScriptingBackend( BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x );

		// Get filename.
		string[] levels = new string[] { "Assets/Main.unity" };

		// Build player.
		BuildPipeline.BuildPlayer( levels, "C:/temp/SteamworksTest32/SteamworksTest32.exe", BuildTarget.StandaloneWindows, BuildOptions.Development | BuildOptions.UncompressedAssetBundle );
	}

	[MenuItem( "Build Tools/Build Windows IL2CPP" )]
	public static void WindowsIL2CPP()
	{
		PlayerSettings.SetScriptingBackend( BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP );

		// Get filename.
		string[] levels = new string[] { "Assets/Main.unity" };


		// Build player.
		BuildPipeline.BuildPlayer( levels, "C:/temp/SteamworksTestIl2CPP/SteamworksTestIL2CPP.exe", BuildTarget.StandaloneWindows64, BuildOptions.Development | BuildOptions.UncompressedAssetBundle );


		PlayerSettings.SetScriptingBackend( BuildTargetGroup.Standalone, ScriptingImplementation.WinRTDotNET );
	}

	[MenuItem( "Build Tools/All" )]
	public static void All()
	{
		Osx();
		Linux();
		Linux32();
		Windows();
		Windows32();
		WindowsIL2CPP();
	}
}