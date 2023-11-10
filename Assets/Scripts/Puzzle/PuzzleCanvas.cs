using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleCanvas : MonoBehaviour
{
    [SerializeField] CodePuzzleController codePuzzleController;
    [SerializeField] SymbolPuzzleController symbolPuzzleController;

    void Start() 
    {
        HideUI();
    }

    public void StartSolvePuzzle(PuzzleType puzzleType,string code)
    {
        SetCode(puzzleType,code);
        ShowUI(puzzleType);
    }

    void SetCode(PuzzleType puzzleType,string code)
    {
        switch(puzzleType)
        {
            case PuzzleType.Code:
                codePuzzleController.SetCode(code);
                break;
            case PuzzleType.Symbol:
                symbolPuzzleController.SetCode(code);
                break;
        }
    }

    public void ShowUI(PuzzleType puzzleType)
    {
        switch(puzzleType)
        {
            case PuzzleType.Code:
                codePuzzleController.canvasGroup.alpha = 1;
                codePuzzleController.canvasGroup.interactable = true;
                codePuzzleController.canvasGroup.blocksRaycasts = true;
                break;
            case PuzzleType.Symbol:
                symbolPuzzleController.canvasGroup.alpha = 1;
                symbolPuzzleController.canvasGroup.interactable = true;
                symbolPuzzleController.canvasGroup.blocksRaycasts = true;
                break;
        }
    }

    public void HideUI()
    {
        codePuzzleController.canvasGroup.alpha = 0;
        codePuzzleController.canvasGroup.interactable = false;
        codePuzzleController.canvasGroup.blocksRaycasts = false;

        symbolPuzzleController.canvasGroup.alpha = 0;
        symbolPuzzleController.canvasGroup.interactable = false;
        symbolPuzzleController.canvasGroup.blocksRaycasts = false;
    }
}
