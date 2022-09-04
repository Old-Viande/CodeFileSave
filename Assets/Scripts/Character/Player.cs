using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player :Character
{

    public Mask mask;

    public Weapon mainWeapon;

    public Weapon subWeapon;

    public Equipment[] equipmentSave = new Equipment[4];
}
