using UnityEngine;
using UnityEditor;

public class BuildWebGL : MonoBehaviour
{

	[MenuItem("Tools/WebGL Build")]
	static void Build()
	{
		string[] scenes = { "Assets/Scenes/setup.unity", "Assets/Scenes/demo.unity" };

		//EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebGL);
#if UNITY_5_6
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WebGL | BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_2_0);
#else
		PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0;
#endif
		BuildPipeline.BuildPlayer(scenes, @"Build/WebGL", BuildTarget.WebGL, BuildOptions.None);
	}

}