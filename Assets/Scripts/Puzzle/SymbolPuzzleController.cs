using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolPuzzleController : MonoBehaviour
{
    public CanvasGroup canvasGroup {get; private set;}

    [SerializeField] string code;

    private void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetCode(string _code)
    {
        code = _code;
    }
}
