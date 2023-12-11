using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPuzzleCanvas : MonoBehaviour
{
    public CanvasGroup canvasGroup {get; private set;}

    [Header("Light")]
    [SerializeField] Button addLightEnergy;
    [SerializeField] Button removeLightEnergy;
    
    [Header("Door")]
    [SerializeField] Button addDoorEnergy;
    [SerializeField] Button removeDoorEnergy;
    [Header("Bridge")]
    [SerializeField] Button addBridgeEnergy;
    [SerializeField] Button removeBridgeEnergy;

    [Header("Energy")]
    [SerializeField] CanvasEnergySlotConfig unuseEnergySlotConfig;
    [SerializeField] CanvasEnergySlotConfig lightEnergySlotConfig;
    [SerializeField] CanvasEnergySlotConfig doorEnergySlotConfig;
    [SerializeField] CanvasEnergySlotConfig bridgeEnergySlotConfig;


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
        addLightEnergy.onClick.AddListener(() => {  
            if(energyPuzzleController.AddEnergy(ENERGYTYPE.LIGHT))
                {
                    lightEnergySlotConfig.OnAddEnergy();
                    unuseEnergySlotConfig.OnRemoveEnergy();
                }
            });
        removeLightEnergy.onClick.AddListener(() => {  
            if(energyPuzzleController.RemoveEnergy(ENERGYTYPE.LIGHT))
                {
                    lightEnergySlotConfig.OnRemoveEnergy();
                    unuseEnergySlotConfig.OnAddEnergy();
                }
            });

        addDoorEnergy.onClick.AddListener(() => {  
            if(energyPuzzleController.AddEnergy(ENERGYTYPE.DOOR))
                {
                    doorEnergySlotConfig.OnAddEnergy();
                    unuseEnergySlotConfig.OnRemoveEnergy();
                }
            });
        removeDoorEnergy.onClick.AddListener(() => {  
            if(energyPuzzleController.RemoveEnergy(ENERGYTYPE.DOOR))
                {
                    doorEnergySlotConfig.OnRemoveEnergy();
                    unuseEnergySlotConfig.OnAddEnergy();
                }
            });

        addBridgeEnergy.onClick.AddListener(() => {  
            if(energyPuzzleController.AddEnergy(ENERGYTYPE.BRIDGE))
                {
                    bridgeEnergySlotConfig.OnAddEnergy();
                    unuseEnergySlotConfig.OnRemoveEnergy();
                }
            });
        removeBridgeEnergy.onClick.AddListener(() => {  
            if(energyPuzzleController.RemoveEnergy(ENERGYTYPE.BRIDGE))
                {
                    bridgeEnergySlotConfig.OnRemoveEnergy();
                    unuseEnergySlotConfig.OnAddEnergy();
                }
            });
    }

    void RemoveListenerInButton()
    {
        addLightEnergy.onClick.RemoveAllListeners();
        removeLightEnergy.onClick.RemoveAllListeners();
        addDoorEnergy.onClick.RemoveAllListeners();
        removeDoorEnergy.onClick.RemoveAllListeners();
        addBridgeEnergy.onClick.RemoveAllListeners();
        removeBridgeEnergy.onClick.RemoveAllListeners();
    }

    public void CreateEnergyAllSlot(int maxEnergy, int unuseEnergy, int maxLightEnergy, int currentLightEnergy, int maxDoorEnergy, int currentDoorEnergy, int maxBridgeEnergy, int currentBridgeEnergy)
    {
        ClearEnergySlot();

        CreateEnergySlot(unuseEnergySlotConfig,maxEnergy,unuseEnergy);
        CreateEnergySlot(lightEnergySlotConfig,maxLightEnergy,currentLightEnergy);
        CreateEnergySlot(doorEnergySlotConfig,maxDoorEnergy,currentDoorEnergy);
        CreateEnergySlot(bridgeEnergySlotConfig,maxBridgeEnergy,currentBridgeEnergy);
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

        foreach(var n in lightEnergySlotConfig.energySlotList)
        {
            Destroy(n.gameObject);
        }
        lightEnergySlotConfig.energySlotList.Clear();
        lightEnergySlotConfig.activeEnergyQueue.Clear();
        lightEnergySlotConfig.deactiveEnergyQueue.Clear();

        foreach(var n in doorEnergySlotConfig.energySlotList)
        {
            Destroy(n.gameObject);
        }
        doorEnergySlotConfig.energySlotList.Clear();
        lightEnergySlotConfig.activeEnergyQueue.Clear();
        lightEnergySlotConfig.deactiveEnergyQueue.Clear();

        foreach(var n in bridgeEnergySlotConfig.energySlotList)
        {
            Destroy(n.gameObject);
        }
        bridgeEnergySlotConfig.energySlotList.Clear();
        lightEnergySlotConfig.activeEnergyQueue.Clear();
        lightEnergySlotConfig.deactiveEnergyQueue.Clear();
    }
}

[Serializable]
public class CanvasEnergySlotConfig
{
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
