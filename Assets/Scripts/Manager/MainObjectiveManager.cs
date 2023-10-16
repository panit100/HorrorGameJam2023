using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectiveManager : Singleton<MainObjectiveManager>
{
    protected override void InitAfterAwake(){}

    public GameObject currentObjective;
    [SerializeField] int objectiveIndex = 0;
    public List<GameObject> objectiveItems;
    private void Start()
    {
        if (objectiveItems.Count == 0)
        {
            return;
        }
        currentObjective = objectiveItems[objectiveIndex];
    }

    public void GetCheckObjective(GameObject checkObjective)
    {
        if (checkObjective == currentObjective)
        {
            checkObjective.SetActive(false);
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
            print("No Objective");
        }
    }
}
