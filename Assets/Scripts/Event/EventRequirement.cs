/// <summary>
/// �¼���������
/// </summary>
public enum EventTrigger
{
    [EnumName("����ɫ����")]
    OnCreated = 0,
    OnDied = 1,
}

/// <summary>
/// �����¼���Ч����������
/// </summary>
public enum RoomEventEffectTrigger
{
    [EnumName("��ҽ��뷿��ʱ����")]
    OnEnter = 0,
    [EnumName("����뿪����ʱ����")]
    OnLeave = 1,
    [EnumName("���ͣ�����䣨���غ�ʱ������")]
    OnStay = 2
}

/// <summary>
/// �¼�����ǰ������
/// </summary>
public enum EventTriggerPre
{
    [EnumName("��")]
    None = 0,
    [EnumName("����ʱ����")]
    TimeDuration = 1,
    [EnumName("���������0~100��")]
    Random = 2,
    [EnumName("��������")]
    EnemyCount = 3,
    [EnumName("��������")]
    RoomCount = 4,
    [EnumName("��ǰ�غ���")]
    RoundSpent = 5
}

/// <summary>
/// �¼�ִ��Ч��,���Ŀ��
/// </summary>
public enum EventEffectWithTarget
{
    [EnumName("�ı���ֵ")]
    ChangeValue = 0,
    [EnumName("�ı�Buff")]
    ChangeBuff = 1,
    [EnumName("�ı似��")]
    ChangeSkill = 2,
    [EnumName("�ƶ�����")]
    MoveObject = 3
}

/// <summary>
/// �¼�ִ��Ч��,��Ŀ��
/// </summary>
public enum EventEffectWithoutTarget
{
    [EnumName("������ҽ�ɫ")]
    AddPlayer = 0,
    [EnumName("�Ƴ���ҽ�ɫ")]
    RemovePlayer = 1,
    [EnumName("���ɵ���")]
    CreateEnemy = 2,
}

/// <summary>
/// �¼�ִ�ж���
/// </summary>
public enum EventTarget
{
    [EnumName("��")]
    None = 0,
    [EnumName("��ǰ���")]
    CurrentPlayer = 1,
    [EnumName("�������")]
    AllPlayer = 2,
    [EnumName("���з��������")]
    AllPlayerInRoom = 3,
    [EnumName("���з��������")]
    RandomPlayer = 4,
    [EnumName("���е���")]
    AllEnemy = 5,
    [EnumName("���з����ڵ���")]
    AllEnemyInRoom = 6,
    [EnumName("���з��������")]
    RandomEnemy = 7
}

/// <summary>
/// �¼�ִ�ж���ɸѡ����
/// </summary>
public enum EventTargetFilter
{
    [EnumName("��")]
    None = 0,
    [EnumName("�ж�������ֵ")]
    CheckValue = 1,
    [EnumName("�ж������Ƿ�Ϊ��ǰ���")]
    IsCurrentPlayer = 2
}

/// <summary>
/// �¼���ť���ж�����
/// </summary>
public enum EventButtonCondition
{
    [EnumName("��")]
    None = 0,
    [EnumName("�ж������ֵ")]
    CheckPlayerValue = 1
}

/// <summary>
/// ���û������������
/// </summary>
public enum EventValueType
{
    [EnumName("����ֵ")]
    HealthPoint = 0,
    [EnumName("�������ֵ")]
    MaxHealthPoint = 1,
    [EnumName("������")]
    Attack = 2,
    [EnumName("������")]
    Defense = 3,
    [EnumName("�ٶȣ�Ӱ���ж�˳��")]
    Speed = 4,
    [EnumName("�ж���")]
    ActionPoint = 5,
    [EnumName("����ж���")]
    MaxActionPoint = 6,
    [EnumName("�ƶ��ٶȣ�Ӱ��ÿ���ж�������ƶ����룩")]
    MoveSpeed = 7,
    [EnumName("������ȴ")]
    SkillColdDown = 8
}

/// <summary>
/// ����ɸѡ
/// </summary>
public enum EventValueFilter
{
    [EnumName("����")]
    EqualsTo = 0,
    [EnumName("С��")]
    LessThan = 1,
    [EnumName("С�ڵ���")]
    LessThanOrEqualsTo = 2,
    [EnumName("����")]
    GreaterThan = 3,
    [EnumName("���ڵ���")]
    GreaterThanOrEqualsTo = 4,
    [EnumName("����Լ��")]
    MinMax = 5
}

/// <summary>
/// ��������ɸѡ
/// </summary>
public enum EventValueCountFilter
{
    [EnumName("����")]
    Single = 0,
    [EnumName("���")]
    Multi = 1,
    [EnumName("����")]
    All = 2
}

/// <summary>
/// �������͸ı䷽ʽ
/// </summary>
public enum ValueChangeType
{
    [EnumName("ֱ������")]
    Set = 0,
    [EnumName("����")]
    Add = 1,
    [EnumName("���Ӱٷֱȣ���λ��%��")]
    AddPercent = 2,
    [EnumName("����")]
    Minus = 3,
    [EnumName("���ٰٷֱȣ���λ��%��")]
    MinusPercent = 4,
    [EnumName("����")]
    Multiply = 5
}

/// <summary>
/// Buff�ı䷽ʽ
/// </summary>
public enum BuffChangeType
{
    [EnumName("����")]
    Add = 0,
    [EnumName("�Ƴ�")]
    Remove = 1,
    [EnumName("�ı���ֵ")]
    ChangeValue = 2,
    [EnumName("ˢ��")]
    Refresh = 3
}

/// <summary>
/// Buff��������
/// </summary>
public enum BuffValueType
{
    [EnumName("����ʱ��")]
    Duration = 0
}

/// <summary>
/// ���ܸı䷽ʽ
/// </summary>
public enum SkillChangeType
{
    [EnumName("����")]
    Add = 0,
    [EnumName("�Ƴ�")]
    Remove = 1,
    [EnumName("�ı���ֵ")]
    ChangeValue = 2,
    [EnumName("ˢ��")]
    Refresh = 3
}

/// <summary>
/// ������ɾ��ʽ
/// </summary>
public enum SkillCountChangeType
{
    [EnumName("ѡ��ȫ��")]
    SelectAll = 0,
    [EnumName("��ѡһ")]
    SelectOne = 1
}

/// <summary>
/// ������������
/// </summary>
public enum SkillValueType
{
    [EnumName("����״̬")]
    LockState = 0,
    [EnumName("��������")]
    Name = 1,
    [EnumName("���ԣ���ȴʱ�����ʹ�ô�����")]
    Property = 2,
    [EnumName("������ԣ���ȴʱ�����ʹ�ô�����")]
    MaxProperty = 3,
    [EnumName("������Χ")]
    AttackRange = 4,
    [EnumName("����Ӱ�췶Χ")]
    SkillEffectRange = 5,
    [EnumName("��ʼ�˺�")]
    OriginAttack = 6,
    [EnumName("ѡ������")]
    ChooseType = 7,
    [EnumName("������״")]
    Shape = 8,
    [EnumName("���ɼ�����λ��")]
    MaxUnitsChoose = 9
}