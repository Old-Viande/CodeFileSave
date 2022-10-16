using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EventOption 
{
    [TextArea]
    public string optionText;
    /// <summary>
    /// 该选项所需的属性
    /// </summary>
    public CharacterProperty effectProperty;
    /// <summary>
    /// 属性检测合格所需的数值
    /// </summary>
    public int detect;
    /// <summary>
    /// 最小随机范围
    /// </summary>
    public float minRange;
    /// <summary>
    /// 最大随机范围
    /// </summary>
    public float maxRange;
    /// <summary>
    /// 吻合该数组内的数值时，判定为成功
    /// </summary>
    public float[] checkNumber;
}
