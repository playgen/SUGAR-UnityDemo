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
	private GameObject _nameGroup;
	[SerializeField]
	private Text _username;
	[SerializeField]
	private Text _groupName;
	[SerializeField]
	private List<int> _groupIds;
	[SerializeField]
	private List<int> _userIds;
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
		_nameGroup.SetActive(false);
		_resourcePanel.gameObject.SetActive(false);
		_title.text = "SUGAR Sign-in";
		_characterText.text = "Register to start the demo and get 100 chocolate!";
		SUGARManager.Account.DisplayPanel(success =>
		{
			if (success)
			{
				_nameGroup.SetActive(true);
				_username.text = SUGARManager.CurrentUser.Name;
				SUGARManager.Resource.Add("Chocolate", 100, resourceSuccess => { });
				SUGARManager.Client.Group.CreateAsync(new GroupRequest { Name = SUGARManager.CurrentUser.Name + "'s Group" },
				groupMade =>
				{
					SUGARManager.CurrentGroup = groupMade;
					_groupName.text = SUGARManager.CurrentGroup.Name;
					_currentStep = 1;
					_previous.gameObject.SetActive(true);
					_next.gameObject.SetActive(true);
					LoadStep();
				},
				groupError =>
				{
					
				});
			}
		});
	}

	public void UpdateGroup(ActorResponse actor)
	{
		
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
		if (_currentStep < 9)
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
				_resourcePanel.gameObject.SetActive(false);
				break;
			case 5:
				SUGARManager.Evaluation.Hide();
				break;
			case 6:
				SUGARManager.Evaluation.Hide();
				break;
			case 7:
				SUGARManager.Leaderboard.Hide();
				break;
			case 8:
				SUGARManager.Evaluation.Hide();
				break;
			case 9:
				SUGARManager.Evaluation.Hide();
				break;
		}
	}

	private void LoadStep()
	{
		_previous.interactable = _currentStep > 1;
		_next.interactable = _currentStep < 9;
		switch (_currentStep)
		{
			case 1:
				_groupInfoPanel.Display(_groupIds, _userIds);
				_title.text = "Make Relationships";
				_characterText.text = "You can create friendships with users, add users to your group and make alliances with other groups";
				break;
			case 2:
				SUGARManager.Leaderboard.Display("MOST_CHOCOLATE_USER", PlayGen.SUGAR.Common.LeaderboardFilterType.Top);
				_title.text = "User Leaderboard";
				_characterText.text = "You can see how your chocolate count compares to other players";
				break;
			case 3:
				_resourcePanel.DisplayUser(_userIds);
				_title.text = "User Resoruce Sharing";
				_characterText.text = "You can cooperate by sharing your chocolate with your friends and your group";
				break;
			case 4:
				_resourcePanel.DisplayGroup(_userIds, _groupIds);
				_title.text = "Group Resoruce Sharing";
				_characterText.text = "Your group can cooperate by giving chocolate to its members or allied groups";
				break;
			case 5:
				SUGARManager.Evaluation.DisplayAchievementList();
				_title.text = "User Achievements";
				_characterText.text = "Your actions earn you progress toward achievements";
				break;
			case 6:
				SUGARManager.Evaluation.DisplaySkillList();
				_title.text = "User Skills";
				_characterText.text = "Cooperating earns you progress toward completing this skill";
				break;
			case 7:
				SUGARManager.Leaderboard.Display("MOST_CHOCOLATE_GROUP", PlayGen.SUGAR.Common.LeaderboardFilterType.Top);
				_title.text = "Group Leaderboard";
				_characterText.text = "Transferring chocolate to your group will help them rank more highly";
				break;
			case 8:
				SUGARManager.Evaluation.DisplayGroupAchievementList();
				_title.text = "Group Achievements";
				_characterText.text = "Achievements can also be associated with groups";
				break;
			case 9:
				SUGARManager.Evaluation.DisplayGroupSkillList();
				_title.text = "Group Skills";
				_characterText.text = "Achievements and skills for groups can also be based on the actions of their members";
				break;
		}
		var buttons = new List<GameObject> { _previous.gameObject, _next.gameObject };
		buttons.BestFit();
	}
}
