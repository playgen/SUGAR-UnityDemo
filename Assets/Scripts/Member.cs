using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Shared;

public class Member : MonoBehaviour
{
	public GameObject MemberItemPrefab;
	public GameObject MemberList;
	public Text StatusText;

	private UserFriendClient _friend;
	private GroupMemberClient _groupMember;
	private string _defaultStatusText;

	void Awake()
	{
		_friend = ScriptLocator.Controller.Factory.UserFriend;
		_groupMember = ScriptLocator.Controller.Factory.GroupMember;
		_defaultStatusText = StatusText.text;
	}

	private void Reset()
	{
		StatusText.text = _defaultStatusText;
	}

	void OnEnable()
	{
		if (ScriptLocator.Controller.GroupId.HasValue)
		{
			UpdateMembersList();
		}
	}

	void OnDisable()
	{
		ClearList();
	}

	private void ClearList()
	{
		//Remove old friends list
		foreach (Transform child in MemberList.transform)
		{
			Destroy(child.gameObject);
		}
	}

	void UpdateMembersList()
	{
		ClearList();
		var userFriends = _friend.GetFriends(ScriptLocator.Controller.UserId.Value);
		var groupMembers = _groupMember.GetMembers(ScriptLocator.Controller.GroupId.Value);
		var userFriendIds = new HashSet<int>(userFriends.Select(x => x.Id));
		int counter = 0;
		var listRect = MemberList.GetComponent<RectTransform>().rect;
		foreach (var member in groupMembers)
		{
			if (member.Id == ScriptLocator.Controller.UserId.Value)
			{
				// do not show self
				continue;
			}
			var memberItem = Instantiate(MemberItemPrefab);
			memberItem.transform.SetParent(MemberList.transform, false);
			var itemRectTransform = memberItem.GetComponent<RectTransform>();
			itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 8);
			itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 8)));
			memberItem.GetComponentInChildren<Text>().text = member.Name;
			counter++;
			var memberCopy = member;
			var itemButton = memberItem.GetComponentInChildren<Button>();
			if (userFriendIds.Contains(member.Id))
			{
				Destroy(itemButton.gameObject);
			}
			else
			{
				itemButton.onClick.AddListener(() => AddFriend(memberCopy.Id));
			}
		}
	}

	void AddFriend(int memberId)
	{
		var userFriend = ScriptLocator.Controller.Factory.UserFriend;
		try
		{
			// Add friend
			var relationshipResponse = userFriend.CreateFriendRequest(new RelationshipRequest()
			{
				AcceptorId = memberId,
				RequestorId = ScriptLocator.Controller.UserId.Value,
				AutoAccept = true
				
			});
			StatusText.text = "Successfully added friend!";
			UpdateMembersList();
			// Update Data for Achievement Progress
			try
			{
				// Update Achievement Progress
				ScriptLocator.Controller.SaveData(ScriptLocator.Controller.UserId.Value, "FriendsAdded", "1", GameDataType.Long);
				ScriptLocator.Controller.AchievementPanel.GetComponent<Achievement>().UpdateAchivementLists();
				ScriptLocator.Controller.UpdateUi();
			}
			catch (Exception ex)
			{
				StatusText.text = "SaveData Fail. " + ex.Message;
			}

		}
		catch (Exception ex)
		{
			StatusText.text = "Failed to add friend. " + ex.Message;
		}
	}
}
