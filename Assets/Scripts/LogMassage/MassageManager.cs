using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LogMassage;
using Sirenix.OdinInspector;
using UnityEngine;

public class MassageManager : Singleton<MassageManager>
{
    [SerializeField] string fileName;
    [SerializeField] MassageText massageTextTemplate;
    [SerializeField] Transform massageTextContanier;
    
    [SerializeField] Dictionary<string,MassageData> massageDataDic = new Dictionary<string, MassageData>();
    [Indent,SerializeField,ReadOnly] List<LogData> logDataDic = new List<LogData>();

    [SerializeField] UIMessageNotification uIMessageNotification;

    [Header("Massage Color")]
    [SerializeField] Color M_SenderColor = Color.white;
    [SerializeField] Color M_TimeColor = Color.white;
    [SerializeField] Color M_MassageColor = Color.white;

    [Header("Obejctive Color")]
    [SerializeField] Color O_SenderColor = Color.white;
    [SerializeField] Color O_TimeColor = Color.white;
    [SerializeField] Color O_MassageColor = Color.white;

    protected override void InitAfterAwake()
    {

    }

    void Start() 
    {
        Init();
        LoadMassageDataFromCSV();
    }

    public void Init()
    {   
        logDataDic.Clear();
        massageTextTemplate.gameObject.SetActive(false);
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
        print(objectiveData.Sender);
        print(objectiveData.LogMessage);
        LogData newLogData = new LogData(objectiveData.Sender,TimeManager.Instance.GetCurrentTime(),objectiveData.LogMessage,LogType.Objective);
        logDataDic.Add(newLogData);

        DisplayLogData(newLogData);
    }

    public void AddLogData(string massageCode)
    {
        LogData newLogData = new LogData(massageDataDic[massageCode].sender,TimeManager.Instance.GetCurrentTime(),massageDataDic[massageCode].massage,LogType.Massage);
        logDataDic.Add(newLogData);

        DisplayLogData(newLogData);
    }

    [Button]
    void DisplayLogData(LogData logData)
    {
        MassageText massageText = Instantiate(massageTextTemplate,massageTextContanier);
        if(logData.logType == LogType.Massage)
            massageText.SetMassageText(logData.GetLogString(M_SenderColor,M_TimeColor,M_MassageColor));
        else
            massageText.SetMassageText(logData.GetLogString(O_SenderColor,O_TimeColor,O_MassageColor));

        massageText.gameObject.SetActive(true);
        uIMessageNotification.PlayEnter();
    }

    public void HideNotificationText()
    {
        uIMessageNotification.PlayExit();
    }

    [Button]
    void ForceDisplayLogConsole()
    {
        //TODO: Force player to use log console
    }
    
}
