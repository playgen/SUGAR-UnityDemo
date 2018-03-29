using System.Linq;

using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.Loading;

using UnityEngine;
using UnityEngine.UI;

public class ResourcePrefab : MonoBehaviour {

	[SerializeField]
	private Text _name;
	[SerializeField]
	private Button _giveOne;
	[SerializeField]
	private Button _giveFive;
	[SerializeField]
	private Button _giveTen;
	[SerializeField]
	private Button _giveTwentyFive;
	private ActorResponse _actor;

	public void SetUp(ActorResponse actor)
	{
		_actor = actor;
		_name.text = actor.Name;
		_giveOne.onClick.RemoveAllListeners();
		_giveFive.onClick.RemoveAllListeners();
		_giveTen.onClick.RemoveAllListeners();
		_giveTwentyFive.onClick.RemoveAllListeners();
		_giveOne.onClick.AddListener(() => Loading.Start());
		_giveFive.onClick.AddListener(() => Loading.Start());
		_giveTen.onClick.AddListener(() => Loading.Start());
		_giveTwentyFive.onClick.AddListener(() => Loading.Start());
		if (ResourcePanel.User)
		{
			_giveOne.onClick.AddListener(() => SUGARManager.Resource.Transfer(_actor.Id, "Chocolate", 1, success =>
			{
				if (success)
				{
					SUGARManager.GameData.Send("CHOCOLATE_SHARED", 1);
				}
				GetComponentInParent<Canvas>().GetComponentsInChildren<ResourcePrefab>(true).ToList().ForEach(r => r.UpdateCount());
				Loading.Stop();
			}));
			_giveFive.onClick.AddListener(() => SUGARManager.Resource.Transfer(_actor.Id, "Chocolate", 5, success =>
			{
				if (success)
				{
					SUGARManager.GameData.Send("CHOCOLATE_SHARED", 5);
				}
				GetComponentInParent<Canvas>().GetComponentsInChildren<ResourcePrefab>(true).ToList().ForEach(r => r.UpdateCount());
				Loading.Stop();
			}));
			_giveTen.onClick.AddListener(() => SUGARManager.Resource.Transfer(_actor.Id, "Chocolate", 10, success =>
			{
				if (success)
				{
					SUGARManager.GameData.Send("CHOCOLATE_SHARED", 10);
				}
				GetComponentInParent<Canvas>().GetComponentsInChildren<ResourcePrefab>(true).ToList().ForEach(r => r.UpdateCount());
				Loading.Stop();
			}));
			_giveTwentyFive.onClick.AddListener(() => SUGARManager.Resource.Transfer(_actor.Id, "Chocolate", 25, success =>
			{
				if (success)
				{
					SUGARManager.GameData.Send("CHOCOLATE_SHARED", 25);
				}
				GetComponentInParent<Canvas>().GetComponentsInChildren<ResourcePrefab>(true).ToList().ForEach(r => r.UpdateCount());
				Loading.Stop();
			}));
		}
		else
		{
			_giveOne.onClick.AddListener(() => SUGARManager.Client.Resource.TransferAsync(new ResourceTransferRequest{SenderActorId = SUGARManager.CurrentGroup.Id, GameId = SUGARManager.GameId, Key = "Chocolate", Quantity = 1, RecipientActorId = _actor.Id}, success =>
			{
				GetComponentInParent<Canvas>().GetComponentsInChildren<ResourcePrefab>(true).ToList().ForEach(r => r.UpdateCount());
				Loading.Stop();
			},
			error =>
			{
					
			}));
			_giveFive.onClick.AddListener(() => SUGARManager.Client.Resource.TransferAsync(new ResourceTransferRequest { SenderActorId = SUGARManager.CurrentGroup.Id, GameId = SUGARManager.GameId, Key = "Chocolate", Quantity = 5, RecipientActorId = _actor.Id }, success =>
			{
				GetComponentInParent<Canvas>().GetComponentsInChildren<ResourcePrefab>(true).ToList().ForEach(r => r.UpdateCount());
				Loading.Stop();
			},
			error =>
			{

			}));
			_giveTen.onClick.AddListener(() => SUGARManager.Client.Resource.TransferAsync(new ResourceTransferRequest { SenderActorId = SUGARManager.CurrentGroup.Id, GameId = SUGARManager.GameId, Key = "Chocolate", Quantity = 10, RecipientActorId = _actor.Id }, success =>
			{
				GetComponentInParent<Canvas>().GetComponentsInChildren<ResourcePrefab>(true).ToList().ForEach(r => r.UpdateCount());
				Loading.Stop();
			},
			error =>
			{

			}));
			_giveTwentyFive.onClick.AddListener(() => SUGARManager.Client.Resource.TransferAsync(new ResourceTransferRequest { SenderActorId = SUGARManager.CurrentGroup.Id, GameId = SUGARManager.GameId, Key = "Chocolate", Quantity = 25, RecipientActorId = _actor.Id }, success =>
			{
				GetComponentInParent<Canvas>().GetComponentsInChildren<ResourcePrefab>(true).ToList().ForEach(r => r.UpdateCount());
				Loading.Stop();
			},
			error =>
			{

			}));
		}
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
		if (ResourcePanel.User)
		{
			long choc;
			SUGARManager.Resource.UserGameResources.TryGetValue("Chocolate", out choc);
			_giveOne.interactable = choc >= 1;
			_giveFive.interactable = choc >= 5;
			_giveTen.interactable = choc >= 10;
			_giveTwentyFive.interactable = choc >= 25;
		}
		else
		{
			SUGARManager.Client.Resource.GetAsync(SUGARManager.GameId, SUGARManager.CurrentGroup.Id, new[] { "Chocolate" }, success =>
			{
				var choc = success.First(s => s.Key == "Chocolate").Quantity;
				_giveOne.interactable = choc >= 1;
				_giveFive.interactable = choc >= 5;
				_giveTen.interactable = choc >= 10;
				_giveTwentyFive.interactable = choc >= 25;
			}, error =>
			{

			});
		}
		
	}
}
