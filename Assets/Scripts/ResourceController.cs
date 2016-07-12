using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
	private static ResourceClient _resourceClient;

	public GameObject ResourcePanel;
	public GameObject ResourceItemPrefab;
	public GameObject ResourceList;


	void Start ()
	{
		_resourceClient = Controller.Factory.Resource;
	}

	public void AddResource(string key, int quantity, int userId)
	{
		try
		{
			_resourceClient.AddOrUpdate(new ResourceAddRequest()
			{
				GameId = Controller.GameId,
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
		try
		{
			_resourceClient.Transfer(new ResourceTransferRequest()
			{
				GameId = Controller.GameId,
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
		//Remove old achievemnts list
		foreach (Transform child in ResourceList.transform)
		{
			Destroy(child.gameObject);
		}
		try
		{
			var resources = _resourceClient.Get(Controller.GameId, Controller.UserId.Value, null);
			int counter = 0;
			var listRect = ResourceList.GetComponent<RectTransform>().rect;
			foreach (var resource in resources)
			{
				if (resource.Quantity > 0)
				{
					var resourceItem = Instantiate(ResourceItemPrefab);
					resourceItem.transform.SetParent(ResourceList.transform, false);
					var itemRectTransform = resourceItem.GetComponent<RectTransform>();
					itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 4);
					itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 4)));
					resourceItem.transform.FindChild("Name").GetComponent<Text>().text = resource.Key;
					resourceItem.transform.FindChild("Quantity").GetComponent<Text>().text = resource.Quantity.ToString();
					counter++;
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Update Resource List Failed: " + exception);
		}
	}

}


