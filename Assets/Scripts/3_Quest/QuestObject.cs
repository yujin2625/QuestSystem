using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class QuestObject : MonoBehaviour
{
    public Quest Quest { get { return m_Quest; } set { m_Quest = value; } }
    private Quest m_Quest;
    private List<StepBase> m_steps { get { return GetComponentsInChildren<StepBase>().ToList(); } }

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=>m_Quest!=null);

    }
    private void Awake()
    {
        m_steps[m_Quest.StepIndex].gameObject.SetActive(true);

    }

    public void NextStep()
    {
        if (Quest.StepIndex > m_steps.Count)       // ���� ������ ���� ��
        {
            EndQuest();
            return;
        }
        // ���� ���� ������Ʈ �ѱ�
        m_steps[m_Quest.StepIndex].gameObject.SetActive(true);
        // DB�� ���� ����
    }

    public void EndQuest()
    {

    }





}

    //// ����Ʈ ID (DB ������)
    //[SerializeField] private string m_QuestID;
    //public string QuestID { get => m_QuestID; }

    //// ����Ʈ �̸� (Prefab �̸���)
    //[SerializeField] private string m_QuestName;
    //public string QuestName { get => m_QuestName; }

    //// �ݺ� ���� (��ȸ��, ����, ����, �Ŵ�, ���, �ʼ�)
    //[SerializeField] ERepeatType m_RepeatType;
    //public ERepeatType RepeatType { get => m_RepeatType; }

    //// ����Ʈ ����
    //[SerializeField] private int m_RewardPoint;
    //public int RewardPoint { get => m_RewardPoint; }

    //// ����Ʈ ���� ���� - ����
    //[SerializeField] private int m_MinLevel;
    //public int MinLevel { get => m_MinLevel; }

    //// ����Ʈ ���� ���� - ���
    //[SerializeField] private ESpace m_Space;
    //public ESpace Space { get => m_Space; }