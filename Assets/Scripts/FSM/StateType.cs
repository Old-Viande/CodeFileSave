using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    
    Unknown,//未知状态
    Start,//回合进入
    Running,//回合运行
    End,//回合结束
    Check,//检测状态
    Idel,//玩家闲置
    Attack,//玩家攻击   
    Skill,//玩家使用技能
    Move,//玩家移动
    EnemyIdel,//敌人闲置
    EnemyAttack,//敌人攻击
    EnemyMove,//敌人移动

}
