using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UIManager {

	private Dictionary<UIPanelType, string> panelPathDict; //存储所有面板prefab的路径

	public Dictionary<UIPanelType, BasePanel> panelDict; //保存所有实例化面板的游戏物体身上的BasePanel组件

	private Stack<BasePanel> panelStack;  //保存打开页面的关系 使用栈保存 后进先出的功能

	private static UIManager instance;
	public static UIManager Instance
	{
		get {
			if (instance == null)
			{
				instance = new UIManager();
			}
			return instance;
		}
	}

	private Transform canvasTransform;
	public Transform CanvasTransform
	{
		get {
			if (canvasTransform == null)
			{
				canvasTransform = GameObject.Find("Canvas").GetComponent<Transform>();
			}
			return canvasTransform;
		}
	}

	private UIManager()
	{
		ParseUIPanelTypeJson();
	}

	/// <summary>
	/// 把某个页面入栈，把页面显示在界面上
	/// </summary>
	public BasePanel PushPanel(UIPanelType panelType)
	{
		if (panelStack == null)
			panelStack = new Stack<BasePanel>();

		//判断栈中是否有页面 如果有就暂停
		if (panelStack.Count > 0)
		{
			BasePanel topPanel = panelStack.Peek();
			topPanel.OnPause();
		}

		BasePanel panel = GetPanel(panelType);
       
		//将新的页面入栈
		panel.OnEnter();
		panelStack.Push(panel);
        //显示在最前面
        panel.transform.SetAsLastSibling();
        return panel;
	}


	/// <summary>
	/// 出栈，把页面从界面上移除
	/// </summary>
	public void PopPanel()
	{
		if (panelStack == null)
			panelStack = new Stack<BasePanel>();
		if (panelStack.Count <= 0) return;
		//关闭
		BasePanel topPanel = panelStack.Pop();
		topPanel.OnExit();
		//恢复点击
		if (panelStack.Count <= 0) return;
		BasePanel nexTopPanel = panelStack.Peek();
		nexTopPanel.OnResume();
	}
	public BasePanel GetPanel(UIPanelType panelType)
	{
		if (panelDict == null)
		{
			panelDict = new Dictionary<UIPanelType, BasePanel>();
		}
		BasePanel panel = panelDict.NewTryGetValue(panelType);
		if (panel == null)
		{
			string path = panelPathDict.NewTryGetValue(panelType);
			GameObject instPanel = GameObject.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
			instPanel.transform.SetParent(CanvasTransform, false);
			panel = instPanel.GetComponent<BasePanel>();
			panelDict.Add(panelType, panel);
		}
		return panel;
	}

	[Serializable]
	class UIPanelTypeJson
	{
		public List<UIPanelInfo> infoList;
	}

	private void ParseUIPanelTypeJson()
	{
		panelPathDict = new Dictionary<UIPanelType, string>();
		TextAsset ta = Resources.Load<TextAsset>("UIPanelType");

        UIPanelTypeJson infoListJson = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

		foreach (UIPanelInfo info in infoListJson.infoList)
		{
			panelPathDict.Add(info.panelType, info.path);
		}
	}
}


