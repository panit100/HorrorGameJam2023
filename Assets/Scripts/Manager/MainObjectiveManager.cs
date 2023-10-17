using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainObjectiveManager : Singleton<MainObjectiveManager>
{
    protected override void InitAfterAwake(){}
    public Objective currentObjective;
    [SerializeField] int objectiveIndex = 0;
    public List<Objective> objectiveItems;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI objectiveText;
    private void Start()
    {
        SetupObjective();
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

    public void GetCheckObjective(Objective checkObjective)
    {
        if (checkObjective == currentObjective)
        {
            checkObjective.gameObject.SetActive(false);
            UpdateProgress();
        }
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
