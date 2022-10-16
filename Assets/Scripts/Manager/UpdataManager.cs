using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdataManager : Singleton<UpdataManager>
{   /// <summary>
    /// 技能范围指示的开关检测
    /// </summary>
    public bool skillMarkOpen = false;
    /// <summary>
    /// 技能箭头指示物的开关
    /// </summary>
    public bool skillLineOpen = false;
    /// <summary>
    /// UI面板的技能按钮已经按下了
    /// </summary>
    public bool skillButtonPushed = false;
    /// <summary>
    /// UI面板的移动按钮已经被按下了
    /// </summary>
    public bool moveButtonPushed = false;
    /// <summary>
    /// 事件是否正在执行
    /// </summary>
    public bool eventHappen = false;

    public bool skillUseButton = false;
    /// <summary>
    /// 技能已经释放
    /// </summary>
    public bool skillRealess = false;
    //private static UpdataManager instance;
    //public static UpdataManager Instance
    //{
    //    get
    //    {

    //        if (instance == null)
    //        {
    //            instance = new UpdataManager();
    //        }
    //        return instance;
    //    }
    //}
    // Start is called before the first frame update
    void Start()
    {
        skillMarkOpen = false;
        skillLineOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
