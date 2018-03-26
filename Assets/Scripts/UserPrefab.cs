using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;

using UnityEngine;
using UnityEngine.UI;

public class UserPrefab : MonoBehaviour {

	[SerializeField]
	private Text _userName;
	[SerializeField]
	private Button _details;
	[SerializeField]
	private Button _add;
	[SerializeField]
	private Button _cancel;
	[SerializeField]
	private Button _remove;
	[SerializeField]
	private Button _accept;
	[SerializeField]
	private Button _reject;
	private ActorResponse _actor;

	public void SetUp(ActorResponse actor, int friendState)
	{
		_actor = actor;
		_userName.text = actor.Name;
		_details.onClick.RemoveAllListeners();
		_details.onClick.AddListener(() => GetComponentInParent<Canvas>().GetComponentInChildren<UserInfoPanel>(true).Display(_actor.Id));
		_add.onClick.RemoveAllListeners();
		_cancel.onClick.RemoveAllListeners();
		_remove.onClick.RemoveAllListeners();
		_accept.onClick.RemoveAllListeners();
		_reject.onClick.RemoveAllListeners();
		_add.gameObject.SetActive(false);
		_cancel.gameObject.SetActive(false);
		_remove.gameObject.SetActive(false);
		_accept.gameObject.SetActive(false);
		_reject.gameObject.SetActive(false);
		switch (friendState)
		{
			case 0:
				_remove.gameObject.SetActive(true);
				_reject.onClick.AddListener(() => SUGARManager.UserFriend.RemoveFriend(_actor.Id));
				_reject.onClick.AddListener(() => _remove.gameObject.SetActive(false));
				break;
			case 1:
				_cancel.gameObject.SetActive(true);
				_cancel.onClick.AddListener(() => SUGARManager.UserFriend.ManageFriendRequest(_actor.Id, false, true));
				_cancel.onClick.AddListener(() => _cancel.gameObject.SetActive(false));
				break;
			case 2:
				_accept.gameObject.SetActive(true);
				_accept.onClick.AddListener(() => SUGARManager.UserFriend.ManageFriendRequest(_actor.Id, true));
				_accept.onClick.AddListener(() => _accept.gameObject.SetActive(false));
				_accept.onClick.AddListener(() => _reject.gameObject.SetActive(false));
				_reject.gameObject.SetActive(true);
				_reject.onClick.AddListener(() => SUGARManager.UserFriend.ManageFriendRequest(_actor.Id, false));
				_reject.onClick.AddListener(() => _accept.gameObject.SetActive(false));
				_reject.onClick.AddListener(() => _reject.gameObject.SetActive(false));
				break;
			case 3:
				_add.gameObject.SetActive(true);
				_add.onClick.AddListener(() => SUGARManager.UserFriend.AddFriend(_actor.Id));
				_add.onClick.AddListener(() => _add.gameObject.SetActive(false));
				break;
		}
	}
}
