using UnityEngine;
using System.Collections;
using PlayGen.SGA.ClientAPI;
using PlayGen.SGA.Contracts;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {

	//private IEnumerable<ActorResponse> _userGroups;   LEADERS
	//private GroupMemberClientProxy _groupMemberProxy; LEADERBOARD PROXY
	public GameObject LeaderItemPrefab;
	public GameObject LeaderboardObject;
	public Text StatusText;

	// Use this for initialization
	void Awake()
	{
	}

	void OnEnable()
	{
		//UpdateLeaderboard();
	}

	void OnDisable()
	{
		ClearList();
	}

	private void ClearList()
	{
		//Remove old friends list
		foreach (Transform child in LeaderboardObject.transform)
		{
			Destroy(child.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
