using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove :MonoBehaviour
{

    public int arreyx;
    public int arreyz;
    public float height;
    public GameObject contral;
    public float radius =0.5f;
    public float direction = 0f;
    public float speed = 10f ;
    private Vector3 lookatposition;
    // Start is called before the first frame update
    private void Awake()
    {
             
    }
    void Start()
    {
        
    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.D))
        {
            if (arreyx < GridCreate.Instance.weith - 1)
            {
                arreyx++;
            }
            else
            {
                arreyx = GridCreate.Instance.weith - 1;
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (arreyx > 0)
            {
                arreyx--;
            }
            else
            {
                arreyx = 0;
            }

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (arreyz < GridCreate.Instance.height - 1)
            {
                arreyz++;
            }
            else
            {
                arreyz = GridCreate.Instance.height - 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (arreyz > 0)
            {
                arreyz--;
            }
            else
            {
                arreyz = 0;
            }
        }*/
    }
    // Update is called once per frame
    void FixedUpdate()
    {


        // Vector3 lookatposition = GridManager.Instance.gridmap.GetGridCenter(0, 0);
        if (DataSave.Instance.currentObj != null)
        {
             lookatposition = DataSave.Instance.currentObj.transform.position;
        }
        else
        {
             lookatposition = GridManager.Instance.roomGridmap.GetGridCenter(0, 0);
        }
       
        if (Input.GetMouseButton(1))
        {
            direction+=Input.GetAxis("Mouse X")*speed;
            if (direction > 330f)
            {
                direction -= 360f;
            }else if(direction <= -30f)
            {
                direction += 360f;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                height += Input.GetAxis("Mouse Y") * speed;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            radius--;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            radius++;
        }

            contral.transform.position = new Vector3(MathF.Cos(Mathf.Deg2Rad*direction) * radius + lookatposition.x, lookatposition.y + height, MathF.Sin(Mathf.Deg2Rad * direction) * radius + lookatposition.z);
        //contral.transform.LookAt(GridManager.Instance.gridmap.GetGridCenter(0, 0));
        contral.transform.LookAt(lookatposition);
    }
    

}
