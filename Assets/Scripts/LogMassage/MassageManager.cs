using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class MassageManager : Singleton<MassageManager>
{
    [SerializeField] string fileName;
    
    [SerializeField] Dictionary<string,MassageData> massageDataDic = new Dictionary<string, MassageData>();
    List<LogData> logDataDic = new List<LogData>();

    protected override void InitAfterAwake()
    {

    }

    void Start() 
    {
        LoadMassageDataFromCSV();
    }

    public void Init()
    {   
        logDataDic.Clear();
    }

    void LoadMassageDataFromCSV()
    {
        massageDataDic.Clear();

        MassageData[] massageDatas = CSVHelper.LoadCSVAsObject<MassageData>(fileName);

        foreach(var n in massageDatas)
        {
            massageDataDic.Add(n.massageCode,n);
        }
    }

    public void AddLogData(MainObjectiveData objectiveData)
    {
        //TODO: Add objective log to log data
        LogData newLogData = new LogData(objectiveData.Sender,TimeManager.Instance.GetCurrentTime(),objectiveData.LogMessage);
        logDataDic.Add(newLogData);

        DisplayLogData();
    }

    public void AddLogData(string massageCode)
    {
        //TODO: Add massage log to log data
        LogData newLogData = new LogData(massageDataDic[massageCode].sender,TimeManager.Instance.GetCurrentTime(),massageDataDic[massageCode].massage);
        logDataDic.Add(newLogData);

        DisplayLogData();
    }

    [Button]
    void TestAddMassageData()
    {
        AddLogData(MainObjectiveManager.Instance.MainObjectiveDataDictionary["tp_terminal"]);
    }

    [Button]
    void TestAddLogData()
    {
        AddLogData("d1_door");
    }

    [Button]
    void DisplayLogData()
    {
        //TODO: Display Log Data in Pip Boy UI
    }

    [Button]
    void ForceDisplayLogConsole()
    {
        //TODO: Force player to use log console
    }
    
}
