using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ObjectiveType
{
    TakePhoto,
    ReachPosition,
    ScanObject
}

public class MainObjectiveManager : Singleton<MainObjectiveManager>
{
    [SerializeField] string startMainObjectiveCode;
    [Indent,SerializeField,ReadOnly] string currentMainObjectiveCode;

    Dictionary<string, MainObjectiveData> mainObjectiveDataDictionary = new Dictionary<string, MainObjectiveData>();
    public Dictionary<string, MainObjectiveData> MainObjectiveDataDictionary => mainObjectiveDataDictionary;

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
        MessageManager.Instance.AddLogData(mainObjectiveDataDictionary[currentMainObjectiveCode]);
    }

    string SetNextObjective(string NextObjectiveCode)
    {
        if(NextObjectiveCode == "end")
        {
            Debug.Log("Happy ending");
            StartCoroutine(GameManager.Instance.GoToCutscene("As_ending"));
        }

        currentMainObjectiveCode = mainObjectiveDataDictionary[currentMainObjectiveCode].NextObjectiveCode;
        return currentMainObjectiveCode;
    }
}
