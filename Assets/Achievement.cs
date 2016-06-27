using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
	private UserAchievementClient _userAchievement;
	public GameObject AchievementList;
	public GameObject AchivementItemPrefab;

	void OnEnable () {
		if (_userAchievement == null) {
			_userAchievement = Controller.Factory.GetUserAchievementClient;
		}
		UpdateAchievementsList();
	}

	public bool SetUpAchievements()
	{
		if (_userAchievement == null)
		{
			_userAchievement = Controller.Factory.GetUserAchievementClient;
		}
		var gameId = Controller.GameId;
		try
		{
			_userAchievement.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Join a Group!",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataValueType.Long,
						Value = "1",
						Key = "GroupsJoined",
						ComparisonType = ComparisonType.GreaterOrEqual
					}
				}
			});
			_userAchievement.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Add 2 Friends!",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataValueType.Long,
						Value = "2",
						Key = "FriendsAdded",
						ComparisonType = ComparisonType.GreaterOrEqual
					}
				}
			});
			_userAchievement.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Remove a Friend!",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataValueType.Long,
						Value = "1",
						Key = "FriendsRemoved",
						ComparisonType = ComparisonType.GreaterOrEqual
					}
				}
			});
			return true;
		}
		catch (Exception ex)
		{
			Debug.Log("Create Achievments Failed. " + ex.Message);
		}
		return false;
	}

	public void UpdateAchievementsList()
	{
		//Remove old achievemnts list
		foreach (Transform child in AchievementList.transform)
		{
			Destroy(child.gameObject);
		}
		try
		{
			var achievements = _userAchievement.GetProgress(Controller.UserId.Value, Controller.GameId);
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
}
