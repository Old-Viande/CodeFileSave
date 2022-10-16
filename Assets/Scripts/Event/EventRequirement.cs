/// <summary>
/// 事件触发条件
/// </summary>
public enum EventTrigger
{
    [EnumName("当角色生成")]
    OnCreated = 0,
    OnDied = 1,
}

/// <summary>
/// 房间事件的效果触发类型
/// </summary>
public enum RoomEventEffectTrigger
{
    [EnumName("玩家进入房间时触发")]
    OnEnter = 0,
    [EnumName("玩家离开房间时触发")]
    OnLeave = 1,
    [EnumName("玩家停留房间（几回合时）触发")]
    OnStay = 2
}

/// <summary>
/// 事件触发前置条件
/// </summary>
public enum EventTriggerPre
{
    [EnumName("无")]
    None = 0,
    [EnumName("触发时间间隔")]
    TimeDuration = 1,
    [EnumName("随机触发（0~100）")]
    Random = 2,
    [EnumName("敌人数量")]
    EnemyCount = 3,
    [EnumName("房间数量")]
    RoomCount = 4,
    [EnumName("当前回合数")]
    RoundSpent = 5
}

/// <summary>
/// 事件执行效果,针对目标
/// </summary>
public enum EventEffectWithTarget
{
    [EnumName("改变数值")]
    ChangeValue = 0,
    [EnumName("改变Buff")]
    ChangeBuff = 1,
    [EnumName("改变技能")]
    ChangeSkill = 2,
    [EnumName("移动对象")]
    MoveObject = 3
}

/// <summary>
/// 事件执行效果,无目标
/// </summary>
public enum EventEffectWithoutTarget
{
    [EnumName("增加玩家角色")]
    AddPlayer = 0,
    [EnumName("移除玩家角色")]
    RemovePlayer = 1,
    [EnumName("生成敌人")]
    CreateEnemy = 2,
}

/// <summary>
/// 事件执行对象
/// </summary>
public enum EventTarget
{
    [EnumName("无")]
    None = 0,
    [EnumName("当前玩家")]
    CurrentPlayer = 1,
    [EnumName("所有玩家")]
    AllPlayer = 2,
    [EnumName("所有房间内玩家")]
    AllPlayerInRoom = 3,
    [EnumName("所有房间内玩家")]
    RandomPlayer = 4,
    [EnumName("所有敌人")]
    AllEnemy = 5,
    [EnumName("所有房间内敌人")]
    AllEnemyInRoom = 6,
    [EnumName("所有房间内玩家")]
    RandomEnemy = 7
}

/// <summary>
/// 事件执行对象筛选条件
/// </summary>
public enum EventTargetFilter
{
    [EnumName("无")]
    None = 0,
    [EnumName("判定对象数值")]
    CheckValue = 1,
    [EnumName("判定对象是否为当前玩家")]
    IsCurrentPlayer = 2
}

/// <summary>
/// 事件按钮的判定条件
/// </summary>
public enum EventButtonCondition
{
    [EnumName("无")]
    None = 0,
    [EnumName("判定玩家数值")]
    CheckPlayerValue = 1
}

/// <summary>
/// 设置或检查的数据类型
/// </summary>
public enum EventValueType
{
    [EnumName("生命值")]
    HealthPoint = 0,
    [EnumName("最大生命值")]
    MaxHealthPoint = 1,
    [EnumName("攻击力")]
    Attack = 2,
    [EnumName("防御力")]
    Defense = 3,
    [EnumName("速度（影响行动顺序）")]
    Speed = 4,
    [EnumName("行动点")]
    ActionPoint = 5,
    [EnumName("最大行动点")]
    MaxActionPoint = 6,
    [EnumName("移动速度（影响每个行动点最大移动距离）")]
    MoveSpeed = 7,
    [EnumName("技能冷却")]
    SkillColdDown = 8
}

/// <summary>
/// 数据筛选
/// </summary>
public enum EventValueFilter
{
    [EnumName("等于")]
    EqualsTo = 0,
    [EnumName("小于")]
    LessThan = 1,
    [EnumName("小于等于")]
    LessThanOrEqualsTo = 2,
    [EnumName("大于")]
    GreaterThan = 3,
    [EnumName("大于等于")]
    GreaterThanOrEqualsTo = 4,
    [EnumName("区间约束")]
    MinMax = 5
}

/// <summary>
/// 数据数量筛选
/// </summary>
public enum EventValueCountFilter
{
    [EnumName("单个")]
    Single = 0,
    [EnumName("多个")]
    Multi = 1,
    [EnumName("所有")]
    All = 2
}

/// <summary>
/// 数据类型改变方式
/// </summary>
public enum ValueChangeType
{
    [EnumName("直接设置")]
    Set = 0,
    [EnumName("增加")]
    Add = 1,
    [EnumName("增加百分比（单位：%）")]
    AddPercent = 2,
    [EnumName("减少")]
    Minus = 3,
    [EnumName("减少百分比（单位：%）")]
    MinusPercent = 4,
    [EnumName("倍数")]
    Multiply = 5
}

/// <summary>
/// Buff改变方式
/// </summary>
public enum BuffChangeType
{
    [EnumName("增加")]
    Add = 0,
    [EnumName("移除")]
    Remove = 1,
    [EnumName("改变数值")]
    ChangeValue = 2,
    [EnumName("刷新")]
    Refresh = 3
}

/// <summary>
/// Buff数据类型
/// </summary>
public enum BuffValueType
{
    [EnumName("持续时间")]
    Duration = 0
}

/// <summary>
/// 技能改变方式
/// </summary>
public enum SkillChangeType
{
    [EnumName("增加")]
    Add = 0,
    [EnumName("移除")]
    Remove = 1,
    [EnumName("改变数值")]
    ChangeValue = 2,
    [EnumName("刷新")]
    Refresh = 3
}

/// <summary>
/// 技能增删方式
/// </summary>
public enum SkillCountChangeType
{
    [EnumName("选择全部")]
    SelectAll = 0,
    [EnumName("多选一")]
    SelectOne = 1
}

/// <summary>
/// 技能数据类型
/// </summary>
public enum SkillValueType
{
    [EnumName("锁定状态")]
    LockState = 0,
    [EnumName("技能名称")]
    Name = 1,
    [EnumName("属性（冷却时间或者使用次数）")]
    Property = 2,
    [EnumName("最大属性（冷却时间或者使用次数）")]
    MaxProperty = 3,
    [EnumName("攻击范围")]
    AttackRange = 4,
    [EnumName("技能影响范围")]
    SkillEffectRange = 5,
    [EnumName("初始伤害")]
    OriginAttack = 6,
    [EnumName("选择类型")]
    ChooseType = 7,
    [EnumName("技能形状")]
    Shape = 8,
    [EnumName("最大可检索单位数")]
    MaxUnitsChoose = 9
}