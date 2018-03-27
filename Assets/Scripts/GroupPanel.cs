using System.Collections.Generic;
using System.Linq;

using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.Loading;

using UnityEngine;

public class GroupPanel : MonoBehaviour {

	[SerializeField]
	private Transform _container;
	[SerializeField]
	private GroupPrefab _prefab;

	public void Display(List<int> ids)
	{
		gameObject.SetActive(true);
		foreach (Transform child in _container)
		{
			Destroy(child.gameObject);
		}
		Loading.Start();
		foreach (var id in ids)
		{
			SUGARManager.Client.Group.GetAsync(id, success =>
			{
				var group = Instantiate(_prefab, _container, true);
				group.SetUp(success);
				if (_container.childCount == ids.Count)
				{
					Loading.Stop();
				}
			}, error =>
			{

			});
		}
	}
}
