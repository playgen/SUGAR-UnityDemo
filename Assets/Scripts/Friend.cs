using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts;

public class Friend : MonoBehaviour {

	public GameObject FriendItemPrefab;
	public GameObject FriendList;
	public Text StatusText;

	private string _defaultStatusText;

	void OnEnable()
	{
		UpdateFriendsList();
	}

	void OnDisable()
	{
		ClearList();
	}

	void Awake()
	{
		_defaultStatusText = StatusText.text;
	}

	void Reset()
	{
		StatusText.text = _defaultStatusText;
	}

	private void ClearList()
	{
		//Remove old friends list
		foreach (Transform child in FriendList.transform)
		{
			Destroy(child.gameObject);
		}
	}

	private void UpdateFriendsList()
	{
		ClearList();
		Debug.Log("UpdateFriendsList: " + ScriptLocator.ResourceController.ResourceList.transform.childCount);
		var friendclient = ScriptLocator.Controller.Factory.UserFriend;
		try
		{
			var friends = friendclient.GetFriends(ScriptLocator.Controller.UserId.Value).Where(m => m.Id > 5 && m.Id < 13);
			int counter = 0;
			var listRect = FriendList.GetComponent<RectTransform>().rect;
			foreach (var friend in friends)
			{
				var friendItem = Instantiate(FriendItemPrefab);
				friendItem.transform.SetParent(FriendList.transform, false);
				var itemRectTransform = friendItem.GetComponent<RectTransform>();
				itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 8);
				itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 8)));
				friendItem.GetComponentInChildren<Text>().text = friend.Name;
				counter++;
				var friendCopy = friend;
				friendItem.transform.Find("RemoveButton").GetComponent<Button>().onClick.AddListener(() => RemoveFriend(friendCopy.Id));
				if (ScriptLocator.ResourceController.ResourceList.transform.childCount > 0)
				{
					friendItem.transform.Find("GiftButton")
						.GetComponent<Button>()
						.onClick.AddListener(() => GiftFriend(friendCopy.Id));
				}
				else
				{
					Destroy(friendItem.transform.Find("GiftButton").gameObject);
				}
			}
		}
		catch (Exception ex)
		{
			StatusText.text = "Failed to get friends list. " + ex.Message;
		}
	}

	private void GiftFriend(int friendId)
	{
		Debug.Log("GiftFriend");
		var resourceController = ScriptLocator.ResourceController;
		if (!resourceController.TransferResource("Daily Chocolate", 1, ScriptLocator.Controller.UserId.Value, friendId))
		{
			StatusText.text = "Sending Gift Failed!";
		}
		else
		{
			StatusText.text = "Gift Sent!";
			ScriptLocator.Controller.UpdateUi();
			UpdateFriendsList();
		}
	}

	private void RemoveFriend(int friendId)
	{
		var friend = ScriptLocator.Controller.Factory.UserFriend;
		try
		{
			friend.UpdateFriend(new RelationshipStatusUpdate()
			{
				AcceptorId = friendId,
				RequestorId = ScriptLocator.Controller.UserId.Value,
				Accepted = true
			});
			StatusText.text = "Successfully removed friend!";
			UpdateFriendsList();
			try
			{
				// Update Achievement Progress
				ScriptLocator.Controller.SaveData(ScriptLocator.Controller.UserId.Value, "FriendsRemoved", "1", EvaluationDataType.Long);
				ScriptLocator.Controller.SaveData(ScriptLocator.Controller.UserId.Value, "FriendsAdded", "-1", EvaluationDataType.Long);
				ScriptLocator.Controller.UpdateUi();
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}
}
