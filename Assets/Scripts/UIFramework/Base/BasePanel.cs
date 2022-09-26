using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasePanel : MonoBehaviour {




	/// <summary>
	/// 界面显示出来
	/// </summary>
	public virtual void OnEnter()
	{

	}
	/// <summary>
	/// 界面暂停（弹出其他界面）
	/// </summary>
	public virtual void OnPause()
	{

	}
	/// <summary>
	/// 界面继续（其他界面移除，恢复本界面的交互）
	/// </summary>
	public virtual void OnResume()
	{

	}
	/// <summary>
	/// 界面被移除，从页面移除不显示 界面关闭
	/// </summary>
	public virtual void OnExit()
	{

	}
}
