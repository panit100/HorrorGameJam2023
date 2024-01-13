using System.Collections.Generic;
using Hellmade.Sound;
using HorrorJam.Audio;
using LogMassage;
using Sirenix.OdinInspector;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    [SerializeField] string fileName;
    [SerializeField] MassageText massageTextTemplate;
    [SerializeField] Transform massageTextContanier;
    
    [SerializeField] Dictionary<string,MassageData> massageDataDic = new Dictionary<string, MassageData>();
    [Indent,SerializeField,ReadOnly] List<LogData> logDataDic = new List<LogData>(); //Use it in save game

    List<MassageText> massageTextList = new List<MassageText>();

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
        LogData newLogData = new LogData(objectiveData.ObjectiveCode,objectiveData.Sender,TimeManager.Instance.GetCurrentTime(),objectiveData.LogMessage,LogType.Objective,objectiveData.SenderColor,objectiveData.TimeColor,objectiveData.MassageColor);
        logDataDic.Add(newLogData);

        ShowNotificationText();
    }

    //FOR normal Log
    public void AddLogData(string massageCode)
    {
        LogData newLogData = new LogData(massageCode,massageDataDic[massageCode].sender,TimeManager.Instance.GetCurrentTime(),massageDataDic[massageCode].massage,LogType.Massage,massageDataDic[massageCode].senderColor,massageDataDic[massageCode].timeColor,massageDataDic[massageCode].massageColor);
        logDataDic.Add(newLogData);

        ShowNotificationText();
    }

    [Button]
    public void DisplayLogData()
    {
        foreach(var n in massageTextList)
        {
            Destroy(n.gameObject);
        }
        massageTextList.Clear();

        foreach(var n in logDataDic)
        {
            MassageText massageText = Instantiate(massageTextTemplate,massageTextContanier);
            
            massageText.logData = n;

            massageText.SetMassageText(n.GetLogString());

            massageText.gameObject.SetActive(true);

            massageTextList.Add(massageText);
        }
    }

    void ShowNotificationText()
    {
        AudioManager.Instance.PlayAudioOneShot("new_massage");

        if(!PipboyManager.Instance.IsUsingPipboy)
        {
            uIMessageNotification.PlayEnter();
        }
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

    [Button]
    public void ClearLogData() // use to clear message in device when change act
    {
        logDataDic.Clear();

        foreach(var n in massageTextList)
        {
            Destroy(n.gameObject);
        }

        massageTextList.Clear();
    }

    [Button]
    public void RemoveMessage(string code) //remove message by code
    {
        MassageText massageText = massageTextList.Find(a => a.logData.code == code);
        massageTextList.Remove(massageText);
        Destroy(massageText.gameObject);
    }
}
