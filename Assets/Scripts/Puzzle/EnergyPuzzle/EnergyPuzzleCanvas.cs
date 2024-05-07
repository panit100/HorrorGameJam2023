using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPuzzleCanvas : MonoBehaviour
{
    public CanvasGroup canvasGroup {get; private set;}

    [Header("UnuseEnergy")]
    [SerializeField] CanvasEnergySlotConfig unuseEnergySlotConfig;
    [Header("EnergyConfig")]
    [SerializeField] List<CanvasEnergySlotConfig> energySlotConfigs = new List<CanvasEnergySlotConfig>();

    [SerializeField] Button closeButton;

    public Action onHideCanvas;
    
    void Awake() 
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start() 
    {
        HideCanvas();

        closeButton.onClick.AddListener(HideCanvas);
    }

    public void ShowCanvas(EnergyPuzzleController energyPuzzleController = null)
    {
        if(energyPuzzleController != null)
            AddListenerToButton(energyPuzzleController);

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideCanvas()
    {
        onHideCanvas?.Invoke();
        RemoveListenerInButton();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        ClearEnergySlot();
    }

    public void AddListenerToButton(EnergyPuzzleController energyPuzzleController)
    {
        for(int i = 0; i < energyPuzzleController.EnergyConfigs.Count; i++)
        {
            int configIndex = i;
            energySlotConfigs[configIndex].addEnergyButton.onClick.AddListener(() => {  
                if(energyPuzzleController.AddEnergy(configIndex))
                    {
                        energySlotConfigs[configIndex].OnAddEnergy();
                        unuseEnergySlotConfig.OnRemoveEnergy();
                    }
                });
            energySlotConfigs[configIndex].removeEnergyButton.onClick.AddListener(() => {  
                if(energyPuzzleController.RemoveEnergy(configIndex))
                    {
                        energySlotConfigs[configIndex].OnRemoveEnergy();
                        unuseEnergySlotConfig.OnAddEnergy();
                    }
                });
        }
    }

    void RemoveListenerInButton()
    {
        foreach(var n in energySlotConfigs)
        {
            n.addEnergyButton.onClick.RemoveAllListeners();
            n.removeEnergyButton.onClick.RemoveAllListeners();
        }
    }

    public void CreateEnergyAllSlot(int maxEnergy, int unuseEnergy, List<EnergyConfig> energyConfigs)
    {
        ClearEnergySlot();

        CreateEnergySlot(unuseEnergySlotConfig,maxEnergy,unuseEnergy);
        
        for(int i = 0; i < energyConfigs.Count; i++)
        {
            CreateEnergySlot(energySlotConfigs[i],energyConfigs[i].maxEnergy,energyConfigs[i].currentEnergy);
        }
    }

    void CreateEnergySlot(CanvasEnergySlotConfig energySlotConfig,int maxEnergy,int currentEnergy)
    {
        energySlotConfig.energyTemplate.gameObject.SetActive(false);

        for(int i = 0; i < maxEnergy-currentEnergy; i++)
        {
            Transform energySlot = Instantiate(energySlotConfig.energyTemplate,energySlotConfig.energyContainer);
            energySlotConfig.energySlotList.Add(energySlot);
            energySlot.gameObject.SetActive(false);
            energySlotConfig.activeEnergyQueue.Enqueue(energySlot);
        }

        for(int i = 0; i < currentEnergy; i++)
        {
            Transform energySlot = Instantiate(energySlotConfig.energyTemplate,energySlotConfig.energyContainer);
            energySlotConfig.energySlotList.Add(energySlot);
            energySlot.gameObject.SetActive(true);
            energySlotConfig.deactiveEnergyQueue.Enqueue(energySlot);
        }
    }

    void ClearEnergySlot()
    {
        foreach(var n in unuseEnergySlotConfig.energySlotList)
        {
            Destroy(n.gameObject);
        }
        unuseEnergySlotConfig.energySlotList.Clear();
        unuseEnergySlotConfig.activeEnergyQueue.Clear();
        unuseEnergySlotConfig.deactiveEnergyQueue.Clear();

        foreach(var n in energySlotConfigs)
        {
            foreach(var m in n.energySlotList)
            {
                Destroy(m.gameObject);
            }
            n.energySlotList.Clear();
            n.activeEnergyQueue.Clear();
            n.deactiveEnergyQueue.Clear();
        }
    }
}

[Serializable]
public class CanvasEnergySlotConfig
{
    [Header("Button")]
    public Button addEnergyButton;
    public Button removeEnergyButton;

    public Transform energyContainer;
    public Transform energyTemplate;
    public List<Transform> energySlotList = new List<Transform>();
    public Queue<Transform> activeEnergyQueue = new Queue<Transform>();
    public Queue<Transform> deactiveEnergyQueue = new Queue<Transform>();

    public void OnAddEnergy()
    {
        Transform energySlot = activeEnergyQueue.Dequeue();
        energySlot.gameObject.SetActive(true);
        deactiveEnergyQueue.Enqueue(energySlot);
    }

    public void OnRemoveEnergy()
    {
        Transform energySlot = deactiveEnergyQueue.Dequeue();
        energySlot.gameObject.SetActive(false);
        activeEnergyQueue.Enqueue(energySlot);
    }
    
}
