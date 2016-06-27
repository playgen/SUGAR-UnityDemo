using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class GroupAchievement : MonoBehaviour
{
	private GroupAchievementClient _groupAchievement;
	public GameObject AchievementList;
	public GameObject AchivementItemPrefab;

	// Use this for initialization
	void OnEnable()
	{
		if (_groupAchievement == null)
		{
			_groupAchievement = Controller.Factory.GetGroupAchievementClient;
		}
		UpdateAchievementsList();
	}

	public void UpdateAchievementsList()
	{
		var groupId = Controller.GroupId;
		if (groupId.HasValue)
		{
			UpdateList(groupId.Value);
		}
	}

	public void UpdateList(int groupId)
	{
		
		//Remove old achievemnts list
		foreach (Transform child in AchievementList.transform)
		{
			Destroy(child.gameObject);
		}
		try
		{
			var achievements = _groupAchievement.GetProgress(groupId, Controller.GameId);
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
		if (_groupAchievement == null)
		{
			_groupAchievement = Controller.Factory.GetGroupAchievementClient;
		}
		var gameId = Controller.GameId;
		try
		{
			_groupAchievement.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Gain 5 Members!",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataValueType.Long,
						Value = "1",
						Key = "MembersJoined",
						ComparisonType = ComparisonType.GreaterOrEqual
					}
				}
			});
			_groupAchievement.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Lose 2 Members!",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataValueType.Long,
						Value = "2",
						Key = "MembersLeft",
						ComparisonType = ComparisonType.GreaterOrEqual
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
