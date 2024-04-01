using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestDataSet
{
    public List<QuestData> QuestDatas;
}

[Serializable]
public class QuestData
{
    public int qm_id;
    public string name;
    public ERepeatType repeat_type;
    public int reward_point;
    public int min_level;
    public ESpace space;
    
    public QuestData(int qm_id, string name, ERepeatType repeat_type, int reward_point, int min_level, ESpace space)
    {
        this.qm_id = qm_id;
        this.name = name;   
        this.repeat_type = repeat_type;
        this.reward_point = reward_point;
        this.min_level = min_level;
        this.space = space;
    }
}
