using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.Loading;
using PlayGen.Unity.Utilities.Text;
using UnityEngine;
using UnityEngine.UI;

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
			var memberList = success.Select(f => f.Id).ToList();
			SUGARManager.UserFriend.GetFriendsList(gotFriends =>
			{
				var friends = SUGARManager.UserFriend.Friends.Select(f => f.Actor.Id).ToList();
				foreach (var user in users)
				{
					SUGARManager.Client.User.GetAsync(user, userDetails =>
					{
						var userObj = Instantiate(_userPrefab, _memberContainer, false);
						userObj.SetUp(userDetails, friends.Contains(user), memberList.Contains(user));
					}, userError =>
					{

					});
				}
				_memberContainer.GetComponentsInChildren<Text>(true).Where(t => t.name == "Username").ToList().BestFit();
				Loaded();
			});
		}, error =>
		{

		});
		SUGARManager.Client.AllianceClient.GetAlliancesAsync(SUGARManager.CurrentGroup.Id, success =>
		{
			var allianceList = success.Select(f => f.Id).ToList();
			foreach (var group in groups)
			{
				SUGARManager.Client.Group.GetAsync(group, groupDetails =>
				{
					var groupObj = Instantiate(_groupPrefab, _allianceContainer, false);
					groupObj.SetUp(groupDetails, allianceList.Contains(group));
				}, groupError =>
				{

				});
				
			}
			_allianceContainer.GetComponentsInChildren<Text>(true).Where(t => t.name == "Username").ToList().BestFit();
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
