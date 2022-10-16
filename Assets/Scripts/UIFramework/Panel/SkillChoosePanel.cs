using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillChoosePanel : BasePanel
{
	public Transform skilllistParent;
	public Button exitBtn;
	private void Start()
    {
        SaveSkillIntoUI();


		//exitBtn.onClick.AddListener(delegate ()
		//{
		//	UIManager.Instance.PopPanel();
		//});
    }

    private void SaveSkillIntoUI()
    {
		//SkillManager.Instance.AddSkill("Attack");
		//�Ѽ��ܴ��뵱ǰ�ж���λ�ļ��ܱ���
		//�����
		foreach (Transform item in skilllistParent)
		{
			Destroy(item.gameObject);
		}
		GameObject tempPrefab = Resources.Load<GameObject>("skillName");
		foreach (var skill in DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave)
        {
            //��prefab
            GameObject tempSkillItem= GameObject.Instantiate<GameObject>(tempPrefab, skilllistParent);

			SkillReal tempReal = tempSkillItem.GetComponent<SkillReal>();
			//�ı��ֲ�����
			tempReal.skill = skill;
			tempReal.tmp.text = skill.skillName;
			tempReal. btn.onClick.AddListener(delegate () {
				SkillManager.Instance.SkillStart(tempReal.skill);				
			});
		}
    }

	public override void OnExit()
	{
		base.OnExit();

		//������ָʾ
		UpdataManager.Instance.skillMarkOpen = false;
		UpdataManager.Instance.skillLineOpen = false;
	}

	public override void OnPause()
	{
		base.OnPause();

		//������ָʾ
		UpdataManager.Instance.skillMarkOpen = false;
		UpdataManager.Instance.skillLineOpen = false;
	}
}
