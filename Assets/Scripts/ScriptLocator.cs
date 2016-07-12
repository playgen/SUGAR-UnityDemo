using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Networking;

[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(ResourceController))]
[RequireComponent(typeof(LoginController))]
public class ScriptLocator : MonoBehaviour
{
	void Awake()
	{
		ResourceController = transform.GetComponent<ResourceController>();
		Controller = transform.GetComponent<Controller>();
		LoginController = transform.GetComponent<LoginController>();
	}

	public static ResourceController ResourceController { get; private set; }

	public static Controller Controller { get; private set; }

	public static LoginController LoginController { get; private set; }

}
