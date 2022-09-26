using UnityEngine;

public partial class SkillManager
{
    private SkillDatas_SO allSkill;

    private static SkillManager instance;
    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SkillManager();
            }
            return instance;
        }
    }
    public SkillManager()
    {
        allSkill = Resources.Load<SkillDatas_SO>("AllSkill");
    }

    #region//�ж��Ƿ��ܹ�ʹ��
    /// <summary>
    /// ��0����ȴ��1��ʹ�ô���
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int ColdorCount(SkillData_SO data)
    {
        //1.�ж�����ȴ����ʹ�ô���
        return (int)data.currentSkillProperty;
    }
    public bool SkillCheckColdDown(SkillData_SO data)
    {
        //2.�����ж���������ж��Ƿ���Ա�ʹ��
        if (data.coldDownTime>0)
        {
            return false;
        }
        else if(data.coldDownTime==0)
        {
            return true;
        }
        else
        {
            Debug.Log("��ȴʱ��Ϊ��");
            return false;
        }
    }
    public bool SkillCheckCountDown(SkillData_SO data)
    {
        //2.�����ж���������ж��Ƿ���Ա�ʹ��
        if (data.countDown>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// �Զ��ж������Ƿ����ʹ��
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool SkillCheckAll(SkillData_SO data)
    {
        if( ColdorCount(data)==0)
        {
           return SkillCheckColdDown(data);
        }
        else if(ColdorCount(data)==1)
        {
            return SkillCheckCountDown(data);
        }
        else
        {
            Debug.Log("������ȴ���ʹ���");
            return false;
        }
    }
    #endregion

    public float AttackMath(int skillLevel,float skillboots,float origin )
    {
        return skillLevel* origin*(-1 *Mathf.Pow((skillboots - 10),2)  + 100) / 20;
    }
    /// <summary>
    /// ���������Ӽ���
    /// </summary>
    /// <param name="skillname"></param>
    #region//���������Ӽ���
    public void AddSkill(string skillname)
    {
        bool canAdd = true;
        GridManager.Instance.characterData.playerSave.TryGetValue(DataSave.Instance.currentObj.name, out Character player);//���������е�ָ����λ
        //����ϵͳ��Ҫ��һ����allskill�������ַ��似�ܸ�characterSo��api.
        foreach (var skilldata in player.skillSave)
        {
            if( skilldata.skillName==skillname)
            {
                canAdd = false ;
                break;
            }
        }
        if (canAdd)
        {
            allSkill.datasSave.TryGetValue(skillname, out SkillData_SO skill);//�������ֶ�������
            DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave.Add(skill);//�Ѽ��ܴ��뵱ǰ�ж���λ�ļ��ܱ���
            player.skillSave.Add(skill);
        }
    }
    #endregion
    //��ʼ����
    //ѡ��Ŀ��UI+���ﳯ��+ȡ������+Ⱥ��/����ѡ��Ŀ��ʱui�仯+��ѡ��Χ(���)��ʾ
    public void skillStart()
    {

    }
    //��ʾ������̣����ų���Ļ���ߣ�
    //���弼��������������ж��ɹ��󣬾� ͷ����ʾ��ǣ�
    //ͬʱ���������Ӧ����ⲻ��ʱ�����Ӧ������
    //Ⱥ�弼�����������������ʾ��Χ����Χ�������ʾ��ǣ�������һ��ʱ������Ӧ��
    //1.��ʾ��Χ��ʲô��
    //2.�����ʲô

}
