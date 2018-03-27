using System.Collections.Generic;
using System.Linq;

using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.Loading;

using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour {

	[SerializeField]
	private Text _resourceCount;
	[SerializeField]
	private Transform _container;
	[SerializeField]
	private ResourcePrefab _prefab;

	public void Display(List<int> users)
	{
		gameObject.SetActive(true);
		foreach (Transform child in _container)
		{
			Destroy(child.gameObject);
		}
		Loading.Start();
		var friendHeader = Instantiate(_resourceCount, _container, true);
		friendHeader.text = "Friends";
		SUGARManager.UserFriend.GetFriendsList(gotFriends =>
		{
			foreach (var friend in SUGARManager.UserFriend.Friends)
			{
				if (users.Contains(friend.Actor.Id))
				{
					var friendObj = Instantiate(_prefab, _container, true);
					friendObj.SetUp(friend.Actor);
				}
			}
			var groupTitle = Instantiate(_resourceCount, _container, true);
			groupTitle.text = "Groups";
			var group = Instantiate(_prefab, _container, true);
			group.SetUp(SUGARManager.CurrentGroup);
			var groupMembers = Instantiate(_resourceCount, _container, true);
			groupMembers.text = "Group Members";
			SUGARManager.Client.GroupMember.GetMembersAsync(SUGARManager.CurrentGroup.Id, success =>
			{
				var memberList = success.ToList();
				foreach (var member in memberList)
				{
					if (users.Contains(member.Id))
					{
						var memberObj = Instantiate(_prefab, _container, true);
						memberObj.SetUp(member);
					}
					if (member == memberList.Last())
					{
						Loading.Stop();
					}
				}
			}, error =>
			{

			});
		});
	}

	private void OnEnable()
	{
		InvokeRepeating("UpdateCount", 0, 1);
	}

	private void OnDisable()
	{
		CancelInvoke("UpdateCount");
	}

	private void UpdateCount()
	{
		long choc = 0;
		SUGARManager.Resource.UserGameResources.TryGetValue("Chocolate", out choc);
		_resourceCount.text = "Chocolate: " + choc;
	}
}
