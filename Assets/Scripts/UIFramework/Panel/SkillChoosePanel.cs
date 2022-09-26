using UnityEngine;
using TMPro;

public class SkillChoosePanel : BasePanel
{
	private CanvasGroup canvasGroup;
	public Transform skilllistParent;
    private void Start()
    {
        SaveSkillIntoUI();
    }

    private void SaveSkillIntoUI()
    {
		//SkillManager.Instance.AddSkill("Attack");
		//�Ѽ��ܴ��뵱ǰ�ж���λ�ļ��ܱ���
		GameObject tempPrefab = Resources.Load<GameObject>("skillName");
		foreach (var skill in DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave)
        {
            //��prefab
            GameObject tempSkillItem= GameObject.Instantiate<GameObject>(tempPrefab, skilllistParent);

			SkillReal tempReal = tempSkillItem.GetComponent<SkillReal>();
			//�ı��ֲ�����
			tempReal.skill = skill;
			tempReal.tmp.text = skill.skillName;
		}
    }

    public override void OnEnter()
	{
		if (canvasGroup == null)
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;

	}

	public override void OnExit()
	{
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;

	}

	public override void OnPause()
	{
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		this.gameObject.SetActive(false);

	}

	public override void OnResume()
	{
		this.gameObject.SetActive(true);

		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}
}
