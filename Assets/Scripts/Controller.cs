using System;
using UnityEngine;
using System.Linq;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Shared;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	private GameDataClient _gameDataClient;
	private GameObject[] _views;
	private int _viewIndex;
	private Achievement _achievementPanel;
	private GameClient _gameClient;
	private UserClient _userClient;
	private GroupClient _groupClient;
	private UserFriendClient _friendClient;
	private GroupMemberClient _memberClient;
	private readonly string[] _userNames = new[] {"Burnadette", "Fracheska", "Mr.Magoo", "Mary-Lou", "T-Bone", "Sarah-May-Concertina", "Colonel-Corny-Cobs"};
	private readonly string[] _groupNames = new[] {"Happy Campers", "Daunting Ducks", "Yellow Submarines", "The Best Group"};
	public SUGARClient Factory;
	public int? UserId { get; set; }
	public int? GroupId { get; set; }
	public string LoginToken { get; set; }
	public int GameId { get; set; }
	public string LeaderboardId { get; set; }
	public string GameName;
	
	public GameObject Views;
	public GameObject UiPanel;
	public GameObject LoginPanel;
	public GameObject AchievementPanel;
	public GameObject GroupAchievementPanel;
	public Button NextButton;
	public Button PreviousButton;

	public string[] UserNames { get { return _userNames; } }
	public string[] GroupNames { get { return _groupNames; } }

	// ReSharper disable once ClassNeverInstantiated.Local

	public void Reset()
	{
		UserId = null;
		GroupId = null;
	}

	void Awake()
	{
#if UNITY_WEBGL
		Factory = new SUGARClient(ScriptLocator.Config.BaseUri, new UnityWebGlHttpHandler());
#else
		Factory = new SUGARClient(ScriptLocator.Config.BaseUri);
#endif
		_gameDataClient = Factory.GameData;
		_gameClient = Factory.Game;
		_userClient = Factory.User;
		_groupClient = Factory.Group;
		_memberClient = Factory.GroupMember;
		_friendClient = Factory.UserFriend;
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

		prev &= CheckGame();
		Debug.Log("CheckGame: " + prev);
		if (!prev)
		{
			prev = GetLeaderboardId();
			Debug.Log("GetLeaderboardId: " + prev);
			return prev;
		}

		prev &= SetUpGame();
		Debug.Log("SetUpGame: " + prev);

		int[] groupIds;
		prev &= SetUpGroups(out groupIds);
		Debug.Log("SetUpGroups: " + prev);

		int[] userIds;
		prev &= SetupUsers(out userIds);
		Debug.Log("SetupUsers: " + prev);

		prev &= SetupUserMembers(groupIds, userIds);
		Debug.Log("SetupUserMembers: " + prev);

		prev &= SetupUserFriends(userIds);
		Debug.Log("SetupUserFriends: " + prev);

		prev &= _achievementPanel.SetUpAchievements();
		Debug.Log("SetUpAchievements: " + prev);

		prev &= SetUpLeaderboard();
		Debug.Log("SetUpLeaderboard: " + prev);

		prev &= ScriptLocator.SkillController.SetUpSkills();
		Debug.Log("SetUpSkills: " + prev);
		return prev;
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

	private bool SetUpGroups(out int[] ids)
	{
		ids = new int[_groupNames.Length];
		_groupClient = Factory.Group;
		try
		{
			for (int i = 0; i < _groupNames.Length; i++)
			{
				var actorResponse = _groupClient.Create(new ActorRequest()
				{
					Name = _groupNames[i]
				});
				ids[i] = actorResponse.Id;
			}
			return true;
		}
		catch (Exception exception)
		{
			Debug.Log("Set Up Groups Failed: " + exception.Message);
		}
		return false;

	}

	private bool SetupUsers(out int[] userIds)
	{
		bool success = true;
		userIds = new int[_userNames.Length];

		for (int i = 0; i < _userNames.Length; i++)
		{			
			try
			{
				var userResponse = _userClient.Create(new ActorRequest
				{
					Name = _userNames[i]
				});

				userIds[i] = userResponse.Id;
				
				success &= true;

			}
			catch (Exception e)
			{
				Debug.Log("Couldn't create " + name + " because: " + e.Message);
				success = false;
			}
		}

		return success;
	}
	
	private bool SetupUserMembers(int[] groupIds, int[] userIds)
	{
		bool success = true;

		for (int i = 0; i < userIds.Length; i++)
		{
			int groupId = groupIds[i % groupIds.Length];

			try
			{
				var relationshipResponse = _memberClient.CreateMemberRequest(new RelationshipRequest
				{
					AcceptorId = groupId,
					RequestorId = userIds[i],
					AutoAccept = true
				});

				success &= true;

			}
			catch (Exception e)
			{
				Debug.Log("Couldn't create " + name + " because: " + e.Message);
				success = false;
			}
		}

		return success;
	}

	private bool SetupUserFriends(int[] userIds)
	{
		bool success = true;

		for (int i = 1; i < userIds.Length; i += 2)
		{
			if (i >= userIds.Length)
			{
				return success;
			}
			
			try
			{
				var relationshipResponse = _friendClient.CreateFriendRequest(new RelationshipRequest
				{
					AcceptorId = userIds[i - 1],
					RequestorId = userIds[i],
					AutoAccept = true
				});

				success &= true;

			}
			catch (Exception e)
			{
				Debug.Log("Couldn't create " + name + " because: " + e.Message);
				success = false;
			}
		}

		return success;
	}

	private bool LoginAdmin()
	{

		var response = ScriptLocator.LoginController.GetLoginAccountResponse("admin", "admin");
	    if (response != null)
	    {
	        return true;
	    }
	    else
	    {
	        throw new Exception("Admin Login Failed");
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


	public void ActivateUiPanels()
	{
		UiPanel.SetActive(true);
	}

	public void UpdateUi()
	{
		ScriptLocator.ResourceController.UpdateList();
		ScriptLocator.SkillController.UpdateList();
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
			PreviousButton.GetComponentInChildren<Text>().text = "Logout";
		}
		else
		{
			PreviousButton.GetComponentInChildren<Text>().text = "Back";
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
		if (PreviousButton.GetComponentInChildren<Text>().text == "Logout")
		{
			Logout();
		}
		if (_viewIndex == 2)
		{
			PreviousButton.GetComponentInChildren<Text>().text = "Logout";
		}
		else
		{
			PreviousButton.GetComponentInChildren<Text>().text = "Back";
		}
		_views[_viewIndex].SetActive(false);
		if (_viewIndex == _views.Length - 1)
		{
			NextButton.interactable = true;
		}
		_viewIndex--;
		_views[_viewIndex].SetActive(true);
	}

	private void Logout()
	{
		UiPanel.SetActive(false);

		var gos = (GameObject[]) GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (var go in gos)
		{
			go.gameObject.BroadcastMessage("Reset");
		}

		

		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
