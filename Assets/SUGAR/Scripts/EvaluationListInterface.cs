using System.Linq;

using PlayGen.SUGAR.Unity;

using UnityEngine;
using UnityEngine.UI;

using PlayGen.Unity.Utilities.BestFit;
using PlayGen.Unity.Utilities.Localization;

public class EvaluationListInterface : BaseEvaluationListInterface
{
	/// <summary>
	/// An array of the EvaluationItemInterfaces on this GameObject, set in the Inspector.
	/// </summary>
	[Tooltip("An array of the EvaluationItemInterfaces on this GameObject, set in the Inspector.")]
	[SerializeField]
	private EvaluationItemInterface[] _evaluationItems;

	/// <summary>
	/// Trigger DoBestFit method and add event listeners for when resolution and language changes.
	/// </summary>
	private void OnEnable()
	{
		BestFit.ResolutionChange += DoBestFit;
		Localization.LanguageChange += OnLanguageChange;
	}

	/// <summary>
	/// Remove event listeners.
	/// </summary>
	private void OnDisable()
	{
		BestFit.ResolutionChange -= DoBestFit;
		Localization.LanguageChange -= OnLanguageChange;
	}

	/// <summary>
	/// Set the pageNumber to 0 before displaying the UI.
	/// </summary>
	protected override void PreDisplay()
	{
	}

	/// <summary>
	/// Adjust EvaluationItemInterface pool to display a page of evaluations.
	/// </summary>
	protected override void Draw()
	{
		var evaluationList = SUGARManager.Evaluation.Progress.Take(_evaluationItems.Length).ToList();
		for (int i = 0; i < _evaluationItems.Length; i++)
		{
			if (i >= evaluationList.Count)
			{
				_evaluationItems[i].Disable();
			}
			else
			{
				_evaluationItems[i].SetText(evaluationList[i], Mathf.Approximately(evaluationList[i].Progress, 1.0f));
			}
		}
		DoBestFit();
	}

	/// <summary>
	/// If a user signs in via this panel, refresh the current page (which should be page 1).
	/// </summary>
	protected override void OnSignIn()
	{
		Show(true);
	}

	/// <summary>
	/// Set the text of all buttons and all evaluations to be as big as possible and the same size within the same grouping.
	/// </summary>
	private void DoBestFit()
	{
		_evaluationItems.Select(t => t.transform.Find("Name").gameObject).BestFit();
		_evaluationItems.Select(t => t.transform.Find("Description").gameObject).BestFit();
		GetComponentsInChildren<Button>().Select(t => t.gameObject).BestFit();
	}

	/// <summary>
	/// Refresh the current page to ensure any text set in code is also translated.
	/// </summary>
	private void OnLanguageChange()
	{
		Show(true);
	}
}
