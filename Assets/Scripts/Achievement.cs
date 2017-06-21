using System;
using UnityEngine;
using System.Collections.Generic;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Shared;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
	private AchievementClient _achievementClient;
	public GameObject AchievementList;
	public GameObject AchivementItemPrefab;
	private int _achieveCount;

	public int ListDisplaySize = 4;

	void OnEnable () {
		UpdateAchivementLists();
	}

	public void UpdateAchivementLists()
	{
		if (_achievementClient == null)
		{
			_achievementClient = ScriptLocator.Controller.Factory.Achievement;
		}
		var userId = ScriptLocator.Controller.UserId;
		if (userId.HasValue)
		{
			UpdateAchievementsList(AchievementList, userId.Value);
		}
		var groupId = ScriptLocator.Controller.GroupId;
		if (groupId.HasValue)
		{
			UpdateAchievementsList(AchievementList, groupId.Value, false);
		}
	}

	private void ClearList(GameObject listObject)
	{
		//Remove old achievemnts list
		foreach (Transform child in listObject.transform)
		{
			Destroy(child.gameObject);
		}
	}

	private void UpdateAchievementsList(GameObject listObject, int actorId, bool clear = true)
	{
		if (clear)
		{
			ClearList(listObject);
			_achieveCount = 0;
		}
		try
		{
			var achievements = _achievementClient.GetGameProgress(ScriptLocator.Controller.GameId, actorId);
			var listRect = listObject.GetComponent<RectTransform>().rect;
			foreach (var achievement in achievements)
			{
				var achievementItem = Instantiate(AchivementItemPrefab);
				achievementItem.transform.SetParent(listObject.transform, false);
				var itemRectTransform = achievementItem.GetComponent<RectTransform>();
				itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / ListDisplaySize);
				itemRectTransform.anchoredPosition = new Vector2(0, (_achieveCount * -(listRect.height / ListDisplaySize)));
				achievementItem.GetComponentInChildren<Text>().text = achievement.Name;
				if (achievement.Progress != 1.0f)
				{
					Destroy(achievementItem.transform.Find("Tick").gameObject);
				}
				_achieveCount++;

			}
		}
		catch (Exception ex)
		{
			Debug.Log("Failed to get achievements list. " + ex.Message);
		}
	}

	public bool SetUpAchievements()
	{
		if (_achievementClient == null)
		{
			_achievementClient = ScriptLocator.Controller.Factory.Achievement;
		}
		var gameId = ScriptLocator.Controller.GameId;
		try
		{
			_achievementClient.Create(new EvaluationCreateRequest()
			{
				
				GameId = gameId,
				Name = "Join a Group!",
				ActorType = ActorType.User,
				Token = "join_group",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						EvaluationDataType = EvaluationDataType.Long,
						Value = "1",
						EvaluationDataKey = "GroupsJoined",
						CriteriaQueryType = CriteriaQueryType.Any,
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor

					}
				}
			});
			_achievementClient.Create(new EvaluationCreateRequest()
			{
				GameId = gameId,
				Name = "Add 2 Friends!",
				ActorType = ActorType.User,
				Token = "add_2_friends",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						EvaluationDataType = EvaluationDataType.Long,
						Value = "2",
						EvaluationDataKey = "FriendsAdded",
						CriteriaQueryType = CriteriaQueryType.Sum,
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor
					}
				}
			});
			_achievementClient.Create(new EvaluationCreateRequest()
			{
				GameId = gameId,
				Name = "Remove a Friend!",
				ActorType = ActorType.User,
				Token = "remove_friend",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						EvaluationDataType = EvaluationDataType.Long,
						Value = "1",
						EvaluationDataKey = "FriendsRemoved",
						CriteriaQueryType = CriteriaQueryType.Any,
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor
					}
				}
			});
			_achievementClient.Create(new EvaluationCreateRequest()
			{
				GameId = gameId,
				Name = "Gain 5 Members!",
				ActorType = ActorType.Group,
				Token = "group_5_members",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						EvaluationDataType = EvaluationDataType.Long,
						Value = "5",
						EvaluationDataKey = "MembersJoined",
						CriteriaQueryType = CriteriaQueryType.Sum,
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor
					}
				}
			});
			_achievementClient.Create(new EvaluationCreateRequest()
			{
				GameId = gameId,
				Name = "Lose 2 Members!",
				ActorType = ActorType.Group,
				Token = "group_minus_2_members",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						EvaluationDataType = EvaluationDataType.Long,
						Value = "2",
						EvaluationDataKey = "MembersLeft",
						CriteriaQueryType = CriteriaQueryType.Sum,
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
