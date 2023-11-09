using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolPuzzleController : MonoBehaviour
{
    public CanvasGroup canvasGroup {get; private set;}

    [SerializeField] string code;
    [SerializeField] Button confirmButton;

    [SerializeField] List<PuzzleSymbolImage> puzzleSymbolImages = new List<PuzzleSymbolImage>();

    void Start() 
    {
        canvasGroup = GetComponent<CanvasGroup>();
        confirmButton.onClick.AddListener(OnConfirm);
    }

    public void SetCode(string _code)
    {
        code = _code;
    }

    string GetSymbolCode()
    {
        string _code = "";
        foreach(var n in puzzleSymbolImages)
        {
            _code += n.GetSymbolType();
        }

        return _code;
    }

    void OnConfirm()
    {   
        if(!GetSymbolCode().Equals(code.Trim()))
            return;

        PuzzleManager.Instance.OnPuzzleComplete();
    }
}
