using UnityEngine;
[System.Serializable]
public class Character
{

    public string name;

    public string UID;

    [HideInInspector]
	public float hp;

	public float maxHp;

    public float speed;

    public float attack;

    public float defense;

    [HideInInspector]
	public int actionPoint;

	public int maxActionPoint;

    public bool isPlayer;

    public int attackRange;

    public GameObject Item;

    public enum State
    {
        Idle,
        Attack,
        Deffence,
        Move
    }

    public State currentState;


}
