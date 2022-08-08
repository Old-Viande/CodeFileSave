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
    // Start is called before the first frame update
    private void Awake()
    {
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }
        
    }
    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (arreyx < GridCreate.Instance.x - 1)
            {
                arreyx++;
            }
            else
            {
                arreyx = GridCreate.Instance.x - 1;
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
            if (arreyz < GridCreate.Instance.z - 1)
            {
                arreyz++;
            }
            else
            {
                arreyz = GridCreate.Instance.z - 1;
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
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        Vector3 lookatposition = GridCreate.Instance.gridmap.GetGridCenter(arreyx, arreyz);
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
        }
        
        contral.transform.position = new Vector3(MathF.Cos(Mathf.Deg2Rad*direction) * radius + lookatposition.x, lookatposition.y + height, MathF.Sin(Mathf.Deg2Rad * direction) * radius + lookatposition.z);
        contral.transform.LookAt(GridCreate.Instance.gridmap.GetGridCenter(arreyx, arreyz));
    }
    

}
