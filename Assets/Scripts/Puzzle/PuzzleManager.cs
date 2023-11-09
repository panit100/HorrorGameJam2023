using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : Singleton<PuzzleManager>
{
    public string currentObjectiveCode {get; private set;}
    [SerializeField] PuzzleCanvas puzzleCanvas;

    UnityEvent tempEvent;

    public bool isSolving {get; private set;}

    protected override void InitAfterAwake()
    {

    }

    public void SolvePuzzle(PuzzleType puzzleType,string objectiveCode,UnityEvent unityEvent = null)
    {
        if(puzzleCanvas == null)
            return;

        currentObjectiveCode = objectiveCode; 

        puzzleCanvas.StartSolvePuzzle(puzzleType,MainObjectiveManager.Instance.MainObjectiveDataDictionary[currentObjectiveCode].PuzzleCode);
        tempEvent = unityEvent;

        PlayerManager.Instance.OnChangePlayerState(PlayerState.puzzle);
    }

    public void OnPuzzleComplete()
    {
        MainObjectiveManager.Instance.UpdateProgress(currentObjectiveCode);
        tempEvent?.Invoke();
        tempEvent.RemoveAllListeners();

        puzzleCanvas.HideUI();

        PlayerManager.Instance.OnChangePlayerState(PlayerState.Move);
    }
}
