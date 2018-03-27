using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;

using UnityEngine;
using UnityEngine.UI;

public class GroupPrefab : MonoBehaviour {

	[SerializeField]
	private Text _groupName;
	[SerializeField]
	private Button _join;
	private ActorResponse _actor;

	public void SetUp(ActorResponse actor)
	{
		_actor = actor;
		_groupName.text = actor.Name;
		_join.onClick.RemoveAllListeners();
		_join.gameObject.SetActive(SUGARManager.CurrentGroup == null);
		_join.onClick.AddListener(() => SUGARManager.UserGroup.AddGroup(actor.Id, false));
		_join.onClick.AddListener(() => GetComponentInParent<Canvas>().GetComponentInChildren<Controller>(true).UpdateGroup(_actor));
		_join.onClick.AddListener(() => SUGARManager.GameData.Send("GROUPS_JOINED", 1));
	}
}
