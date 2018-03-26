using PlayGen.SUGAR.Unity;

using UnityEngine;

public class UserInfoPanel : MonoBehaviour
{
	[SerializeField]
	private Transform _friendContainer;
	[SerializeField]
	private Transform _groupContainer;
	[SerializeField]
	private UserPrefab _userPrefab;
	[SerializeField]
	private GroupPrefab _groupPrefab;

	public void Display(int id)
	{
		gameObject.SetActive(true);
		foreach (Transform child in _friendContainer)
		{
			Destroy(child.gameObject);
		}
		foreach (Transform child in _groupContainer)
		{
			Destroy(child.gameObject);
		}
		foreach (var member in SUGARManager.UserFriend.Friends)
		{
			var user = Instantiate(_userPrefab, _friendContainer, true);
			user.SetUp(member.Actor, 0);
		}
		foreach (var member in SUGARManager.UserFriend.PendingSent)
		{
			var user = Instantiate(_userPrefab, _friendContainer, true);
			user.SetUp(member.Actor, 1);
		}
		foreach (var member in SUGARManager.UserFriend.PendingReceived)
		{
			var user = Instantiate(_userPrefab, _friendContainer, true);
			user.SetUp(member.Actor, 2);
		}
		foreach (var groupIn in SUGARManager.UserGroup.Groups)
		{
			var group = Instantiate(_groupPrefab, _groupContainer, true);
			group.SetUp(groupIn.Actor, true);
		}
	}
}
