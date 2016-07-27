using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class ConfigLoader : MonoBehaviour
{
	private string ConfigPath
	{
		get
		{
			string path = Application.streamingAssetsPath + "/config.json";
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			path = "file:///" + path;
#endif
			return path;
		}
	}

	private void Start ()
	{
		StartCoroutine(LoadOnlineConfig());
	}

	private IEnumerator LoadOnlineConfig()
	{
		var www = new WWW(ConfigPath);
		yield return www;

		var config = JsonConvert.DeserializeObject<Config>(www.text);
		ScriptLocator.Config = config;

		SceneManager.LoadScene(1);
	}
}
