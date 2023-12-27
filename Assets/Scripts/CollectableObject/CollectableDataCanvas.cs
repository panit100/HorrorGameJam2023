using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectableDataCanvas : MonoBehaviour
{
    [Header("Canvas Group")]
    [SerializeField] CanvasGroup dataGroup;
    [SerializeField] CanvasGroup dataInfo;
    [Header("Data Group")]
    [SerializeField] Transform dataContainer;
    [SerializeField] CollectableDataSlot dataTemplate;
    
    [Header("Data Info")]
    [SerializeField] TMP_Text dataHeaderText;
    [SerializeField] TMP_Text dataContentText;

    List<GameObject> dataSlots = new List<GameObject>();
    
    [Button]
    void ShowDataSlots()
    {
        CreateDataInPlayerInventory();

        dataGroup.alpha = 1;
        dataGroup.interactable = true;
        dataGroup.blocksRaycasts = true;

        dataInfo.alpha = 0;
        dataInfo.interactable = false;
        dataInfo.blocksRaycasts = false;
    }

    void ShowDataInfo(string header,string content)
    {
        dataGroup.alpha = 0;
        dataGroup.interactable = false;
        dataGroup.blocksRaycasts = false;

        dataInfo.alpha = 1;
        dataInfo.interactable = true;
        dataInfo.blocksRaycasts = true;
        
        SetDataInfo(header,content);
    }
    
    void CreateDataInPlayerInventory()
    {
        ClearSlot();

        foreach (var item in PlayerManager.Instance.PlayerInventory.CollectableObjects)
        {
            CreateDataSlot(item);
        }
    }

    void CreateDataSlot(CollectableScriptableObject _obejct)
    {
        CollectableDataSlot newData = Instantiate(dataTemplate,dataContainer);
        newData.CollectableObject = _obejct;
        newData.dataHeaderText.text = _obejct.dataHeader;
        newData.GetComponent<Button>().onClick.AddListener(() => {
            string header = newData.CollectableObject.dataHeader;
            string content = newData.CollectableObject.dataContent;
            ShowDataInfo(header,content);
        });
        
        newData.gameObject.SetActive(true);
        dataSlots.Add(newData.gameObject);
    }

    [Button]
    void SetDataInfo(string header,string content)
    {
        dataHeaderText.text = header;
        dataContentText.text = content;
    }

    void ClearSlot()
    {
        foreach(var slot in dataSlots)
        {
            Destroy(slot);
        }

        dataSlots.Clear();
    }
}
