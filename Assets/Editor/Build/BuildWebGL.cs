using UnityEngine;
using UnityEditor;

public class BuildWebGL : MonoBehaviour
{

	[MenuItem("Tools/WebGL Build")]
	static void Build()
	{
		string[] scenes = { "Assets/Scenes/setup.unity", "Assets/Scenes/demo.unity" };
		//EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebGL);
		//EditorUserBuildSettings.webGLOptimizationLevel = 3; //todo was commented for unity 5.4.2 compatability but haven't checked for replacement functionality
		PlayerSettings.apiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0;
		BuildPipeline.BuildPlayer(scenes, @"Build/WebGL", BuildTarget.WebGL, BuildOptions.None);
	}

}