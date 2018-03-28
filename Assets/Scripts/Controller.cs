using System.Collections.Generic;

using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;
using PlayGen.Unity.Utilities.BestFit;

using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
	private int _currentStep;
	[SerializeField]
	private Button _previous;
	[SerializeField]
	private Button _next;
	[SerializeField]
	private List<int> _groupIds;
	[SerializeField]
	private List<int> _userIds;
	[SerializeField]
	private GroupPanel _groupPanel;
	[SerializeField]
	private GroupInfoPanel _groupInfoPanel;
	[SerializeField]
	private ResourcePanel _resourcePanel;
	[SerializeField]
	private Text _title;
	[SerializeField]
	private Text _characterText;

	void Awake()
	{
		ConsoleDebugRedirect.Redirect();
		_previous.gameObject.SetActive(false);
		_next.gameObject.SetActive(false);
		_groupInfoPanel.gameObject.SetActive(false);
		_groupPanel.gameObject.SetActive(false);
		_resourcePanel.gameObject.SetActive(false);
		_title.text = "SUGAR Sign-in";
		_characterText.text = "Register to start the demo and get 100 chocolate!";
		SUGARManager.Account.DisplayPanel(success =>
		{
			if (success)
			{
				SUGARManager.Resource.Add("Chocolate", 100, resourceSuccess => { });
				SUGARManager.Evaluation.ForceNotification("Chocolate Count: 100");
				_groupPanel.Display(_groupIds);
				_title.text = "Join a Group";
				_characterText.text = "Choose which group you want to join";
			}
		});
	}

	public void UpdateGroup(ActorResponse actor)
	{
		SUGARManager.CurrentGroup = actor;
		_groupPanel.gameObject.SetActive(false);
		_currentStep = 1;
		_previous.gameObject.SetActive(true);
		_next.gameObject.SetActive(true);
		LoadStep();
	}

	public void Previous()
	{
		HideCurrent();
		if (_currentStep > 1)
		{
			_currentStep--;
			LoadStep();
		}
	}

	public void Next()
	{
		HideCurrent();
		if (_currentStep < 8)
		{
			_currentStep++;
			LoadStep();
		}
	}

	private void HideCurrent()
	{
		switch (_currentStep)
		{
			case 1:
				_groupInfoPanel.gameObject.SetActive(false);
				break;
			case 2:
				SUGARManager.Leaderboard.Hide();
				break;
			case 3:
				_resourcePanel.gameObject.SetActive(false);
				break;
			case 4:
				SUGARManager.Evaluation.Hide();
				break;
			case 5:
				SUGARManager.Evaluation.Hide();
				break;
			case 6:
				SUGARManager.Leaderboard.Hide();
				break;
			case 7:
				SUGARManager.Evaluation.Hide();
				break;
			case 8:
				SUGARManager.Evaluation.Hide();
				break;
		}
	}

	private void LoadStep()
	{
		_previous.interactable = _currentStep > 1;
		_next.interactable = _currentStep < 8;
		switch (_currentStep)
		{
			case 1:
				_groupInfoPanel.Display(_groupIds, _userIds);
				_title.text = "Group Relationships";
				_characterText.text = "Here are your fellow group members and the other groups yours is allied with";
				break;
			case 2:
				SUGARManager.Leaderboard.Display("MOST_CHOCOLATE_USER", PlayGen.SUGAR.Common.LeaderboardFilterType.Top);
				_title.text = "User Leaderboard";
				_characterText.text = "See how your chocolate count compares to others";
				break;
			case 3:
				_resourcePanel.Display(_userIds);
				_title.text = "Resoruce Sharing";
				_characterText.text = "You've got plenty of chocolate to share with your group and friends";
				break;
			case 4:
				SUGARManager.Evaluation.DisplayAchievementList();
				_title.text = "User Achievements";
				_characterText.text = "Your actions earn you progress toward achievements. How have you been doing?";
				break;
			case 5:
				SUGARManager.Evaluation.DisplaySkillList();
				_title.text = "User Skills";
				_characterText.text = "Cooperating earns you progress toward completing this skill";
				break;
			case 6:
				SUGARManager.Leaderboard.Display("MOST_CHOCOLATE_GROUP", PlayGen.SUGAR.Common.LeaderboardFilterType.Top);
				_title.text = "Group Leaderbaord";
				_characterText.text = "Transferring chocolate to your group will help them rank highly";
				break;
			case 7:
				SUGARManager.Evaluation.DisplayGroupAchievementList();
				_title.text = "Group Achievements";
				_characterText.text = "Groups also have achievements we keep track of";
				break;
			case 8:
				SUGARManager.Evaluation.DisplayGroupSkillList();
				_title.text = "Group Skills";
				_characterText.text = "Achievements and skills for groups can be based on the actions of their members as well as the group itself";
				break;
		}
		var buttons = new List<GameObject> { _previous.gameObject, _next.gameObject };
		buttons.BestFit();
	}
}
