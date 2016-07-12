using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	private GameDataClient _gameDataClient;
	private GameObject[] _views;
	private int _viewIndex;
	private Achievement _achievementPanel;
	private GameObject _skillTracker;
	private GameClient _gameClient;

	public SUGARClient Factory;
	public int? UserId { get; set; }
	public int? GroupId { get; set; }
	public string LoginToken { get; set; }
	public int GameId { get; set; }
	public string LeaderboardId { get; set; }
	public string GameName;
	public string BaseUri;
	public GameObject Views;
	public GameObject UiPanel;
	public GameObject LoginPanel;
	public GameObject AchievementPanel;
	public GameObject GroupAchievementPanel;
	public Button NextButton;
	public Button PreviousButton;

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
		NextButton = UiPanel.transform.FindChild("NextBtn").gameObject.GetComponent<Button>();
		NextButton.onClick.AddListener(NextView);
		PreviousButton = UiPanel.transform.FindChild("PreviousBtn").gameObject.GetComponent<Button>();
		PreviousButton.onClick.AddListener(PreviousView);
		AchievementPanel = UiPanel.transform.FindChild("AchievementPanel").gameObject;
		GroupAchievementPanel = UiPanel.transform.FindChild("GroupAchievementPanel").gameObject;
		_achievementPanel = AchievementPanel.GetComponent<Achievement>();
		_skillTracker = UiPanel.transform.FindChild("SkillTracker").gameObject;
		if (_skillTracker == null)
		{
			Debug.LogError("Skill Panel not found");
		}
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

	private void UpdateSkill()
	{
		var skillClient = Factory.Skill;
		try
		{
			var responses = skillClient.GetGameProgress(UserId.Value, GameId);
			var response = responses.FirstOrDefault();
			Debug.Log("UpdateSkill " + response.Progress);
			_skillTracker.GetComponentInChildren<Text>().text = response.Name + ":";
			_skillTracker.GetComponentInChildren<Image>().fillAmount = response.Progress;
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

	private bool GetLeaderboardId()
	{
		var leaderboardClient = Factory.Leaderboard;
		try
		{
			var leaderboardReponse = leaderboardClient.Get(GameId);
			LeaderboardId = leaderboardReponse.Select(x => x.Token).FirstOrDefault();
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
			LeaderboardId = leaderboardResponse.Token;
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
		try
		{
			ScriptLocator.LoginController.GetLoginAccountResponse("admin", "admin");
			return true;
		}
		catch
		{
			try
			{
				ScriptLocator.LoginController.GetRegisterAccountResponse("admin", "admin", true);
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


	public void ActivateAchievementPanels()
	{
		UiPanel.SetActive(true);
	}

	public void UpdateUi()
	{
		ScriptLocator.ResourceController.UpdateList();
		UpdateSkill();
		_achievementPanel.UpdateAchivementLists();
	}

	public void SaveData(int actorId, string key, string value, GameDataType dataType)
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

	public void NextView()
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
		if (_viewIndex == 1)
		{
			PreviousButton.GetComponent<Text>().text = "Logout";
			PreviousButton.onClick.AddListener(Logout);
		}
		_views[_viewIndex].SetActive(true);
	}

	private void Logout()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
