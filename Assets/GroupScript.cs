using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI;
using PlayGen.SGA.Contracts;

public class GroupScript : MonoBehaviour
{
    private IEnumerable<ActorResponse> _userGroups;
    private GroupMemberClientProxy _groupMemberProxy;
    public GameObject GroupItemPrefab;
    public GameObject GroupList;
    public Text StatusText;

    // Use this for initialization
    void Awake ()
    {
        _groupMemberProxy = ControllerScript.ProxyFactory.GetGroupMemberClientProxy;
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
            _userGroups = _groupMemberProxy.GetUserGroups(ControllerScript.UserId.Value);
            if (_userGroups.Any())
            {
                StatusText.text = "User already in a group!";
                ControllerScript.GroupId = _userGroups.First().Id;
            }
            UpdateGroups();
        }
        catch (Exception ex)
        {
            StatusText.text = "Failed to get user's Groups" + ex.Message;
        }
    }

    void UpdateGroups()
    {
        ClearList();
        var groupProxy = ControllerScript.ProxyFactory.GetGroupClientProxy;
        var groups = groupProxy.Get();
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
                if (ControllerScript.GroupId.HasValue)
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
            _groupMemberProxy.UpdateMember(new RelationshipStatusUpdate()
            {
                AcceptorId = groupId,
                RequestorId = ControllerScript.UserId.Value
            });
            StatusText.text = "Successfully Left the group!";
            ControllerScript.GroupId = null;
            UpdateGroupsList();
            try
            {
                // Update Achievement Progress
                ControllerScript.SaveData("GroupsLeft", "1", DataType.Long);
                ControllerScript.AchievementPanel.GetComponent<AchievementScript>().UpdateAchievementsList();
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
            var relationshipResponse = _groupMemberProxy.CreateMemberRequest(new RelationshipRequest()
            {
                AcceptorId = groupId,
                RequestorId = ControllerScript.UserId.Value,
                AutoAccept = true
            });
            ControllerScript.GroupId = relationshipResponse.AcceptorId;
            StatusText.text = "Successfully Joined the group!";
            UpdateGroupsList();
            try
            {
                // Update Achievement Progress
                ControllerScript.SaveData("GroupsJoined", "1", DataType.Long);
                ControllerScript.AchievementPanel.GetComponent<AchievementScript>().UpdateAchievementsList();
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
