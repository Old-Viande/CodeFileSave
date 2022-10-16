using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdataManager : Singleton<UpdataManager>
{   /// <summary>
    /// ���ܷ�Χָʾ�Ŀ��ؼ��
    /// </summary>
    public bool skillMarkOpen = false;
    /// <summary>
    /// ���ܼ�ͷָʾ��Ŀ���
    /// </summary>
    public bool skillLineOpen = false;
    /// <summary>
    /// UI���ļ��ܰ�ť�Ѿ�������
    /// </summary>
    public bool skillButtonPushed = false;
    /// <summary>
    /// UI�����ƶ���ť�Ѿ���������
    /// </summary>
    public bool moveButtonPushed = false;
    /// <summary>
    /// �¼��Ƿ�����ִ��
    /// </summary>
    public bool eventHappen = false;

    public bool skillUseButton = false;
    /// <summary>
    /// �����Ѿ��ͷ�
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
