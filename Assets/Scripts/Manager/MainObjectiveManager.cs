using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public string LogMessage;
    
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
    public Objective currentObjective; //TODO: Change to string or MainObjectiveData
    [SerializeField] int objectiveIndex = 0;
    public List<Objective> objectiveItems;

    Dictionary<string, MainObjectiveData> mainObjectiveDataDictionary = new Dictionary<string, MainObjectiveData>();
    MainObjectiveData currentMainObjectiveData;
    [SerializeField] string mainObjectiveFile;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI objectiveText;
    protected override void InitAfterAwake()
    {

    }

    private void Start()
    {
        LoadDialogueFromCSV(mainObjectiveFile);
        //SetupObjective();
    }
    void LoadDialogueFromCSV(string csvFile)
    {
        MainObjectiveData[] mainObjectiveDatas = CSVHelper.LoadCSVAsObject<MainObjectiveData>(csvFile);

        foreach (var data in mainObjectiveDatas)
        {
            mainObjectiveDataDictionary.Add(data.ObjectiveCode, data);
            print(mainObjectiveDataDictionary[data.ObjectiveCode].ObjectiveName);
        }
    }

    void SetupObjective()
    {
        if (objectiveItems.Count == 0)
        {
            return;
        }
        currentObjective = objectiveItems[objectiveIndex];
        UpdateObjectiveText();
    }

    public bool GetCheckObjective(string objectiveCode)
    {
        //TODO: Use objective code to check current objective
        // if (checkObjective == currentObjective)
        // {
        //     UpdateProgress();
        //     return true;
        // }

        return false;
    }
    void UpdateProgress()
    {
        objectiveIndex += 1;
        if (objectiveIndex < objectiveItems.Count)
        {
            currentObjective = objectiveItems[objectiveIndex];
        }
        else
        {
            currentObjective = null;
            print("No Objective");
        }
        UpdateObjectiveText();
    }
    void UpdateObjectiveText()
    {
        if (currentObjective == null)
        {
            objectiveText.text = "-";
            return;
        }
        if (currentObjective.GetComponent<InteractObject>() != null)
        {
            objectiveText.text = "Find " + currentObjective.name;
        }
        else if (currentObjective.GetComponent<Scanable>() != null)
        {
            objectiveText.text = "Find and Scan " + currentObjective.name;
        }
    }

    //TODO: Create function to Get log massage

    //TODO: Craete function to Set Next Objective
}
