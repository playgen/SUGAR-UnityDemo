using PlayGen.SUGAR.Unity;

using UnityEngine;
using UnityEngine.UI;

public class GroupSidePanel : MonoBehaviour {

	[SerializeField]
	private Button _details;

	public void Display()
	{
		gameObject.SetActive(true);
		_details.onClick.RemoveAllListeners();
		_details.onClick.AddListener(() => GetComponentInParent<Canvas>().GetComponentInChildren<GroupInfoPanel>(true).Display(SUGARManager.CurrentGroup.Id));
		_details.GetComponentInChildren<Text>().text = SUGARManager.CurrentGroup.Name;
	}
}
