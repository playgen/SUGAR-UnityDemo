﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
	private AchievementClient _achievementClient;
	public GameObject AchievementList;
	public GameObject AchivementItemPrefab;
	public GameObject GroupAchievementList;

	void OnEnable () {
		UpdateAchivementLists();
	}

	public void UpdateAchivementLists()
	{
		if (_achievementClient == null)
		{
			_achievementClient = Controller.Factory.Achievement;
		}
		var userId = Controller.UserId;
		if (userId.HasValue)
		{
			UpdateAchievementsList(AchievementList, userId.Value);
		}
		var groupId = Controller.GroupId;
		if (groupId.HasValue)
		{
			UpdateAchievementsList(GroupAchievementList, groupId.Value);
		}
	}

	private void UpdateAchievementsList(GameObject listObject, int actorId)
	{
		//Remove old achievemnts list
		foreach (Transform child in listObject.transform)
		{
			Destroy(child.gameObject);
		}
		try
		{
			var achievements = _achievementClient.GetGameProgress(Controller.GameId, actorId);
			int counter = 0;
			var listRect = listObject.GetComponent<RectTransform>().rect;
			foreach (var achievement in achievements)
			{
				var achievementItem = Instantiate(AchivementItemPrefab);
				achievementItem.transform.SetParent(listObject.transform, false);
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

	//public void UpdateAchievementsList()
	//{
	//	//Remove old achievemnts list
	//	foreach (Transform child in AchievementList.transform)
	//	{
	//		Destroy(child.gameObject);
	//	}
	//	try
	//	{
	//		var achievements = _achievementClient.GetGameProgress(Controller.GameId, Controller.UserId.Value);
	//		int counter = 0;
	//		var listRect = AchievementList.GetComponent<RectTransform>().rect;
	//		foreach (var achievement in achievements)
	//		{
	//			var achievementItem = Instantiate(AchivementItemPrefab);
	//			achievementItem.transform.SetParent(AchievementList.transform, false);
	//			var itemRectTransform = achievementItem.GetComponent<RectTransform>();
	//			itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 10);
	//			itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 10)));
	//			achievementItem.GetComponentInChildren<Text>().text = achievement.Name;
	//			if (achievement.Progress != 1.0f)
	//			{
	//				Destroy(achievementItem.transform.FindChild("Tick").gameObject);
	//			}
	//			counter++;
				
	//		}
	//	}
	//	catch (Exception ex)
	//	{
	//		Debug.Log("Failed to get achievements list. " + ex.Message);
	//	}
	//}

	//public void UpdateGroupAchievementsList(int groupId)
	//{

	//	//Remove old achievemnts list
	//	foreach (Transform child in GroupAchievementList.transform)
	//	{
	//		Destroy(child.gameObject);
	//	}
	//	try
	//	{
	//		var achievements = _achievementClient.GetGameProgress(Controller.GameId, groupId);
	//		int counter = 0;
	//		var listRect = AchievementList.GetComponent<RectTransform>().rect;
	//		foreach (var achievement in achievements)
	//		{
	//			var achievementItem = Instantiate(AchivementItemPrefab);
	//			achievementItem.transform.SetParent(AchievementList.transform, false);
	//			var itemRectTransform = achievementItem.GetComponent<RectTransform>();
	//			itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 10);
	//			itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 10)));
	//			achievementItem.GetComponentInChildren<Text>().text = achievement.Name;
	//			if (achievement.Progress != 1.0f)
	//			{
	//				Destroy(achievementItem.transform.FindChild("Tick").gameObject);
	//			}
	//			counter++;

	//		}
	//	}
	//	catch (Exception ex)
	//	{
	//		Debug.Log("Failed to get achievements list. " + ex.Message);
	//	}
	//}

	public bool SetUpAchievements()
	{
		if (_achievementClient == null)
		{
			_achievementClient = Controller.Factory.Achievement;
		}
		var gameId = Controller.GameId;
		try
		{
			_achievementClient.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Join a Group!",
				ActorType = ActorType.User,
				Token = "join_group",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataType.Long,
						Value = "1",
						Key = "GroupsJoined",
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor

					}
				}
			});
			_achievementClient.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Add 2 Friends!",
				ActorType = ActorType.User,
				Token = "add_2_friends",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataType.Long,
						Value = "2",
						Key = "FriendsAdded",
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor
					}
				}
			});
			_achievementClient.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Remove a Friend!",
				ActorType = ActorType.User,
				Token = "remove_friend",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataType.Long,
						Value = "1",
						Key = "FriendsRemoved",
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor
					}
				}
			});
			_achievementClient.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Gain 5 Members!",
				ActorType = ActorType.Group,
				Token = "group_5_members",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataType.Long,
						Value = "5",
						Key = "MembersJoined",
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor
					}
				}
			});
			_achievementClient.Create(new AchievementRequest()
			{
				GameId = gameId,
				Name = "Lose 2 Members!",
				ActorType = ActorType.Group,
				Token = "group_minus_2_members",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataType.Long,
						Value = "2",
						Key = "MembersLeft",
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor
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
}
