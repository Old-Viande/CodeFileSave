using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public Character unit;
    public Transform hitPoint;
    public Transform carryPoint;
    public Transform maskPoint;
    public Animator anim;
    public AudioSource audioSour;
    public BuffManager buffManager;
    public void Update()
    {
        if (unit.hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    protected void Awake()
    {
        if (anim==null)
        {
            anim = GetComponentInChildren<Animator>();
            if (anim==null)
            {
                Debug.Log(unit.name + "û�з��ö���״̬��");

            }
        }
        if (audioSour== null)
        {
            audioSour = GetComponentInChildren<AudioSource>();
            if (audioSour == null)
            {
                Debug.Log(unit.name + "AudioNUll������tm���㴴����һ��");
                audioSour = this.gameObject.AddComponent<AudioSource>();
            }
        }
        buffManager = new();
    }

    
}
