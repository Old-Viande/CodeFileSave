using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureData : MonoBehaviour
{
    public enum FurnitureType
    {
        OneByOne,
        OneByTwo
    }

    [SerializeField]private FurnitureType m_FurnitureType;
    public FurnitureType Type
    {
        get
        {
            return m_FurnitureType;
        }
    }
}
