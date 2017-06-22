using System;
using UnityEngine;
using PlayGen.SUGAR.Client;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
	private SkillClient _skillClient;

	public GameObject SkillPanel;
	public GameObject SkillItemPrefab;
	public GameObject SkillList;

	void Awake()
	{
		_skillClient = ScriptLocator.Controller.Factory.Skill;
	}

	public void UpdateList()
	{
		//Remove old achievemnts list
		foreach (Transform child in SkillList.transform)
		{
			Destroy(child.gameObject);
		}
		try
		{
			var responses = _skillClient.GetGameProgress(ScriptLocator.Controller.GameId, ScriptLocator.Controller.UserId.Value);
			int counter = 0;
			var listRect = SkillList.GetComponent<RectTransform>().rect;
			foreach (var response in responses)
			{
				var skillItem = Instantiate(SkillItemPrefab);
				skillItem.transform.SetParent(SkillList.transform, false);
				var itemRectTransform = skillItem.GetComponent<RectTransform>();
				itemRectTransform.sizeDelta = new Vector2(listRect.width, listRect.height / 5);
				itemRectTransform.anchoredPosition = new Vector2(0, (counter * -(listRect.height / 5)));
				skillItem.transform.Find("Name").GetComponent<Text>().text = response.Name + ":";
				skillItem.transform.Find("Bar").GetComponent<Image>().fillAmount = response.Progress;
				counter++;
			}
		   
		}
		catch (Exception exception)
		{
			Debug.LogError("Update Resource List Failed: " + exception);
		}
	}
}
