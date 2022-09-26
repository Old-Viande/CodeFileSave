using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniCanvas : MonoBehaviour
{
    public GameObject bloodbar;
    private Character character;
    private void Start()
    {
        //if( this.GetComponentInParent<CharacterData>().unit is Player)
        // {
        //    // this.GetComponent<Slider>().value = character.hp / character.maxHp;
        // }
        // else
        // {

        // }
        character = this.GetComponentInParent<CharacterData>().unit;
    }
    private void Update()
    {
        if(this.GetComponentInParent<CharacterData>().unit is Player)
        {
             bloodbar.GetComponent<Slider>().value = character.hp / character.maxHp;
        }
        else if(this.GetComponentInParent<CharacterData>().unit is Enemy)
        {
            bloodbar.GetComponent<Slider>().value = character.hp / character.maxHp;
        }
    }
    // Start is called before the first frame update
    private void LateUpdate()
    {
        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(0, 180, 0);
    }
}
