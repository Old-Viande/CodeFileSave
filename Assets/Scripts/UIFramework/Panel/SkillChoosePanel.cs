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
		//把技能存入当前行动单位的技能表中
		GameObject tempPrefab = Resources.Load<GameObject>("skillName");
		foreach (var skill in DataSave.Instance.currentObj.GetComponent<CharacterData>().unit.skillSave)
        {
            //生prefab
            GameObject tempSkillItem= GameObject.Instantiate<GameObject>(tempPrefab, skilllistParent);

			SkillReal tempReal = tempSkillItem.GetComponent<SkillReal>();
			//改表现层数据
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
