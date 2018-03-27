using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.Loading;

using UnityEngine;

public class GroupInfoPanel : MonoBehaviour {

	[SerializeField]
	private Transform _memberContainer;
	[SerializeField]
	private Transform _allianceContainer;
	[SerializeField]
	private UserPrefab _userPrefab;
	[SerializeField]
	private GroupPrefab _groupPrefab;
	private bool _loadedOne;

	public void Display(List<int> groups, List<int> users)
	{
		_loadedOne = false;
		gameObject.SetActive(true);
		foreach (Transform child in _memberContainer)
		{
			Destroy(child.gameObject);
		}
		foreach (Transform child in _allianceContainer)
		{
			Destroy(child.gameObject);
		}
		Loading.Start();
		SUGARManager.Client.GroupMember.GetMembersAsync(SUGARManager.CurrentGroup.Id, success =>
		{
			var memberList = success.ToList();
			SUGARManager.UserFriend.GetFriendsList(gotFriends =>
			{
				var friends = SUGARManager.UserFriend.Friends.Select(f => f.Actor.Id).ToList();
				foreach (var member in memberList)
				{
					if (users.Contains(member.Id))
					{
						var user = Instantiate(_userPrefab, _memberContainer, true);
						user.SetUp(member, friends.Contains(member.Id));
					}
				}
			});
			Loaded();
		}, error =>
		{

		});
		SUGARManager.Client.AllianceClient.GetAlliancesAsync(SUGARManager.CurrentGroup.Id, success =>
		{
			var allianceList = success.ToList();
			foreach (var ally in allianceList)
			{
				if (groups.Contains(ally.Id))
				{
					var group = Instantiate(_groupPrefab, _allianceContainer, true);
					group.SetUp(ally);
				}
			}
			Loaded();
		}, error =>
		{

		});
	}

	private void Loaded()
	{
		if (!_loadedOne)
		{
			_loadedOne = true;
		}
		else
		{
			Loading.Stop();
		}
	}
}
