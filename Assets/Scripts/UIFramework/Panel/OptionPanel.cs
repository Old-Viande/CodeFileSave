using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : BasePanel
{
	private CanvasGroup canvasGroup;
	public Button attackBtn;
    public void Start()
    {
		attackBtn.onClick.AddListener(ShowSkillUI);
    }

    private void ShowSkillUI()
    {
		UIManager.Instance.PushPanel(UIPanelType.SkillChoosePanel);

    }

    public override void OnEnter()
	{
		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	public override void OnExit()
	{
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	public override void OnPause()
	{
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = 0.5f;
	}

	public override void OnResume()
	{
		canvasGroup.alpha = 1f;


		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

}
