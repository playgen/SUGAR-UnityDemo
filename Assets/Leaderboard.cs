using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayGen.SUGAR.Client;
using PlayGen.SUGAR.Contracts;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{

	//private IEnumerable<ActorResponse> _userGroups;   LEADERS
	private LeaderboardClient _leaderboardClient;
	public GameObject LeaderItemPrefab;
	public GameObject LeaderboardObject;
	public Text StatusText;

	// Use this for initialization
	void Awake()
	{
		_leaderboardClient = Controller.Factory.Leaderboard;
	}

	void OnEnable()
	{
		UpdateLeaderboard();
	}

	private void UpdateLeaderboard()
	{
		ClearList();
		try
		{
			var leaderboardStandingsResponse = _leaderboardClient.CreateGetLeaderboardStandings(new LeaderboardStandingsRequest()
			{
				LeaderboardId = Controller.LeaderboardId,
				ActorId = Controller.UserId.Value,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				Limit = 8,
				Offset = 0

			});
			UpdateList(leaderboardStandingsResponse);
		}
		catch (Exception exception)
		{
			StatusText.text = exception.Message;
		}
	}

	private void UpdateList(IEnumerable<LeaderboardStandingsResponse> leaderboardStandings)
	{
		
		var listRect = LeaderboardObject.GetComponent<RectTransform>().rect;
		foreach (var standing in leaderboardStandings)
		{
			var leaderboardItem = Instantiate(LeaderItemPrefab);
			leaderboardItem.transform.SetParent(LeaderboardObject.transform, false);
			var itemRectTransform = leaderboardItem.GetComponent<RectTransform>();
			itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 8);
			itemRectTransform.anchoredPosition = new Vector2(0, ((standing.Ranking - 1) * -(listRect.height / 8)));
			var itemScript = leaderboardItem.GetComponent<LeaderItemScript>();
			itemScript.NameText.text = standing.ActorName;
			itemScript.RankText.text = standing.Ranking.ToString();
			itemScript.ValueText.text = standing.Value;
		}
	}

	//void OnDisable()
	//{
	//	ClearList();
	//}

	private void ClearList()
	{
		//Remove old friends list
		foreach (Transform child in LeaderboardObject.transform)
		{
			Destroy(child.gameObject);
		}
		
	}
}