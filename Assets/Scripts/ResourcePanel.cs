using System.Collections.Generic;
using System.Linq;

using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.BestFit;
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
		SUGARManager.UserFriend.GetFriendsList(gotFriends =>
		{
			var friendFound = false;
			foreach (var friend in SUGARManager.UserFriend.Friends)
			{
				if (users.Contains(friend.Actor.Id))
				{
					if (!friendFound)
					{
						var friendHeader = Instantiate(_resourceCount, _container, false);
						friendHeader.text = "Friends";
						((RectTransform)friendHeader.transform).sizeDelta = new Vector2(((RectTransform)friendHeader.transform).sizeDelta.x, 15);
						friendFound = true;
					}
					var friendObj = Instantiate(_prefab, _container, false);
					friendObj.SetUp(friend.Actor);
				}
			}
			if (SUGARManager.CurrentGroup != null)
			{
				var groupTitle = Instantiate(_resourceCount, _container, false);
				groupTitle.text = "Groups";
				((RectTransform)groupTitle.transform).sizeDelta = new Vector2(((RectTransform)groupTitle.transform).sizeDelta.x, 15);
				var group = Instantiate(_prefab, _container, false);
				group.SetUp(SUGARManager.CurrentGroup);
				SUGARManager.Client.GroupMember.GetMembersAsync(SUGARManager.CurrentGroup.Id, success =>
				{
					var memberList = success.ToList();
					var memberFound = false;
					foreach (var member in memberList)
					{
						if (!memberFound)
						{
							var groupMembers = Instantiate(_resourceCount, _container, false);
							groupMembers.text = "Group Members";
							((RectTransform)groupMembers.transform).sizeDelta = new Vector2(((RectTransform)groupMembers.transform).sizeDelta.x, 15);
							memberFound = true;
						}
						if (users.Contains(member.Id))
						{
							var memberObj = Instantiate(_prefab, _container, false);
							memberObj.SetUp(member);
						}
						if (member == memberList.Last())
						{
							Loading.Stop();
							_container.GetComponentsInChildren<Text>(true).Where(t => t.name == "Username").ToList().BestFit();
							_container.GetComponentsInChildren<Button>(true).Select(t => t.gameObject).ToList().BestFit();
						}
					}
				}, error =>
				{

				});
			}
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
