using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EventOption 
{
    [TextArea]
    public string optionText;
    /// <summary>
    /// ��ѡ�����������
    /// </summary>
    public CharacterProperty effectProperty;
    /// <summary>
    /// ���Լ��ϸ��������ֵ
    /// </summary>
    public int detect;
    /// <summary>
    /// ��С�����Χ
    /// </summary>
    public float minRange;
    /// <summary>
    /// ��������Χ
    /// </summary>
    public float maxRange;
    /// <summary>
    /// �Ǻϸ������ڵ���ֵʱ���ж�Ϊ�ɹ�
    /// </summary>
    public float[] checkNumber;
}
