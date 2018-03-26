using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;

using UnityEngine;
using UnityEngine.UI;

public class GroupPrefab : MonoBehaviour {

	[SerializeField]
	private Text _groupName;
	[SerializeField]
	private Button _details;
	[SerializeField]
	private Button _primary;
	[SerializeField]
	private Button _join;
	private ActorResponse _actor;

	public void SetUp(ActorResponse actor, bool primary)
	{
		_actor = actor;
		_groupName.text = actor.Name;
		_details.onClick.RemoveAllListeners();
		_details.onClick.AddListener(() => GetComponentInParent<Canvas>().GetComponentInChildren<GroupInfoPanel>(true).Display(_actor.Id));
		_primary.onClick.RemoveAllListeners();
		_join.onClick.RemoveAllListeners();
		_primary.gameObject.SetActive(primary);
		_join.gameObject.SetActive(!primary);
		if (primary)
		{
			_primary.onClick.AddListener(() => GetComponentInParent<Canvas>().GetComponentInChildren<Controller>(true).UpdateGroup(_actor));
		}
		else
		{
			_join.onClick.AddListener(() => SUGARManager.UserGroup.AddGroup(actor.Id));
			if (SUGARManager.CurrentGroup == null)
			{
				_join.onClick.AddListener(() => GetComponentInParent<Canvas>().GetComponentInChildren<Controller>(true).UpdateGroup(_actor));
			}
		}
	} 
}
