using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Information : MonoBehaviour
{
    public GameObject Panel;
    public Text text;
    public Text bartext;
    public GameObject bloodbar;
    public TextMeshProUGUI roundText; //UI²ã×¨ÓÃTMP
    public Player character;
    //public GameObject Move;
    //public GameObject Attack;
    //public GameObject Defence;
    private int roundcount=1;
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
        float numb = (float)character.actionPoint / (float)character.maxActionPoint;
    
        if (character.actionPoint == 0&& Input.GetMouseButtonUp(0))
        {
            roundcount++;
        }
        roundText.text = "Round £º" + roundcount;
        bloodbar.GetComponent<Slider>().value = numb;
        bartext.text ="ActionPoint :"+ character.actionPoint + " / " + character.maxActionPoint;
        text.text = character.name;
        //switch (currentState)
        //{
        //    case State.Idle:
        //        Attack.SetActive(true);
        //        Move.SetActive(true);
        //        Defence.SetActive(true);
        //        break;
        //    case State.Attack:
        //        Attack.SetActive(true);
        //        Move.SetActive(false);
        //        Defence.SetActive(false);
        //        break;
        //    case State.Move:
        //        Attack.SetActive(false);
        //        Move.SetActive(true);
        //        Defence.SetActive(false);
        //        break;
        //    case State.Deffence:
        //        Attack.SetActive(false);
        //        Move.SetActive(false);
        //        Defence.SetActive(true);
        //        break;
        //}

    }

}
