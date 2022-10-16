using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : BasePanel
{
	private CanvasGroup canvasGroup;
	public Button attackBtn;
	public Button moveBtn;
    public void Start()
    {
		attackBtn.onClick.AddListener(ShowSkillUI);
		moveBtn.onClick.AddListener(ReadyToMove);
    }

    private void ReadyToMove()
    {
		UpdataManager.Instance.skillLineOpen = true;
		UpdataManager.Instance.moveButtonPushed = true;
		UpdataManager.Instance.skillButtonPushed = false;
	}

	private void ShowSkillUI()
    {
		UIManager.Instance.PushPanel(UIPanelType.SkillChoosePanel);
		UpdataManager.Instance.skillButtonPushed = true;
		UpdataManager.Instance.moveButtonPushed = false;
    }

}
