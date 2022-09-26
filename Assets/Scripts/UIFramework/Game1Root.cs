using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game1Root : MonoBehaviour
{



    private void Awake()
    {
        UIManager.Instance.PushPanel(UIPanelType.Start);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
