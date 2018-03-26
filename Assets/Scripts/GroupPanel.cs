using System.Collections.Generic;
using PlayGen.SUGAR.Unity;

using UnityEngine;

public class GroupPanel : MonoBehaviour {

	[SerializeField]
	private Transform _container;
	[SerializeField]
	private GroupPrefab _prefab;

	public void DisplayPrimary(List<int> ids)
	{
		gameObject.SetActive(true);
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		foreach (var id in ids)
		{
			SUGARManager.Client.Group.GetAsync(id, success =>
			{
				var group = Instantiate(_prefab, _container, true);
				group.SetUp(success, true);
			}, error =>
			{

			});
		}
	}

	public void DisplayJoin(List<int> ids)
	{
		gameObject.SetActive(true);
		foreach (Transform child in _container)
		{
			Destroy(child.gameObject);
		}
		foreach (var id in ids)
		{
			SUGARManager.Client.Group.GetAsync(id, success =>
			{
				var group = Instantiate(_prefab, _container, true);
				group.SetUp(success, false);
			}, error =>
			{

			});
		}
	}
}
