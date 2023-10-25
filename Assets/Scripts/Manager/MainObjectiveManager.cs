using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;

public enum ObjectiveType
{
    TakePhoto,
    ReachPosition,
    ScanObject
}

public class MainObjectiveData{
    public string ObjectiveName;
    public string ObjectiveCode;
    public string ObjectiveType;
    public string NextObjectiveCode;
    public string Sender;
    public string LogMessage;
    public string SenderColor;
    public string TimeColor;
    public string MassageColor;
    
    public ObjectiveType GetObjectiveType()
    {
        switch (this.ObjectiveType)
        {
            case "ObjectiveType.TakePhoto":
                return global::ObjectiveType.TakePhoto;
            case "ObjectiveType.ReachPosition":
                return global::ObjectiveType.ReachPosition;
            case "ObjectiveType.ScanObject":
                return global::ObjectiveType.ScanObject;
            default:
                return global::ObjectiveType.TakePhoto;
        }
    }
}

public class MainObjectiveManager : Singleton<MainObjectiveManager>
{
    [SerializeField] string startMainObjectiveCode;

    Dictionary<string, MainObjectiveData> mainObjectiveDataDictionary = new Dictionary<string, MainObjectiveData>();
    public Dictionary<string, MainObjectiveData> MainObjectiveDataDictionary => mainObjectiveDataDictionary;
    [Indent,SerializeField,ReadOnly] string currentMainObjectiveCode;
    [SerializeField] string mainObjectiveFile;
   
    
    protected override void InitAfterAwake()
    {

    }

    private void Start()
    {
        LoadDialogueFromCSV();
    }

    public void LoadDialogueFromCSV()
    {
        MainObjectiveData[] mainObjectiveDatas = CSVHelper.LoadCSVAsObject<MainObjectiveData>(mainObjectiveFile);

        foreach (var data in mainObjectiveDatas)
        {
            mainObjectiveDataDictionary.Add(data.ObjectiveCode, data);
        }
    }

    public void SetupObjective()
    {
        currentMainObjectiveCode = startMainObjectiveCode;
    }

    public bool isEqualCurrentObjective(string objectiveCode)
    {
        if (currentMainObjectiveCode == objectiveCode)
            return true;
        else
            return false;
    }
    
    public void UpdateProgress(string objectiveCode)
    {
        if (!isEqualCurrentObjective(objectiveCode))
            return;

        SendLogToPipBoy();
        SetNextObjective(mainObjectiveDataDictionary[currentMainObjectiveCode].NextObjectiveCode);

    }

    public void SendLogToPipBoy()
    {
        MassageManager.Instance.AddLogData(mainObjectiveDataDictionary[currentMainObjectiveCode]);
    }

    string SetNextObjective(string NextObjectiveCode)
    {
        if(NextObjectiveCode == "")
        {
            Debug.Log("Happy ending");
            StartCoroutine(GameManager.Instance.GoToCutscene("As_ending"));
        }

        currentMainObjectiveCode = mainObjectiveDataDictionary[currentMainObjectiveCode].NextObjectiveCode;
        return currentMainObjectiveCode;
    }
}
