using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;

public class Group : MonoBehaviour
{
	private IEnumerable<ActorResponse> _userGroups;
	private GroupMemberClient _groupMember;
	public GameObject GroupItemPrefab;
	public GameObject GroupList;
	public Text StatusText;

	// Use this for initialization
	void Awake ()
	{
		_groupMember = Controller.Factory.GroupMember;
	}

	void OnEnable()
	{
		UpdateGroupsList();
	}

	void OnDisable()
	{
		ClearList();
	}

	private void ClearList()
	{
		//Remove old friends list
		foreach (Transform child in GroupList.transform)
		{
			Destroy(child.gameObject);
		}
	}

	void UpdateGroupsList()
	{
		try
		{
			_userGroups = _groupMember.GetUserGroups(Controller.UserId.Value);
			if (_userGroups.Any())
			{
				StatusText.text = "User already in a group!";
				Controller.GroupId = _userGroups.First().Id;
			}
			UpdateGroups();
		}
		catch (Exception exception)
		{
			StatusText.text = "Failed to get user's Groups" + exception.Message;
		}
	}

	void UpdateGroups()
	{
		ClearList();
		var groupclient = Controller.Factory.Group;
		var groups = groupclient.Get();
		int counter = 0;
		var userGroupIds = new HashSet<int>(_userGroups.Select(x => x.Id));
		var listRect = GroupList.GetComponent<RectTransform>().rect;
		foreach (var group in groups)
		{
			var groupItem = Instantiate(GroupItemPrefab);
			groupItem.transform.SetParent(GroupList.transform, false);
			var itemRectTransform = groupItem.GetComponent<RectTransform>();
			itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height/8);
			itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 8)));
			groupItem.GetComponentInChildren<Text>().text = group.Name;
			counter++;
			var groupCopy = group;
			var itemButton = groupItem.GetComponentInChildren<Button>();
			if (userGroupIds.Contains(group.Id))
			{
				itemButton.GetComponentInChildren<Text>().text = "LEAVE";
				itemButton.onClick.AddListener(() => LeaveGroup(groupCopy.Id));
			}
			else
			{
				if (Controller.GroupId.HasValue)
				{
					itemButton.interactable = false;
				}
				else
				{
					itemButton.onClick.AddListener(() => JoinGroup(groupCopy.Id));
				}
			}


		}
	}

	private void LeaveGroup(int groupId)
	{
		try
		{
			_groupMember.UpdateMember(new RelationshipStatusUpdate()
			{
				AcceptorId = groupId,
				RequestorId = Controller.UserId.Value
			});
			StatusText.text = "Successfully Left the group!";
			try
			{
				// Update Achievement Progress
				Controller.SaveData(Controller.UserId.Value, "GroupsLeft", "1", GameDataType.Long);
				Controller.SaveData(groupId, "MembersLeft", "1", GameDataType.Long);			// ERRR?
				Controller.GroupId = null;
				UpdateGroupsList();
				Controller.UpdateAchievements();
			}
			catch (Exception ex)
			{
				StatusText.text = "SaveData Fail. " + ex.Message;
			}
		}
		catch (Exception ex)
		{
			StatusText.text = "Leave Group Failed. " + ex.Message;
		}
	}

	void JoinGroup(int groupId)
	{
		try
		{
			var relationshipResponse = _groupMember.CreateMemberRequest(new RelationshipRequest()
			{
				AcceptorId = groupId,
				RequestorId = Controller.UserId.Value,
				AutoAccept = true
			});
			Controller.GroupId = relationshipResponse.AcceptorId;
			StatusText.text = "Successfully Joined the group!";
			UpdateGroupsList();
			try
			{
				// Update Achievement Progress
				Controller.SaveData(Controller.UserId.Value, "GroupsJoined", "1", GameDataType.Long);	//error
				Controller.SaveData(groupId, "MembersJoined", "1", GameDataType.Long);
				Controller.UpdateAchievements();
			}
			catch (Exception ex)
			{
				StatusText.text = "SaveData Fail. " + ex.Message;
			}
		}
		catch (Exception ex)
		{
			StatusText.text = "Join Group Failed. " + ex.Message;
		}
	}
}
