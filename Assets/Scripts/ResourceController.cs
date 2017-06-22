using System;
using System.Linq;
using UnityEngine;
using System.Net;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
	private ResourceClient _resourceClient;

	public GameObject ResourcePanel;
	public GameObject ResourceItemPrefab;
	public GameObject ResourceList;


	void Start ()
	{
		_resourceClient = ScriptLocator.Controller.Factory.Resource;
	}

	public void AddResource(string key, int quantity, int userId)
	{
		try
		{
			_resourceClient.AddOrUpdate(new ResourceAddRequest()
			{
				GameId = ScriptLocator.Controller.GameId,
				ActorId = userId,
				Key = key,
				Quantity = quantity
			});
			UpdateList();
		}
		catch (Exception exception)
		{
			Debug.LogError("AddResource Failed: " + exception);
		}
	}

	public bool TransferResource(string key, int quantity, int userId, int targetId)
	{
		Debug.Log("TransferResource");

		try
		{
			_resourceClient.Transfer(new ResourceTransferRequest()
			{
				GameId = ScriptLocator.Controller.GameId,
				SenderActorId = userId,
				RecipientActorId = targetId,
				Key = key,
				Quantity = quantity,
			});
			return true;
		}
		catch (WebException exception)
		{
			Debug.LogError("TransferResource Failed: " + exception);
		}
		return false;
	}

	public void UpdateList()
	{
		Debug.Log("UpdateResourceList");

		//Remove old achievemnts list
		foreach (Transform child in ResourceList.transform)
		{
			// hackey fix
			child.SetParent(null);
			Destroy(child.gameObject);
		}

		try
		{
			var resources = _resourceClient.Get(ScriptLocator.Controller.GameId, ScriptLocator.Controller.UserId.Value, null);
			Debug.Log("UpdateResourceList: " + resources.Count());
			int counter = 0;
			var listRect = ResourceList.GetComponent<RectTransform>().rect;
			foreach (var resource in resources)
			{
				if (resource.Quantity > 0)
				{
					var resourceItem = Instantiate(ResourceItemPrefab);
					resourceItem.transform.SetParent(ResourceList.transform, false);
					var itemRectTransform = resourceItem.GetComponent<RectTransform>();
					itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 2);
					itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 2)));
					resourceItem.transform.Find("Name").GetComponent<Text>().text = resource.Key;
					resourceItem.transform.Find("Quantity").GetComponent<Text>().text = resource.Quantity.ToString();
					counter++;
				}
			}
			Debug.Log("ListRect: " + ResourceList.transform.childCount);

		}
		catch (Exception exception)
		{
			Debug.LogError("Update Resource List Failed: " + exception);
		}
	}
}


