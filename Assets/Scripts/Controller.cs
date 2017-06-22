using System;
using UnityEngine;
using System.Linq;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Client.Unity;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	private GameDataClient _gameDataClient;
	private GameObject[] _views;
	private int _viewIndex;
	private Achievement _achievementPanel;
	private GameClient _gameClient;
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
	public Button NextButton;
	public Button PreviousButton;

	// ReSharper disable once ClassNeverInstantiated.Local

	public void Reset()
	{
		UserId = null;
		GroupId = null;
	}

	void Awake()
	{
		ConsoleDebugRedirect.Redirect();
#if UNITY_WEBGL
		Factory = new SUGARClient(ScriptLocator.Config.BaseUri, new UnityWebGlHttpHandler(), false);
#else
		Factory = new SUGARClient(ScriptLocator.Config.BaseUri);
#endif
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
		NextButton = UiPanel.transform.Find("NextBtn").gameObject.GetComponent<Button>();
		NextButton.onClick.AddListener(NextView);
		PreviousButton = UiPanel.transform.Find("PreviousBtn").gameObject.GetComponent<Button>();
		PreviousButton.onClick.AddListener(PreviousView);
		AchievementPanel = UiPanel.transform.Find("AchievementPanel").gameObject;
		_achievementPanel = AchievementPanel.GetComponent<Achievement>();
		UiPanel.SetActive(false);
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

	public bool CheckGame()
	{
		try
		{
			var gameResponses = _gameClient.Get(GameName);
			foreach (var gameResponse in gameResponses)
			{
				if (gameResponse.Name == GameName)
				{
					GameId = gameResponse.Id;
					GetLeaderboardId();
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

	public void ActivateUiPanels()
	{
		UiPanel.SetActive(true);
	}

	public void UpdateUi()
	{
		ScriptLocator.ResourceController.UpdateList();
		_achievementPanel.UpdateAchivementLists();
	}

	public void SaveData(int actorId, string key, string value, EvaluationDataType dataType)
	{
		_gameDataClient.Add(new EvaluationDataRequest()
		{
			GameId = GameId,
			CreatingActorId = actorId,
			EvaluationDataType = dataType,
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
	}
}
