using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }


    /// <summary>
    /// QM의 역할
    /// - DB에서 정보 받아오기/전달하기
    /// - 저장된 퀘스트 정보 분류, 저장
    /// - 퀘스트 실행 조건 확인
    /// - 퀘스트 오브젝트 생성
    /// </summary>


    // Need to get input from inspector
    [Header("Required Inputs")]
    [SerializeField] private string GetUserQuestURL;
    [SerializeField] private string GetQuestURL;
    [SerializeField] private Transform QuestParent;
    [SerializeField] private List<QuestObject> QuestPrefabs = new List<QuestObject>();

    [Space(10f)]

    // Serialized for Debugging
    [Header("View Result")]
    [SerializeField] private bool IsQuestSet = true;
    public UserQuestDataSet userQuestDataSet;
    public QuestDataSet questDataSet;
    [Space(10f)]

    [Header("QuestList")]
    public List<Quest> QuestList = new List<Quest>();


    private List<QuestObject> QuestObjects { get { return QuestParent.GetComponentsInChildren<QuestObject>().ToList(); } }

    private IEnumerator Start()
    {
        yield return SetQuestData();
        CreateQuestObjects();


    }

    public IEnumerator SetQuestData()
    {
        //UserQuestDataSet userQuestDataSet;
        //QuestDataSet questDataSet;

        // =========User Quest Data 가져오기==================
        Debug.Log("Getting User Quest Data...");
        UnityWebRequest userQuestWWW = UnityWebRequest.Get(GetUserQuestURL);
        yield return userQuestWWW.SendWebRequest();
        if (userQuestWWW.error != null)
        {
            Debug.LogError(userQuestWWW.error);
            Debug.LogError("Getting User Quest Data Failed");
            yield break;
        }
        userQuestDataSet = JsonUtility.FromJson<UserQuestDataSet>(userQuestWWW.downloadHandler.text);
        Debug.Log("User Quest Data Get Complete!!");

        // =========Quest Data 가져오기====================
        Debug.Log("Getting Quest Data...");
        UnityWebRequest questWWW = UnityWebRequest.Get(GetQuestURL);
        yield return questWWW.SendWebRequest();
        if (questWWW.error != null)
        {
            Debug.LogError(questWWW.error);
            Debug.LogError("Getting Quest Data Failed");
            yield break;
        }
        questDataSet = JsonUtility.FromJson<QuestDataSet>(questWWW.downloadHandler.text);
        Debug.Log("Quest Data Get Complete!!");

        // ===========Quest List 설정하기==================
        if (!IsQuestSet)
        {
            Debug.LogError("!!!!!!!! Quest Is Already Getting Set !!!!!!!!\nThis Method Will Be Stopped");
            yield break;
        }
        IsQuestSet = false;
        Debug.Log("Setting Quest List...");
        QuestList.Clear();
        foreach (QuestData item in questDataSet.QuestDatas)
        {
            UserQuestData userData = GetMatchingData(userQuestDataSet, item);
            if (userData == null)
                QuestList.Add(new Quest(item.qm_id.ToString(), item.name, item.repeat_type, item.reward_point, item.min_level, item.space));
            else
                QuestList.Add(new Quest(item.qm_id.ToString(), item.name, item.repeat_type, item.reward_point, item.min_level, item.space, userData.cond_num, userData.completed));
        }
        Debug.Log("Quest List Set Complete !!");
        IsQuestSet = true;

    }

    public void CreateQuestObjects()
    {
        if (!IsQuestSet)
        {
            Debug.LogError("QuestNotSetYet");
            return;
        }
        foreach (Quest quest in GetQuestByCompletion(false))     // 모든 진행 가능 퀘스트
        {
            if (!CheckQuestObjectExsistance(quest.QuestName))       // 퀘스트 오브젝트 존재하지 않으면
            {
                GameObject obj = Instantiate(Resources.Load(quest.QuestName) as GameObject, QuestParent);
                obj.GetComponent<QuestObject>().Quest = quest;              // 퀘스트 오브젝트에 퀘스트 정보 전달
            }
        }

    }

    private bool CheckQuestObjectExsistance(string name)
    {
        foreach(QuestObject obj in QuestObjects)
        {
            if (obj.name.Split('(')[0] == name)
                return true;
        }
        return false;

    }


    //public List<Quest> GetQuest()
    //{
    //    if (IsQuestSet)
    //        return QuestList;
    //    else
    //    {
    //        Debug.LogError("QuestNotSetYet");
    //        return null;
    //    }
    //}

    //public Quest GetQuestByName(string name)
    //{
    //    if (!IsQuestSet)
    //    {
    //        Debug.LogError("QuestNotSetYet");
    //        return null;
    //    }
    //    foreach (Quest quest in QuestList)
    //    {
    //        if (quest.QuestName == name)
    //            return quest;
    //    }
    //    return null;
    //}

    public List<Quest> GetQuestByRepeatType(ERepeatType repeatType)
    {
        if (!IsQuestSet)
        {
            Debug.LogError("QuestNotSetYet");
            return null;
        }
        List<Quest> result = new List<Quest>();
        foreach (Quest quest in QuestList)
        {
            if (quest.RepeatType == repeatType)
                result.Add(quest);
        }
        return result;
    }

    public List<Quest> GetQuestByCompletion(bool completion)
    {
        if (!IsQuestSet)
        {
            Debug.LogError("QuestNotSetYet");
            return null;
        }
        List<Quest> result = new List<Quest>();
        foreach (var quest in QuestList)
        {
            if (quest.IsCompleted == completion)
                result.Add(quest);
        }
        return result;
    }

    private QuestData GetMatchingData(QuestDataSet questDataSet, UserQuestData userQuestData)
    {
        foreach (QuestData quest in questDataSet.QuestDatas)
        {
            if (quest.name == userQuestData.quest_id)
                return quest;
        }
        return null;
    }
    private UserQuestData GetMatchingData(UserQuestDataSet userQuestDataSet, QuestData questData)
    {
        foreach (UserQuestData userData in userQuestDataSet.QuestDatas)
        {
            if (userData.quest_id == questData.name)
                return userData;
        }
        return null;
    }

}
