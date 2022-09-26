using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对Dictionary的扩展
/// </summary>
public static class DictionaryExtension {
	/// <summary>
	/// 尝试根据Key得到对应的Value，没有得到则返回null
	/// </summary>
	/// <param name=""></param>
	/// <param name=""></param>
	public static Tvalue NewTryGetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
	{
		Tvalue value;
		dict.TryGetValue(key, out value);
		return value;
	}

   
}
