using UnityEditor;

public class Build
{
    [MenuItem("Build/WebGL")]
    public static void BuildWebGL()
    { 
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
    }
}
