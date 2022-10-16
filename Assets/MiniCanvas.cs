using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniCanvas : MonoBehaviour
{
    public Slider bloodbar;
    public Slider bloodFloor;
    private bool FloorStart=false;
    private float FloorTime = 0;
    public float FloorSpeed = 1;
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
    public void OnHpChange()
    {
        bloodbar.value = character.hp / character.maxHp;
        FloorStart = true;
        FloorTime = 0;
    }
    private void Update()
    {
        if (FloorStart&&FloorTime<1)
        {
            FloorTime += Time.deltaTime *FloorSpeed;
            bloodFloor.value = Mathf.Lerp(bloodFloor.value, bloodbar.value,FloorTime);
        }
        if (FloorTime>=1)
        {
            FloorTime = 0;
            FloorStart = false;
        }
    }
    //private void Update()
    //{
    //    if(this.GetComponentInParent<CharacterData>().unit is Player)
    //    {
    //         bloodbar.GetComponent<Slider>().value = character.hp / character.maxHp;
    //    }
    //    else if(this.GetComponentInParent<CharacterData>().unit is Enemy)
    //    {
    //        bloodbar.GetComponent<Slider>().value = character.hp / character.maxHp;
    //    }
    //}
    // Start is called before the first frame update
    private void LateUpdate()
    {
        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(0, 180, 0);
    }
}
