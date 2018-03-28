using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.Loading;

using UnityEngine;
using UnityEngine.UI;

public class UserPrefab : MonoBehaviour {

	[SerializeField]
	private Text _userName;
	[SerializeField]
	private Button _addFriend;
	[SerializeField]
	private Button _addMember;
	private ActorResponse _actor;

	public void SetUp(ActorResponse actor, bool friend, bool member, bool asUser = true)
	{
		_actor = actor;
		_userName.text = actor.Name;
		_addFriend.onClick.RemoveAllListeners();
		_addFriend.gameObject.SetActive(!friend);
		_addFriend.onClick.AddListener(() => SUGARManager.UserFriend.AddFriend(_actor.Id, false));
		_addFriend.onClick.AddListener(() => SUGARManager.GameData.Send("FRIENDS_MADE", 1));
		_addFriend.onClick.AddListener(() => _addFriend.gameObject.SetActive(false));
		_addMember.onClick.RemoveAllListeners();
		_addMember.gameObject.SetActive(!member);
		_addMember.onClick.AddListener(MemberRequest);
		_addMember.onClick.AddListener(() => _addMember.gameObject.SetActive(false));
	}

	private void MemberRequest()
	{
		Loading.Start();
		SUGARManager.Client.GroupMember.CreateMemberRequestAsync(new RelationshipRequest { RequestorId = _actor.Id, AcceptorId = SUGARManager.CurrentGroup.Id, AutoAccept = true },
			success =>
				{
					Loading.Stop();
				}, error =>
				{
					Loading.Stop();
				});
	}
}
