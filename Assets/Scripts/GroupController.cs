using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Shared;

public class GroupController : MonoBehaviour
{
	public GameObject GroupItemPrefab;
	public GameObject GroupList;
	public Text StatusText;

    private IEnumerable<ActorResponse> _userGroups;
    private GroupMemberClient _groupMember;
    private string _defaultStatusText;

	// Use this for initialization
	void Awake()
	{
		_groupMember = ScriptLocator.Controller.Factory.GroupMember;
		_defaultStatusText = StatusText.text;
	}

	void Reset()
	{
		StatusText.text = _defaultStatusText;
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
		ScriptLocator.Controller.GroupId = null;

		try
		{
			_userGroups = _groupMember.GetUserGroups(ScriptLocator.Controller.UserId.Value);
			if (_userGroups.Any())
			{
				StatusText.text = "Group Successfully Joined!!";
				ScriptLocator.Controller.GroupId = _userGroups.First().Id;
			}
			UpdateGroups();
		}
		catch (Exception exception)
		{
			StatusText.text = "Failed to get user's Groups" + exception.Message;
			Debug.LogError(exception);
		}
	}

	void UpdateGroups()
	{
		ClearList();
		var groupclient = ScriptLocator.Controller.Factory.Group;
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
				ScriptLocator.Controller.UpdateUi();
				itemButton.GetComponentInChildren<Text>().text = "LEAVE";
				itemButton.onClick.AddListener(() => LeaveGroup(groupCopy.Id));
			}
			else
			{
				if (ScriptLocator.Controller.GroupId.HasValue)
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
				RequestorId = ScriptLocator.Controller.UserId.Value
			});
			StatusText.text = "Successfully Left the group!";
			try
			{
				// Update Achievement Progress
				ScriptLocator.Controller.SaveData(ScriptLocator.Controller.UserId.Value, "GroupsLeft", "1", GameDataType.Long);
				ScriptLocator.Controller.SaveData(groupId, "MembersLeft", "1", GameDataType.Long);          // ERRR?
				ScriptLocator.Controller.GroupId = null;
				UpdateGroupsList();
				ScriptLocator.Controller.UpdateUi();
			}
			catch (Exception exception)
			{
				StatusText.text = "SaveData Fail. " + exception.Message;
				Debug.LogError(exception);
			}
		}
		catch (Exception exception)
		{
			StatusText.text = "Leave Group Failed. " + exception.Message;
			Debug.LogError(exception);
		}
	}

	private void JoinGroup(int groupId)
	{
		try
		{
			var relationshipResponse = _groupMember.CreateMemberRequest(new RelationshipRequest()
			{
				AcceptorId = groupId,
				RequestorId = ScriptLocator.Controller.UserId.Value,
				AutoAccept = true
			});
			ScriptLocator.Controller.GroupId = relationshipResponse.AcceptorId;
			StatusText.text = "Successfully Joined the group!";
			UpdateGroupsList();
			try
			{
				// Update Achievement Progress
				ScriptLocator.Controller.SaveData(ScriptLocator.Controller.UserId.Value, "GroupsJoined", "1", GameDataType.Long); //error
				ScriptLocator.Controller.SaveData(groupId, "MembersJoined", "1", GameDataType.Long);
				ScriptLocator.Controller.UpdateUi();
			}
			catch (Exception exception)
			{
				StatusText.text = "SaveData Fail. " + exception.Message;
				Debug.LogError(exception);
			}
		}
		catch (Exception exception)
		{
			StatusText.text = "Join Group Failed. " + exception.Message;
			Debug.LogError(exception);
		}
	}
}
