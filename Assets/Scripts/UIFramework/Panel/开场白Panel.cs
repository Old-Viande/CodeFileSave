using UnityEngine;

public class 开场白Panel : BasePanel
{
    private CanvasGroup canvasGroup;

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
		this.gameObject.SetActive(false);

	}

	public override void OnResume()
	{
		this.gameObject.SetActive(true);

		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	public void OnConversationEnd()
    {
		UIManager.Instance.PushPanel(UIPanelType.Main);
    }
}
