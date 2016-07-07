using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	private static GameDataClient _gameDataClient;
	private static GameObject[] _views;
	private static int _viewIndex;
	private static Achievement _achievementPanel;
	private GameClient _gameClient;

	public static SUGARClient Factory;
	public static int? UserId { get; set; }
	public static int? GroupId { get; set; }
	public static string LoginToken { get; set; }
	public static int GameId { get; set; }
	public string GameName;
	public string BaseUri;
	public GameObject Views;
	public GameObject BtnPanel;
	public GameObject LoginPanel;
	public static GameObject AchievementPanel;
	public static GameObject GroupAchievementPanel;
	public static Button NextButton;
	public static Button PreviousButton;

	void Awake()
	{
		Factory = new SUGARClient(BaseUri);
		_gameDataClient = Factory.GameData;
		_gameClient = Factory.Game;
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
	}

	void Start()
	{
		if (!SetUp())
		{
			Debug.LogError("Setup Failed");
		}
	}

	private bool SetUp()
	{
		if (LoginAdmin())
		{
			if (!CheckGame())
			{
				if (SetUpGame())
				{
					if (_achievementPanel.SetUpAchievements())
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
		return false;
	}

	private bool LoginAdmin()
	{
		var loginScript = LoginPanel.GetComponent<Login>();
		try
		{
			loginScript.GetLoginAccountResponse("admin", "admin");
			return true;
		}
		catch
		{
			try
			{
				loginScript.GetRegisterAccountResponse("admin", "admin", true);
				return true;
			}
			catch (Exception exception)
			{
				throw new Exception(exception.Message);
			}
		}
	}


	private bool CheckGame()
	{
		try
		{
			var gameResponses = _gameClient.Get(GameName);
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
			var gameResponse = _gameClient.Create(new GameRequest
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
		_achievementPanel.UpdateAchivementLists();
	}

	public static void SaveData(int actorId, string key, string value, GameDataType dataType)
	{
		var saveDataResponse = _gameDataClient.Add(new GameDataRequest()
		{
			GameId = GameId,
			ActorId = actorId,
			GameDataType = dataType,
			Value = value,
			Key = key
		});
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
