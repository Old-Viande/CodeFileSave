using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class CameraTempController : MonoBehaviour
{

    public CatCameraController selfController;
    //震动相关的代码应该是要被调用的.在这里面安排好方法
    //锁定后平时相机的位置稍稍偏向锁定的事物,但是还是应该把角色当做主体,不需要缩放来补足.缩放对2.5D的游戏牺牲感太大了
    //除了锁定的事物还会和任务道具等选择的物体进行轻度相机位置混合.(调味料)
    /*
     * 
     * 损伤值不为零时线性降低.
     *  这个就直接写成多线程了,改变只是改变数量,不影响方法对其不断的衰减,方法做完bool false,方法做时true锁起来,写在set里
     * 
     * 相机抖动强弱=损伤值的指数倍.就是y=x`2(平方),或者y=x`3(立方)
     * 
     * 震动
     * 噪声要特制,一定不要做平平缓缓的噪声,那样就不是震动了.所以
     * Perlin.PerlinNoiseSeedCustom(shakeDamage*15, shakeDamage*15,Random.Range(0, 1000),0.8f接近1,子函数更大,3子函数长度缩放较低较明显)
     * angle = maxAngle * shake相机抖动强弱 * Perlin.PerlinNoiseSeedCustom
     * offsetx = maxOffset * shake 相机抖动强弱* Perlin.PerlinNoiseSeedCustom;
     * offsetY = maxOffset * shake相机抖动强弱 * Perlin.PerlinNoiseSeedCustom;
     * maxOffset和maxAngle代表倍率shake代表根据损伤值算出的相机抖动强度，取值范围[0,1]Perlin.PerlinNoiseSeedCustom()为生成随机浮点数的函数，获取[-1,1]之间的随机浮点数。
     * shakyCamera.angle = camera.angle + angle;
     * hakyCamera.center = camera.center + Vector2(offsetX,offsetY);
     * 然后我们每一帧为相机添加旋转和位移，值得注意的是，我们要保留相机的原先的旋转和位置，每一帧的位移和旋转都是基于最开始的相机参数。
     * 
     * 偏移
     * 超过距离阈值上限的接近系数为0
     * 在距离阈值下限以下的接近系数为1
     * 在距离阈值上下限之间的接近系数在[0,1]之间
     * ，在这区间平滑地改变接近系数。
     * 比如如果距离超过10我们对它不感兴趣，因为它离屏幕太远了，接近系数为0。
     * 如果距离为5的事物我们则感兴趣。
     * 在5-10之间，我们平滑地改变我们对这些事物的接近系数，
     * 比如距离为9的事物可能只有0.2的接近系数。
     *如果这些关注点有重要程度的差异，我们会用重要性×接近系数得到一个权重。
     * 然后我们把所有的关注点对应的数值加起来，用加权平均除以权重的总和，去看看最后把相机拉向哪里。
     */
    
    
    // Update is called once per frame
    //    0.01很慢，适合60帧游戏0.1是比较合理的快速0.5难以置信的快
    //FixedUpdate和late相性并不好
    
    
    //震动状态和震动时使用的Pos;
    private bool nowShaking = false;
    private Vector3 tempPos;//移动偏移过程中这个点位是改变的.才行,才足够正确
    private Quaternion tempRot;


  // 偏移相关
  // 值有变动的话, 就要检测偏移
     private void Update()
    {
        if (Input.GetKey(KeyCode.J))//测试震动
        {
            this.selfController.shakeDamage += 0.5f;
            OnSetShake();
        }
    }
    private void Start()
    {

        //Vector3 a=Vector3.zero;//事实证明里面的玩意不是引用类型,所以加了个Transform
        // this.ListenOffsetThing.Add(Vector3.zero);
        // this.ListenOffsetThing.Add(Vector3.forward);
        // this.ListenOffsetThing.Add(a);
        // this.ListenOffsetThing.Add(Vector3.forward);
        // this.ListenOffsetThing.Remove(a);
        // for (int i = 0; i < this.ListenOffsetThing.Count; i++)
        // {
        //     Debug.Log(this.ListenOffsetThing[i]+"序号"+i);
        // }
        selfController = new CatCameraController() { };
        selfController._cameraControllerRC = this;
    }

    void FixedUpdate()
    {
        //Debug.Log(Mathf.PerlinNoise(Random.Range(0f,1f), Random.Range(0f,1f)));
        DoShakeInFixedUpdate();
    }


    //需要震动就调这个
    public  void AddShake( float _AddShake)
    {
        //首先是改变shake的值,其次是通知RC开始执行shake的递减和震动;

        selfController.shakeDamage += _AddShake;
        selfController._cameraControllerRC.OnSetShake();

    }


   /////////////////////////////////// //震动相关////////////////////////////////////
    private void DoShakeInFixedUpdate()
    {
        if (this.nowShaking)
        {
            if (this.selfController.shakeDamage<=0f)
            {
                this.nowShaking = false;//关
                Transform transform1 = transform;
                transform1.position = this.tempPos;
                transform1.rotation = this.tempRot;
                this.selfController.shakeDamage = 0;
                return;
            }
            //先震
            ShakeCamera();
            //后减
            this.selfController.shakeDamage -=this.selfController.attenSpeed*Time.fixedDeltaTime*50;
           
        }
    }

    private void ShakeCamera()
    {
        // 5值=最大旋转or偏移 *shakeReal* Perlin.PerlinNoiseSeedCustom(shakeDamage,shakeDamage,种子(5个值各自用不同的),高度,密度)
        float shakeReal = math.pow(this.selfController.shakeDamage,2);
        float shakeDamage = this.selfController.shakeDamage;
        float angleZ=this.selfController.maxAngleZ*shakeReal*(float)Perlin.PerlinNoiseSeedCustom(shakeDamage*15, shakeDamage*15,Random.Range(0, 1000),0.8f,3);
        //只震旋转角
        float offsetX=this.selfController.maxoffsetX*shakeReal*(float)Perlin.PerlinNoiseSeedCustom(shakeDamage*15, shakeDamage*15,Random.Range(0, 1000),0.8f,3);
        float offsetY=this.selfController.maxoffsetZ*shakeReal*(float)Perlin.PerlinNoiseSeedCustom(shakeDamage*15, shakeDamage*15,Random.Range(0, 1000),0.8f,3);
        //没有前后抖动
        //这个就是真的加在local上
        this.transform.position =this.tempPos+ this.transform.TransformDirection(new Vector3(offsetX, offsetY,0 )) ;//temp点在移动时的处理和这里无关.
        //Debug.Log("angleZ"+angleZ+"震动系数"+shakeReal);
        
        //笑死我了.虽然这玩意用的是Local但是扭他的时候还是基于世界坐标,所以相机自己的z轴要换上他在世界的轴向
        this.transform.rotation=Quaternion.Euler(this.transform.TransformDirection(new Vector3(0,0,angleZ)))*this.tempRot;
    }
    /// <summary>
    /// 开始震动
    /// </summary>
    public  void OnSetShake()
    {
            //每帧去震动一次
            //每帧减少一定量//都在Fixed对齐拉倒
            if (!this.nowShaking)//震动的结束是震动自己判定(没得震了)
            {
                this.nowShaking = true;
                var transform1 = this.transform;
                this.tempPos = transform1.position;
                this.tempRot = transform1.rotation;
            }
    }
}
