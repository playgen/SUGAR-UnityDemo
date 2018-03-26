using PlayGen.SUGAR.Unity;

using UnityEngine;
using UnityEngine.UI;

public class UserSidePanel : MonoBehaviour {

	[SerializeField]
	private Button _details;

	public void Display()
	{
		gameObject.SetActive(true);
		_details.onClick.RemoveAllListeners();
		_details.onClick.AddListener(() => GetComponentInParent<Canvas>().GetComponentInChildren<UserInfoPanel>(true).Display(SUGARManager.CurrentUser.Id));
		_details.GetComponentInChildren<Text>().text = SUGARManager.CurrentUser.Name;
	}
}
