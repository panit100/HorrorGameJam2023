using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Events;

enum DialogueStatus
{
    START,
    TYPING,
    END,
}

public class DialogueManager : Singleton<DialogueManager>
{
    // public DialogueCanvas dialogueCanvasPrefab;

    [ContextMenuItem("OnPlayDialogue","OnPlayDialogue")]
    public string nextDialogueID;
    [Range(0f,1f)]
    public float textSpeed;

    DialogueData currentDialogueData = null;
    DialogueStatus dialogueStatus = DialogueStatus.END;

    Coroutine displayTextCoroutine;

    // DialogueCanvas dialogueCanvas;

    Dictionary<string, DialogueData> dialogueDataDictionary = new Dictionary<string, DialogueData>();

    bool canSkip = true;

    UnityAction startDialogueCallback,endDialogueCallback;

    protected override void InitAfterAwake()
    {   
        LoadDialogueFromCSV("DialogueTest");

        AddInputAction();
    }

    void Start()
    {
        CreateDialogueCanvas();
    }

    void AddInputAction()
    {
        // InputSystemManager.Instance.onNextDialogue += OnNextDialogue;
        // InputSystemManager.Instance.onNextDialogue += SpeedDialogue;
        // InputSystemManager.Instance.onSkipDialogue += SkipDialogue;
    }

    void RemoveInputAction()
    {
        // InputSystemManager.Instance.onNextDialogue -= OnNextDialogue;
        // InputSystemManager.Instance.onNextDialogue -= SpeedDialogue;
        // InputSystemManager.Instance.onSkipDialogue -= SkipDialogue;
    }

    void CreateDialogueCanvas()
    {
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;

        if(canvas != null)
        {
            // dialogueCanvas = Instantiate(dialogueCanvasPrefab,canvas);
            // SetActiveDialogueCanvas(false);
        }
        else
            throw new Exception("Canvas is not found. Please create canvas first or check tag in canvas object.");

    }

    void SetActiveDialogueCanvas(bool active)
    {
        // dialogueCanvas.gameObject.SetActive(active);
    }

    public void StartDialogue(DialogueData dialogueData, bool _canSkip = true,UnityAction _startDialogueCallback = null,UnityAction _endDialogueCallback = null)
    {
        // if(dialogueCanvas == null)
        // {
        //     Debug.LogError("Please create dialogue canvas first.");
        //     return;
        // }
        
        if(dialogueStatus != DialogueStatus.END)
        {
            Debug.LogError($"Dialogue {currentDialogueData.ID} is not finish.");
            return;
        }

        if(currentDialogueData == null)
        {
            SetActiveDialogueCanvas(true);

            // InputSystemManager.Instance.ToggleDialogueControl(true);

            dialogueStatus = DialogueStatus.START;

            canSkip = _canSkip;
            startDialogueCallback = _startDialogueCallback;
            endDialogueCallback = _endDialogueCallback;

            currentDialogueData = dialogueData;

            RunStartDialogueCallBack();

            UpdateDialogueUI(currentDialogueData);
        }
        else
        {
            Debug.LogError($"Dialogue didn't finish yet. {dialogueData.ID} : {dialogueData.DialogueText}" );
        }
    }

    public void NextDialogue(DialogueData dialogueData)
    {
        // if(dialogueCanvas == null)
        // {
        //     Debug.LogError("Dialogue canvas is missing");
        //     return;
        // }
        
        if(dialogueStatus != DialogueStatus.END)
        {
            Debug.LogError($"Dialogue {currentDialogueData.ID} is not finish.");
            return;
        }

        if(currentDialogueData == null)
        {
            dialogueStatus = DialogueStatus.START;

            currentDialogueData = dialogueData;

            UpdateDialogueUI(currentDialogueData);
        }
        else
        {
            Debug.LogError($"Dialogue didn't finish yet. {dialogueData.ID} : {dialogueData.DialogueText}" );
        }
    }

    void EndDialogue(DialogueData dialogueData)
    {
        if(currentDialogueData == dialogueData)
        {
            nextDialogueID = dialogueData.NextDialogueID;
            dialogueStatus = DialogueStatus.END;
        }
        else
        {
            Debug.LogError($"Dialogue : {dialogueData.ID} isn't running now.");
        }
    }

    public void SpeedDialogue()
    {
        if(dialogueStatus == DialogueStatus.TYPING)
        {
            StopCoroutine(displayTextCoroutine);
            UpdateDialogueText(currentDialogueData.DialogueText);
            EndDialogue(currentDialogueData);
        }
    }

    public void SkipDialogue()
    {
        if(!canSkip)
            return;

        StopCoroutine(displayTextCoroutine);
        OnLastDialogueEnd();
    }

    DialogueData GetNextDialogue(string dialogueID)
    {
        DialogueData nextDialogue = dialogueDataDictionary[dialogueID];
        return nextDialogue;
    }

    void ContinueDialogue()
    {
        if(IsDialogueNotEnd())
            return;

        if(IsCurrentDialogueNull())
            return;

        if(IsHaveNextDialogue())
        {
            currentDialogueData = null;
            NextDialogue(GetNextDialogue(nextDialogueID));
            nextDialogueID = "";
            return;
        }
        else
        {
            OnLastDialogueEnd();
            return;
        }
    }

    bool IsDialogueNotEnd()
    {
        if(dialogueStatus != DialogueStatus.END)
        {
            Debug.LogWarning("Dialogue didn't finish.");
            return true;
        }
        else
            return false;
    }

    bool IsCurrentDialogueNull()
    {
        if(currentDialogueData == null)
        {
            Debug.LogError("Don't have next dialogue.");
            return true;
        }
        else
            return false;
    }

    bool IsHaveNextDialogue()
    {
        if(currentDialogueData.NextDialogueID != "")
            return true;
        else
            return false;
    }

    void UpdateDialogueUI(DialogueData dialogueData)
    {
        // dialogueCanvas.UpdateDialogueUI(dialogueData);
        // displayTextCoroutine = StartCoroutine(DisplayText(dialogueData.DialogueText));
    }

    void UpdateDialogueText(string text)
    {
        // dialogueCanvas.UpdateDialogueText(text);
    }

    IEnumerator DisplayText(string dialogueText)
    {
        
        string _dialogueText = "";
        UpdateDialogueText(_dialogueText);
        yield return new WaitForSeconds(textSpeed);

        dialogueStatus = DialogueStatus.TYPING;
        foreach(var n in dialogueText.ToCharArray())
        {
            _dialogueText += n;
            UpdateDialogueText(_dialogueText);
            yield return new WaitForSeconds(textSpeed);
        }

        EndDialogue(currentDialogueData);
    }

    public void OnNextDialogue()
    {
        ContinueDialogue();
    }

    void OnLastDialogueEnd()
    {
        RunEndDialogueCallBack();

        currentDialogueData = null;
        dialogueStatus = DialogueStatus.END;
        canSkip = true;
        SetActiveDialogueCanvas(false);
        // InputSystemManager.Instance.ToggleDialogueControl(false);
    }

    void LoadDialogueFromJSON(string dialogueFile)
    {
        DialogueData data = JsonHelper.LoadJSONAsObject<DialogueData>(dialogueFile);

        dialogueDataDictionary.Add(data.ID,data);
    }

    void LoadDialogueFromCSV(string dialogueFile)
    {
        DialogueData[] dialogueDatas = CSVHelper.LoadCSVAsObject<DialogueData>(dialogueFile);

        foreach(var data in dialogueDatas)
        {
            dialogueDataDictionary.Add(data.ID,data);
        }
    }

    void RunStartDialogueCallBack()
    {
        startDialogueCallback?.Invoke();
        startDialogueCallback = null;   
    }

    void RunEndDialogueCallBack()
    {
        endDialogueCallback?.Invoke();
        endDialogueCallback = null;   
    }

#region  Test function
    void OnPlayDialogue()
    {
        StartDialogue(dialogueDataDictionary["test_0"],true);
    }
#endregion

    void OnDestroy() 
    {
        RemoveInputAction();
    }
}
