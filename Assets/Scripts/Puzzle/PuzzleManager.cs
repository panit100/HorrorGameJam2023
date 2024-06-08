using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : Singleton<PuzzleManager>
{
    public string currentObjectiveCode { get; private set; }
    [SerializeField] PuzzleCanvas puzzleCanvas;

    UnityEvent tempEvent;

    public bool isSolving { get; private set; }

    bool isDemo = false;

    protected override void InitAfterAwake()
    {

    }

    public void SolvePuzzle(PuzzleType puzzleType, string objectiveCode, UnityEvent unityEvent = null, bool isDemo = false)
    {
        this.isDemo = isDemo;

        if (puzzleCanvas == null)
            return;

        currentObjectiveCode = objectiveCode;

        if (!this.isDemo)
            puzzleCanvas.StartSolvePuzzle(puzzleType, MainObjectiveManager.Instance.MainObjectiveDataDictionary[currentObjectiveCode].PuzzleCode);
        else
            puzzleCanvas.StartSolvePuzzle(puzzleType, objectiveCode);
        tempEvent = unityEvent;

        PlayerManager.Instance.OnChangePlayerState(PlayerState.puzzle);
    }

    public void OnPuzzleComplete()
    {
        if (!isDemo)
            MainObjectiveManager.Instance.UpdateProgress(currentObjectiveCode);

        tempEvent?.Invoke();
        tempEvent.RemoveAllListeners();

        puzzleCanvas.HideUI();

        PlayerManager.Instance.OnChangePlayerState(PlayerState.Move);
    }

    public void OnClose()
    {
        tempEvent.RemoveAllListeners();
        puzzleCanvas.HideUI();
        PlayerManager.Instance.OnChangePlayerState(PlayerState.Move);
    }
}
