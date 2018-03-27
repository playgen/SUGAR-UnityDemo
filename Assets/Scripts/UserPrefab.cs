using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;

using UnityEngine;
using UnityEngine.UI;

public class UserPrefab : MonoBehaviour {

	[SerializeField]
	private Text _userName;
	[SerializeField]
	private Button _add;
	private ActorResponse _actor;

	public void SetUp(ActorResponse actor, bool friend)
	{
		_actor = actor;
		_userName.text = actor.Name;
		_add.onClick.RemoveAllListeners();
		_add.gameObject.SetActive(!friend);
		_add.onClick.AddListener(() => SUGARManager.UserFriend.AddFriend(_actor.Id, false));
		_add.onClick.AddListener(() => SUGARManager.GameData.Send("FRIENDS_MADE", 1));
		_add.onClick.AddListener(() => _add.gameObject.SetActive(false));
	}
}
