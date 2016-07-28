using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
	private SkillClient _skillClient;

	public GameObject SkillPanel;
	public GameObject SkillItemPrefab;
	public GameObject SkillList;

	void Awake()
	{
		_skillClient = ScriptLocator.Controller.Factory.Skill;
	}

	public void UpdateList()
	{
		Debug.Log("update");
		//Remove old achievemnts list
		foreach (Transform child in SkillList.transform)
		{
			Destroy(child.gameObject);
		}
		try
		{
			var responses = _skillClient.GetGameProgress(ScriptLocator.Controller.GameId, ScriptLocator.Controller.UserId.Value);
			Debug.Log(responses.Count());

			int counter = 0;
			var listRect = SkillList.GetComponent<RectTransform>().rect;
			foreach (var response in responses)
			{
				var skillItem = Instantiate(SkillItemPrefab);
				skillItem.transform.SetParent(SkillList.transform, false);
				var itemRectTransform = skillItem.GetComponent<RectTransform>();
				itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 5);
				itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 5)));
				skillItem.transform.FindChild("Name").GetComponent<Text>().text = response.Name + ":";
				skillItem.transform.FindChild("Bar").GetComponent<Image>().fillAmount = response.Progress;
				counter++;
			}
		   
		}
		catch (Exception exception)
		{
			Debug.LogError("Update Resource List Failed: " + exception);
		}
	}

	public bool SetUpSkills()
	{
		try
		{
			_skillClient.Create(new AchievementRequest()
			{
				GameId = ScriptLocator.Controller.GameId,
				Name = "Social Skill",
				ActorType = ActorType.User,
				Token = "SOCIAL",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataType.Long,
						Value = "8",
						Key = "FriendsAdded",
						CriteriaQueryType = CriteriaQueryType.Sum,
						ComparisonType = ComparisonType.GreaterOrEqual,
						Scope = CriteriaScope.Actor

					}
				}
			});
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogError("Failed to create skill: " + exception.Message);
		}
		return false;
	}
}
