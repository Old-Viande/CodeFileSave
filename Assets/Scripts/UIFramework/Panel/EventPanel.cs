using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPanel : BasePanel
{
    public TextMeshProUGUI m_EventNameText;
    public GameObject m_ButtonTemplate;
    public Transform m_ButtonParent;
    public GameObject m_DescriptionTemplate;
    public Transform m_DescriptionParent;

    private readonly List<string> m_DescriptionList = new();
    private readonly List<RoomEvent.ButtonData> m_ButtonDataList = new();

    private int m_Counter = 0;
    private float m_Timer = 0.0f;
    public float m_TimeStep = 1.0f;

    private void Update()
    {
        UpdatePanel();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        UpdataManager.Instance.eventHappen = true;
    }

    public override void OnExit()
    {
        base.OnExit();

        UpdataManager.Instance.eventHappen = false;

        for (int i = 0; i < m_DescriptionParent.childCount; i++)
        {
            m_DescriptionParent.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < m_ButtonParent.childCount; i++)
        {
            m_ButtonParent.GetChild(i).gameObject.SetActive(false);
        }

        m_DescriptionList.Clear();
        m_ButtonDataList.Clear();
        m_Counter = 0;
        m_Timer = 0.0f;
    }

    private void UpdatePanel()
    {
        if (m_DescriptionList.Count == 0)
            return;

        m_Timer += Time.deltaTime;
        int index = Mathf.FloorToInt(m_Timer) + 1;
        if (index > m_Counter)
        {
            if (m_Counter < m_DescriptionList.Count)
            {
                GameObject text = null;
                if (m_Counter < m_DescriptionParent.childCount)
                    text = m_DescriptionParent.GetChild(m_Counter).gameObject;
                if (text == null)
                    text = Instantiate(m_DescriptionTemplate, m_DescriptionParent);

                text.GetComponent<TextMeshProUGUI>().text = m_DescriptionList[m_Counter];
                text.SetActive(true);
                m_Counter++;
            }
            else if (m_Counter == m_DescriptionList.Count)
            {
                for (int i = 0; i < m_ButtonDataList.Count; i++)
                {
                    var buttonData = m_ButtonDataList[i];
                    if (RoomEventManager.Instance.CheckButtonCondition(buttonData))
                    {
                        GameObject newButton = null;
                        if (i < m_ButtonParent.childCount)
                            newButton = m_ButtonParent.GetChild(i).gameObject;
                        if (newButton == null)
                            newButton = Instantiate(m_ButtonTemplate, m_ButtonParent);

                        newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonData.Text;
                        newButton.GetComponent<Button>().onClick = buttonData.ButtonEvent;
                        newButton.SetActive(true);
                    }
                }
                m_Counter++;
            }
        }
    }

    public void UpdateRoomEvent(RoomEvent roomEvent)
    {
        m_EventNameText.text = roomEvent.Name;
        m_DescriptionList.Clear();
        for (int i = 0; i < roomEvent.Description.Count; i++)
        {
            m_DescriptionList.Add(roomEvent.Description[i]);
        }
        m_ButtonDataList.Clear();
        for (int i = 0; i < roomEvent.ButtonList.Count; i++)
        {
            m_ButtonDataList.Add(roomEvent.ButtonList[i]);
        }
    }
}
