using UnityEngine;
using System.Collections;
using PlayGen.SGA.ClientAPI;
using PlayGen.SGA.Contracts;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour {

	//private IEnumerable<ActorResponse> _userGroups;   LEADERS
	//private GroupMemberClientProxy _groupMemberProxy; LEADERBOARD PROXY
	public GameObject LeaderItemPrefab;
	public GameObject Leaderboard;
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
		foreach (Transform child in Leaderboard.transform)
		{
			Destroy(child.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
