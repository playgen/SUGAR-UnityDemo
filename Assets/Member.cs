using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;

public class Member : MonoBehaviour
{
	private UserFriendClient _friend;
	private GroupMemberClient _groupMember;
	public GameObject MemberItemPrefab;
	public GameObject MemberList;
	public Text StatusText;

	void Awake()
	{
		_friend = Controller.Factory.UserFriend;
		_groupMember = Controller.Factory.GroupMember;
	}

	void OnEnable()
	{
		if (Controller.GroupId.HasValue)
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
		var userFriends = _friend.GetFriends(Controller.UserId.Value);
		var groupMembers = _groupMember.GetMembers(Controller.GroupId.Value);
		var userFriendIds = new HashSet<int>(userFriends.Select(x => x.Id));
		int counter = 0;
		var listRect = MemberList.GetComponent<RectTransform>().rect;
		foreach (var member in groupMembers)
		{
			if (member.Id == Controller.UserId.Value)
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
				itemButton.GetComponentInChildren<Text>().text = "REMOVE";
				itemButton.onClick.AddListener(() => RemoveFriend(memberCopy.Id));
			}
			else
			{
				itemButton.onClick.AddListener(() => AddFriend(memberCopy.Id));
			}
		}
	}

	void AddFriend(int memberId)
	{
		var userFriend = Controller.Factory.UserFriend;
		try
		{
			// Add friend
			var relationshipResponse = userFriend.CreateFriendRequest(new RelationshipRequest()
			{
				AcceptorId = memberId,
				RequestorId = Controller.UserId.Value,
				AutoAccept = true
				
			});
			StatusText.text = "Successfully added friend!";
			UpdateMembersList();
			// Update Data for Achievement Progress
			try
			{
				// Update Achievement Progress
				Controller.SaveData(Controller.UserId.Value, "FriendsAdded", "1", GameDataType.Long);
				Controller.AchievementPanel.GetComponent<Achievement>().UpdateAchivementLists();
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


	private void RemoveFriend(int friendId)
	{
		var friend = Controller.Factory.UserFriend;
		try
		{
			friend.UpdateFriend(new RelationshipStatusUpdate()
			{
				AcceptorId = friendId,
				RequestorId = Controller.UserId.Value,
				Accepted = true
			});
			StatusText.text = "Successfully removed friend!";
			UpdateMembersList();
			try
			{
				// Update Achievement Progress
				Controller.SaveData(Controller.UserId.Value, "FriendsRemoved", "1", GameDataType.Long);
				Controller.AchievementPanel.GetComponent<Achievement>().UpdateAchivementLists();
				//Controller.NextView();
			}
			catch (Exception ex)
			{
				StatusText.text = "SaveData Fail. " + ex.Message;
			}
		}
		catch (Exception ex)
		{
			StatusText.text = "Remove Friend Failed. " + ex.Message;
		}
	}
}
