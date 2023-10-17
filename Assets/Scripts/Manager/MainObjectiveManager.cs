using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class MainObjectiveData{
    public string ObjectiveName;
    public string ObjectiveCode;
    public ObjectiveType objectiveType;
    public string NextObjectiveCode;
    public string LogMessage;
    
    public enum ObjectiveType
    {
        TakePhoto,
        ReachPosition,
        ScanObject
    }

    public MainObjectiveData(string _ObjectiveName, string _ObjectiveCode, string _ObjectiveType, string _NextObjectiveCode, string _LogMessage)
    {
        ObjectiveName = _ObjectiveName;
        ObjectiveCode = _ObjectiveCode;
        SetObjectiveType(_ObjectiveType);
        NextObjectiveCode = _NextObjectiveCode;
        LogMessage = _LogMessage;
    }

    void SetObjectiveType(string _objectiveType)
    {
        switch (_objectiveType)
        {
            case "ObjectiveType.TakePhoto":
                objectiveType = ObjectiveType.TakePhoto;
                break;
            case "ObjectiveType.ReachPosition":
                objectiveType = ObjectiveType.ReachPosition;
                break;
            case "ObjectiveType.ScanObject":
                objectiveType = ObjectiveType.ScanObject;
                break;
            default:
                Debug.Log("Set Objective Error.");
                break;
        }
    }
}

public class MainObjectiveManager : Singleton<MainObjectiveManager>
{
    public Objective currentObjective;
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

    public bool GetCheckObjective(Objective checkObjective)
    {
        if (checkObjective == currentObjective)
        {
            UpdateProgress();
            return true;
        }

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
}
