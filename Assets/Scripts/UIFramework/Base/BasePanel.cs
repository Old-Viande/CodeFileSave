using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    /// <summary>
    /// 界面显示出来
    /// </summary>
    public virtual void OnEnter()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 界面暂停（弹出其他界面）
    /// </summary>
    public virtual void OnPause()
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// 界面继续（其他界面移除，恢复本界面的交互）
    /// </summary>
    public virtual void OnResume()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 界面被移除，从页面移除不显示 界面关闭
    /// </summary>
    public virtual void OnExit()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
