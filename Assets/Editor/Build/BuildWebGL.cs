using UnityEngine;
using UnityEditor;

public class BuildWebGL : MonoBehaviour
{

	[MenuItem("Tools/WebGL Build")]
	static void Build()
	{
		string[] scenes = { "Assets/Scenes/setup.unity", "Assets/Scenes/demo.unity" };
		//EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebGL);
		EditorUserBuildSettings.webGLOptimizationLevel = 3;
		PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0;
		BuildPipeline.BuildPlayer(scenes, @"Build/WebGL", BuildTarget.WebGL, BuildOptions.None);
	}

}