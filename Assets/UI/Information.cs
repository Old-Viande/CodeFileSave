using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
    public GameObject Panel;
    public Text text;
    public Text bartext;
    public GameObject bloodbar;
    public Player character;
    public GameObject Move;
    public GameObject Attack;
    public GameObject Defence;
    public enum State
    {
        Idle,
        Attack,
        Deffence,
        Move
    }

    public State currentState;

    private void Start()
    {
        //character = CreateManager.Instance.player.GetComponent<PlayerData>().player;

       
    }
    private void Update()
    {
        character = (Player)CreateManager.Instance.player.GetComponent<PlayerData>().unit;
        currentState = (State)character.currentState;
        bloodbar.GetComponent<Slider>().value = character.hp / character.maxHp;
        bartext.text = character.hp + " / " + character.maxHp;
        text.text = character.name;
        switch (currentState)
        {
            case State.Idle:
                Attack.SetActive(true);
                Move.SetActive(true);
                Defence.SetActive(true);
                break;
            case State.Attack:
                Attack.SetActive(true);
                Move.SetActive(false);
                Defence.SetActive(false);
                break;
            case State.Move:
                Attack.SetActive(false);
                Move.SetActive(true);
                Defence.SetActive(false);
                break;
            case State.Deffence:
                Attack.SetActive(false);
                Move.SetActive(false);
                Defence.SetActive(true);
                break;
        }

    }

}
