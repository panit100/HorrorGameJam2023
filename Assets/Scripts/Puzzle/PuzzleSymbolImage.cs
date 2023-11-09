using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSymbolImage : MonoBehaviour
{
    int maxSymbolType = 5;

    [SerializeField] int currentSymbolType = 0;
    [SerializeField] Button nextSymbolButton;
    [SerializeField] Button previousSymbolButton;

    void Start() 
    {
        nextSymbolButton.onClick.AddListener(NextSymbol);
        previousSymbolButton.onClick.AddListener(PreviousSymbol);
    }

    public void NextSymbol()
    {
        currentSymbolType++;
        
        if(currentSymbolType > maxSymbolType)
            currentSymbolType = 0;
    }

    public void PreviousSymbol()
    {
        currentSymbolType--;
        
        if(currentSymbolType < 0)
            currentSymbolType = maxSymbolType;
    }

    public int GetSymbolType()
    {
        return currentSymbolType;
    }

}
