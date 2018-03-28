using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.Loading;

using UnityEngine;
using UnityEngine.UI;

public class GroupPrefab : MonoBehaviour {

	[SerializeField]
	private Text _groupName;
	[SerializeField]
	private Button _ally;
	private ActorResponse _actor;

	public void SetUp(ActorResponse actor, bool ally)
	{
		_actor = actor;
		_groupName.text = actor.Name;
		_ally.gameObject.SetActive(!ally);
		_ally.onClick.RemoveAllListeners();
		_ally.onClick.AddListener(AllianceRequest);
		_ally.onClick.AddListener(() => _ally.gameObject.SetActive(false));
	}

	private void AllianceRequest()
	{
		Loading.Start();
		SUGARManager.Client.AllianceClient.CreateAllianceRequestAsync(new RelationshipRequest { RequestorId = SUGARManager.CurrentGroup.Id, AcceptorId = _actor.Id, AutoAccept = true },
			success =>
			{
				Loading.Stop();
			}, error =>
			{
				Loading.Stop();
			});
	}
}
