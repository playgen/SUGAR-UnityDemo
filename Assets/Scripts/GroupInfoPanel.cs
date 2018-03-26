using System.Linq;
using PlayGen.SUGAR.Unity;

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

	public void Display(int id)
	{
		gameObject.SetActive(true);
		foreach (Transform child in _memberContainer)
		{
			Destroy(child.gameObject);
		}
		foreach (Transform child in _allianceContainer)
		{
			Destroy(child.gameObject);
		}
		SUGARManager.Client.GroupMember.GetMembersAsync(id, success =>
		{
			var memberList = success.ToList();
			foreach (var member in memberList)
			{
				var user = Instantiate(_userPrefab, _memberContainer, true);
				if (SUGARManager.UserFriend.Friends.Select(f => f.Actor.Id).Contains(member.Id))
				{
					user.SetUp(member, 0);
				}
				else if (SUGARManager.UserFriend.PendingSent.Select(f => f.Actor.Id).Contains(member.Id))
				{
					user.SetUp(member, 1);
				}
				else if (SUGARManager.UserFriend.PendingReceived.Select(f => f.Actor.Id).Contains(member.Id))
				{
					user.SetUp(member, 2);
				}
				else
				{
					user.SetUp(member, 3);
				}
			}
		}, error =>
		{

		});
		SUGARManager.Client.AllianceClient.GetAlliancesAsync(id, success =>
		{
			var allianceList = success.ToList();
			foreach (var ally in allianceList)
			{
				var group = Instantiate(_groupPrefab, _memberContainer, true);
				if (SUGARManager.UserGroup.Groups.Select(f => f.Actor.Id).Contains(ally.Id))
				{
					group.SetUp(ally, true);
				}
				else
				{
					group.SetUp(ally, false);
				}
			}
		}, error =>
		{

		});
	}
}
