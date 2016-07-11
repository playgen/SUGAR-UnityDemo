using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ScriptLocator : MonoBehaviour
{

	private static ResourceController _resourceController;

	void Start()
	{
		_resourceController = transform.GetComponent<ResourceController>();
	}

	public static ResourceController GetResourceControl()
	{
		return _resourceController;
	}
}
