using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCountDown : Singleton<TimeCountDown>
{
    public List<float> clocks;
    public Dictionary<string,float> clocksSerch;
    private float time;
    private int index;
    private bool canCount = false;
    private string name;
    // Start is called before the first frame update

    public void AddClock(float time, bool isState, string name)
    {
        clocksSerch = new Dictionary<string, float>();
        this.time = time;
       // clock.isState = isState;
        this.name = name;
        clocks.Add(time);
        clocksSerch.Add(name,time);
        canCount = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Œ“‘⁄≈‹");
        if (canCount)
        {
            index = clocks.ToArray().Length;
            if (index != 0)
            {
                for (int i = 0; i < index; i++)
                {
                    if (clocks[i] > 0)
                    { 
                        clocks[i] -= 1 * Time.deltaTime;
                        //Debug.Log(Mathf.CeilToInt(clocks[i]) + " this is list ");
                    }
                    else
                    {                   
                            clocksSerch.Remove(name);
                            clocks.Remove(clocks[i]);
                    }
                }
            }
        }
    }
}
