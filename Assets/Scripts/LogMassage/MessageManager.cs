using System.Collections.Generic;
using LogMassage;
using Sirenix.OdinInspector;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    [SerializeField] string fileName;
    [SerializeField] MassageText massageTextTemplate;
    [SerializeField] Transform massageTextContanier;
    
    [SerializeField] Dictionary<string,MassageData> massageDataDic = new Dictionary<string, MassageData>();
    [Indent,SerializeField,ReadOnly] List<LogData> logDataDic = new List<LogData>();

    [SerializeField] UIMessageNotification uIMessageNotification;

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

    //FOR Objective
    public void AddLogData(MainObjectiveData objectiveData)
    {
        LogData newLogData = new LogData(objectiveData.Sender,TimeManager.Instance.GetCurrentTime(),objectiveData.LogMessage,LogType.Objective,objectiveData.SenderColor,objectiveData.TimeColor,objectiveData.MassageColor);
        logDataDic.Add(newLogData);

        DisplayLogData(newLogData);
    }

    //FOR normal Log
    public void AddLogData(string massageCode)
    {
        LogData newLogData = new LogData(massageDataDic[massageCode].sender,TimeManager.Instance.GetCurrentTime(),massageDataDic[massageCode].massage,LogType.Massage,massageDataDic[massageCode].senderColor,massageDataDic[massageCode].timeColor,massageDataDic[massageCode].massageColor);
        logDataDic.Add(newLogData);

        DisplayLogData(newLogData);
    }

    [Button]
    void DisplayLogData(LogData logData)
    {
        MassageText massageText = Instantiate(massageTextTemplate,massageTextContanier);
        
        massageText.SetMassageText(logData.GetLogString());

        massageText.gameObject.SetActive(true);

        if(PipboyManager.Instance.IsUsingPipboy)
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
