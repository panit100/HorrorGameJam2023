using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodePuzzleController : MonoBehaviour
{
    [SerializeField] string code;

    [SerializeField] TMP_InputField codeInput;
    [SerializeField] Button confirmButton;
    [SerializeField] Button closeButton;
    public CanvasGroup canvasGroup { get; private set; }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        closeButton.onClick.AddListener(OnClose);
    }

    public void SetCode(string _code)
    {
        code = _code;
        ResetText();
    }

    void ResetText()
    {
        codeInput.text = "000000";
    }

    void OnConfirm()
    {
        if (!codeInput.text.Trim().Equals(code.Trim()))
            return;

        PuzzleManager.Instance.OnPuzzleComplete();
    }

    void OnClose()
    {
        PuzzleManager.Instance.OnClose();
    }
}
