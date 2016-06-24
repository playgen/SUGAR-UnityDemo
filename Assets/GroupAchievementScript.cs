using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI;
using PlayGen.SGA.Contracts;
using UnityEngine.UI;

public class GroupAchievementScript : MonoBehaviour
{
	private GroupAchievementClientProxy _groupAchievementProxy;
	public GameObject AchievementList;
	public GameObject AchivementItemPrefab;

	// Use this for initialization
	void Start()
	{
		_groupAchievementProxy = ControllerScript.ProxyFactory.GetGroupAchievementClientProxy;
		UpdateAchievementsList();
	}

	public void UpdateAchievementsList()
	{
		var groupId = ControllerScript.GroupId;
		if (groupId.HasValue)
		{
			UpdateList(groupId.Value);
		}
	}

	private void UpdateList(int groupId)
	{
		
		//Remove old achievemnts list
		foreach (Transform child in AchievementList.transform)
		{
			Destroy(child.gameObject);
		}
		try
		{
			var achievements = _groupAchievementProxy.GetProgress(groupId, ControllerScript.GameId);
			int counter = 0;
			var listRect = AchievementList.GetComponent<RectTransform>().rect;
			foreach (var achievement in achievements)
			{
				var achievementItem = Instantiate(AchivementItemPrefab);
				achievementItem.transform.SetParent(AchievementList.transform, false);
				var itemRectTransform = achievementItem.GetComponent<RectTransform>();
				itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 10);
				itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 10)));
				achievementItem.GetComponentInChildren<Text>().text = achievement.Name;
				if (achievement.Progress != 1.0f)
				{
					Destroy(achievementItem.transform.FindChild("Tick").gameObject);
				}
				counter++;

			}
		}
		catch (Exception ex)
		{
			Debug.Log("Failed to get achievements list. " + ex.Message);
		}
	}

	public bool SetUpGroupAchievements()
	{
		var gameId = ControllerScript.GameId;
		try
		{
			_groupAchievementProxy.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Gain 5 Members!",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = DataType.Long,
						Value = "1",
						Key = "MembersJoined",
						ComparisonType = ComparisonType.Equals
					}
				}
			});
			_groupAchievementProxy.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Loose 2 Members!",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = DataType.Long,
						Value = "2",
						Key = "MembersLeft",
						ComparisonType = ComparisonType.Equals
					}
				}
			});
			return true;
		}
		catch (Exception ex)
		{
			Debug.Log("Create Group Achievments Failed. " + ex.Message);
		}
		return false;
	}

}
