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
	public static int LeaderboardId { get; set; }
	public string GameName;
	public string BaseUri;
	public GameObject Views;
	public GameObject BtnPanel;
	public GameObject LoginPanel;
	public static Text SkillText;
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
		var skillMeasure = BtnPanel.transform.FindChild("SkillMeasure").gameObject;
		SkillText = skillMeasure.GetComponent<Text>();
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
		var prev = LoginAdmin();
		Debug.Log("LoginAdmin: " + prev);

		prev = prev && CheckGame();
		Debug.Log("CheckGame: " + prev);
		if (!prev)
		{
			prev = GetLeaderboardId();
			Debug.Log("GetLeaderboardId: " + prev);
			return prev;
		}

		prev = prev && SetUpGame();
		Debug.Log("SetUpGame: " + prev);

		prev = prev && SetUpGroups();
		Debug.Log("SetUpGroups: " + prev);

		prev = prev && _achievementPanel.SetUpAchievements();
		Debug.Log("SetUpAchievements: " + prev);

		prev = prev && SetUpLeaderboard();
		Debug.Log("SetUpLeaderboard: " + prev);

		prev = prev && SetUpSkills();
		Debug.Log("SetUpSkills: " + prev);
		return prev;
	}

	private static void UpdateSkill()
	{
		Debug.Log("UpdateAchievements");
		var skillClient = Factory.Skill;
		try
		{
			var responses = skillClient.GetGameProgress(UserId.Value.ToString(), GameId.ToString());
			var response = responses.FirstOrDefault();
			SkillText.text = response.Name + ": " + response.Progress;			// MAKE THIS WORK
		}
		catch(Exception exception)
		{
			Debug.Log("Error updating skill: " + exception.Message);
		}
	}

	private bool SetUpSkills()
	{
		var skillClient = Factory.Skill;
		try
		{
			skillClient.Create(new AchievementRequest()
			{
				GameId = GameId,
				Name = "Social Skill",
				ActorType = ActorType.User,
				Token = "SOCIAL",
				CompletionCriteria = new List<AchievementCriteria>()
				{
					new AchievementCriteria()
					{
						DataType = GameDataType.Long,
						Value = "10",
						Key = "FriendsAdded",
						ComparisonType = ComparisonType.Equals,
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

	private bool GetLeaderboardId()
	{
		var leaderboardClient = Factory.Leaderboard;
		try
		{
			var leaderboardReponse = leaderboardClient.Get(GameId.ToString());
			LeaderboardId = leaderboardReponse.Select(x => x.Id).FirstOrDefault();
			return true;
		}
		catch (Exception exception)
		{
			Debug.LogError("Could not find leaderboard: " + exception.Message);
		}
		return false;
	}

	private bool SetUpLeaderboard()
	{
		var leaderboardClient = Factory.Leaderboard;
		try
		{
			var leaderboardResponse = leaderboardClient.Create(new LeaderboardRequest()
			{
				GameId = GameId,
				Name = "Most Friends",
				Token = "MOST_FRIENDS",
				Key = "FriendsAdded",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType	= LeaderboardType.Cumulative
			});
			LeaderboardId = leaderboardResponse.Id;
			return true;
		}
		catch (Exception exception)
		{
			Debug.Log("Create Leaderboard fail: " + exception.Message);
		}

		return false;
	}

	private bool SetUpGroups()
	{
		int groupNum = 4;
		int[] Ids = new int[groupNum];
		var _groupClient = Factory.Group;
		try
		{
			for (int i = 0; i < groupNum; i++)
			{
				var actorResponse = _groupClient.Create(new ActorRequest()
				{
					Name = "group " + i
				});
				Ids[i] = actorResponse.Id;
			}
			return true;
		}
		catch (Exception exception)
		{
			Debug.Log("Set Up Groups Failed: " + exception.Message);
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
					return false;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("Failed to find game." + ex.Message);
		}
		return true;

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
		UpdateSkill();
		_achievementPanel.UpdateAchivementLists();
	}

	public static void SaveData(int actorId, string key, string value, GameDataType dataType)
	{
		_gameDataClient.Add(new GameDataRequest()
		{
			GameId = GameId,
			ActorId = actorId,
			GameDataType = dataType,
			Value = value,
			Key = key
		});
	}

	public static void NextView()
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
