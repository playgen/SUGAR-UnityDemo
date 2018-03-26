using System.Linq;

using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Unity;

using UnityEngine;

public class Controller : MonoBehaviour
{
	[SerializeField]
	private UserSidePanel _userButtonPanel;
	[SerializeField]
	private GroupSidePanel _groupButtonPanel;
	[SerializeField]
	private int[] _groupIDs;
	[SerializeField]
	private GroupPanel _groupPanel;

	void Start()
	{
		ConsoleDebugRedirect.Redirect();
		_userButtonPanel.gameObject.SetActive(false);
		_groupButtonPanel.gameObject.SetActive(false);
		SUGARManager.Account.DisplayPanel(success =>
		{
			if (success)
			{
				_userButtonPanel.Display();
				SUGARManager.Resource.Add("Chocolate", 5, resourceSuccess => { });
				var groupMatch = SUGARManager.UserGroup.Groups.Where(g => _groupIDs.Contains(g.Actor.Id)).ToList();
				if (groupMatch.Count == 1)
				{
					UpdateGroup(groupMatch.First().Actor);
				}
				else if (groupMatch.Count > 1)
				{
					_groupPanel.DisplayPrimary(groupMatch.Select(g => g.Actor.Id).ToList());
				}
				else
				{
					_groupPanel.DisplayJoin(groupMatch.Select(g => g.Actor.Id).ToList());
				}
			}
		});
	}

	public void UpdateGroup(ActorResponse actor)
	{
		SUGARManager.CurrentGroup = actor;
		_groupButtonPanel.Display();
		_groupPanel.gameObject.SetActive(false);
	}

	private void DisplayGroups()
	{
		//TODO Display SU Group UI - maybe custom to restrict functionality for demo?
		//TODO This UI likely needs a way of selecting 'primary' group
		//TODO Need to pre-create groups and alliances between these groups
	}

	private void DisplayGroupMembers()
	{
		//TODO Display SU Group Member UI for primary group. Functionality should be inaccessible/redirect to group UI if user is in no groups.
	}

	private void DisplayFriends()
	{
		//TODO Display SU Friends UI - maybe custom to restrict functionality for demo?
	}

	private void DisplayLeaderboards()
	{
		//TODO Display SU leaderboard selection UI
	}

	private void DisplayGroupLeaderboard()
	{
		//TODO Display SU leaderboard selection UI
	}

	private void DisplayAchievements()
	{
		//TODO Display SU achievement UI for the user
	}

	private void DisplayGroupAchievements()
	{
		//TODO Display SU achievement UI for the group
		//TODO SU needs a way of showing group achievements
	}

	private void DisplayResources()
	{
		//TODO Display custom UI for seeing current user and primary group chocolate amounts and for giving chocolate to the group
	}
}
