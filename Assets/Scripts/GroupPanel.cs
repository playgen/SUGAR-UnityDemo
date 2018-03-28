using System.Collections.Generic;
using System.Linq;

using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.BestFit;
using PlayGen.Unity.Utilities.Loading;

using UnityEngine;
using UnityEngine.UI;

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
				var group = Instantiate(_prefab, _container, false);
				if (_container.childCount == ids.Count)
				{
					Loading.Stop();
				}
				_container.GetComponentsInChildren<Text>(true).Where(t => t.name == "Username").ToList().BestFit();
			}, error =>
			{

			});
		}
	}
}
