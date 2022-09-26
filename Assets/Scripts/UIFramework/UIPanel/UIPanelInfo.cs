using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class UIPanelInfo:ISerializationCallbackReceiver {
	[NonSerialized]
	public UIPanelType panelType;
	public string panelTypeString;
	public string path;


	/// <summary>
	/// 反序列化之后
	/// </summary>
	public void OnAfterDeserialize()
	{
		UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
		panelType = type;
	}

	/// <summary>
	/// 在序列化之前
	/// </summary>
	public void OnBeforeSerialize()
	{

	}
}
