using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	private static UserDataClient _saveData;
	private static GroupSaveDataClient _groupSaveData;
	private static GameObject[] _views;
	private static int _viewIndex;
	private static Achievement _achievementPanel;
	private static GroupAchievement _groupAchievementPanel;
	private GameClient _game;

	public static ClientProxyFactory Factory;
	public static int? UserId { get; set; }
	public static int? GroupId { get; set; }
	public static string LoginToken { get; set; }
	public static int GameId { get; set; }
	public string GameName;
	public string BaseUri;
	public GameObject Views;
	public GameObject BtnPanel;
	public static GameObject AchievementPanel;
	public static GameObject GroupAchievementPanel;
	public static Button NextButton;
	public static Button PreviousButton;

	void Awake()
	{
		Factory = new ClientProxyFactory(BaseUri);
		_saveData = Factory.GetUserSaveDataClient;
		_groupSaveData = Factory.GetGroupSaveDataClient;
		_game = Factory.GetGameClient;
		_views = new GameObject[Views.transform.childCount];
		_viewIndex = 0;
		foreach (Transform child in Views.transform)
		{
			_views[_viewIndex] = child.gameObject;
			_viewIndex++;
		}
		_viewIndex = 0;
		NextButton = BtnPanel.transform.FindChild("NextBtn").gameObject.GetComponent<Button>();
		NextButton.onClick.AddListener(() => NextView());
		PreviousButton = BtnPanel.transform.FindChild("PreviousBtn").gameObject.GetComponent<Button>();
		PreviousButton.onClick.AddListener(PreviousView);
		AchievementPanel = BtnPanel.transform.FindChild("AchievementPanel").gameObject;
		GroupAchievementPanel = BtnPanel.transform.FindChild("GroupAchievementPanel").gameObject;
		_achievementPanel = AchievementPanel.GetComponent<Achievement>();
		_groupAchievementPanel = GroupAchievementPanel.GetComponent<GroupAchievement>();
		if (!SetUp())
		{
			Debug.LogError("Setup Failed");
		}
	}

	private bool SetUp()
	{
		if (!CheckGame())
		{
			if (SetUpGame())
			{
				if (_achievementPanel.SetUpAchievements() && _groupAchievementPanel.SetUpGroupAchievements())
				{
					NextView(true);
					return true;
				}
			}
		}
		else
		{
			NextView(true);
			return true;
		}
		return false;
	}


	private bool CheckGame()
	{
		try
		{
			var gameResponses = _game.Get(new string[] { GameName });
			foreach (var gameResponse in gameResponses)
			{
				if (gameResponse.Name == GameName)
				{
					GameId = gameResponse.Id;
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Failed to find game." + ex.Message);
		}
		return false;
	}

	private bool SetUpGame()
	{
		try
		{
			var gameResponse = _game.Create(new GameRequest
			{
				Name = GameName
			});
			GameId = gameResponse.Id;
			Debug.Log("Created Game. ID: " + gameResponse.Id);
			return true;
		}
		catch (Exception ex)
		{
			Debug.Log("Create Game Failed: " + ex.Message);

		}
		return false;
	}


	public static void ActivateAchievementPanels()
	{
		AchievementPanel.SetActive(true);
		GroupAchievementPanel.SetActive(true);
	}

	public static void UpdateAchievements()
	{
		_achievementPanel.UpdateAchievementsList();
		_groupAchievementPanel.UpdateAchievementsList();
	}

	public static void SaveData(string key, string value, GameDataValueType dataType)
	{
		var saveDataResponse = _saveData.Add(new SaveDataRequest()
		{
			GameId = GameId,
			ActorId =  UserId.Value,
			GameDataValueType = dataType,
			Value = value,
			Key = key
		});
	}

	public static void SaveGroupData(string key, string value, GameDataValueType dataType)
	{
		if (GroupId.HasValue)
		{
			var saveDataResponse = _groupSaveData.Add(new SaveDataRequest()
			{
				GameId = GameId,
				ActorId = GroupId.Value,
				GameDataValueType = dataType,
				Value = value,
				Key = key
			});
		}
	}

	public static void NextView(bool first = false)
	{
		if (!first)
		{
			_views[_viewIndex].SetActive(false);
			if (_viewIndex == 0)
			{
				PreviousButton.interactable = true;
			}
			_viewIndex++;
			if (_viewIndex == _views.Length - 1)
			{
				NextButton.interactable = false;
			}
		}
		_views[_viewIndex].SetActive(true);

	}

	public void PreviousView()
	{
		_views[_viewIndex].SetActive(false);
		if (_viewIndex == _views.Length - 1)
		{
			NextButton.interactable = true;
		}
		_viewIndex--;
		if (_viewIndex == 0)
		{
			PreviousButton.interactable = false;
		}
		_views[_viewIndex].SetActive(true);
	}

}
